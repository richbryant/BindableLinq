using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Adapters.Incoming
{
    internal abstract class BindableCollectionAdapterBase<TElement> : DispatcherBound, IBindableCollection<TElement>
{
    private readonly IEnumerable _sourceCollection;

	private bool _hasEvaluated;

	public bool HasEvaluated
	{
		get => _hasEvaluated;
        private set
		{
			AssertDispatcherThread();
			_hasEvaluated = value;
			OnPropertyChanged(CommonEventArgsCache.HasEvaluated);
		}
	}

	protected StateScope CollectionChangedSuspendedState { get; } = new StateScope();

    public TElement this[int index]
	{
		get
		{
			if (Dispatcher.DispatchRequired())
			{
				return Dispatcher.Dispatch(() => this[index]);
			}
			var num = 0;
			using (var enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.Current;
					if (index == num)
					{
						return current;
					}
					num++;
				}
			}
			throw new IndexOutOfRangeException();
		}
	}

	public int Count
	{
		get
		{
			if (Dispatcher.DispatchRequired())
			{
				return Dispatcher.Dispatch(() => Count);
			}
			var num = 0;
			using (var enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.Current;
					num++;
				}
			}
			return num;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public event EvaluatingEventHandler<TElement> Evaluating;

	public event NotifyCollectionChangedEventHandler CollectionChanged;

	protected BindableCollectionAdapterBase(IEnumerable sourceCollection, IDispatcher dispatcher)
		: base(dispatcher)
	{
		_sourceCollection = sourceCollection;
	}

	public void Evaluate()
	{
		GetEnumerator();
	}

	public IEnumerator<TElement> GetEnumerator()
	{
		if (Dispatcher.DispatchRequired())
		{
			return Dispatcher.Dispatch(GetEnumerator);
		}
		using (CollectionChangedSuspendedState.Enter())
		{
			var list = new List<TElement>();
			foreach (var item in _sourceCollection)
			{
				if (item is TElement element)
				{
					list.Add(element);
				}
				else if (item != null)
				{
					throw new InvalidCastException("Could not cast object of type {0} to type {1}".FormatWith(item.GetType(), typeof(TElement)));
				}
			}

            if (HasEvaluated) return list.GetEnumerator();
            HasEvaluated = true;
            OnEvaluating(new EvaluatingEventArgs<TElement>(list));
            return list.GetEnumerator();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Refresh()
	{
		if (Dispatcher.DispatchRequired())
		{
			Dispatcher.Dispatch(Refresh);
			return;
		}
		HasEvaluated = false;
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	public void AcceptDependency(IDependencyDefinition definition)
	{
		throw new NotSupportedException("This object cannot accept dependencies directly.");
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		AssertDispatcherThread();
		var propertyChanged = PropertyChanged;
		if (propertyChanged != null && !CollectionChangedSuspendedState.IsWithin)
		{
			propertyChanged(this, e);
		}
	}

	protected virtual void OnEvaluating(EvaluatingEventArgs<TElement> e)
	{
		AssertDispatcherThread();
		Evaluating?.Invoke(this, e);
		OnPropertyChanged(CommonEventArgsCache.Count);
	}

	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		AssertDispatcherThread();
		var collectionChanged = CollectionChanged;
		if (collectionChanged != null && !CollectionChangedSuspendedState.IsWithin)
		{
			collectionChanged(this, e);
		}
		OnPropertyChanged(CommonEventArgsCache.Count);
	}
}

}