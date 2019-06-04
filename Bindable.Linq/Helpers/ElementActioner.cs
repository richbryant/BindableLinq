using System;
using System.Collections.Specialized;
using Bindable.Linq.Collections;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Helpers
{
    internal sealed class ElementActioner<TElement> : DispatcherBound
{
	private readonly WeakEvent<NotifyCollectionChangedEventArgs> _collection_CollectionChangedHandler;

	private readonly Action<TElement> _addAction;

	private readonly IBindableCollection<TElement> _collection;

	private readonly BindableCollection<TElement> _copy;

	private readonly object _object = new object();

	private readonly Action<TElement> _removeAction;

	public ElementActioner(IBindableCollection<TElement> collection, Action<TElement> addAction, Action<TElement> removeAction, IDispatcher dispatcher)
		: base(dispatcher)
	{
		_addAction = addAction;
		_removeAction = removeAction;
		_collection = collection;
		_copy = new BindableCollection<TElement>(dispatcher);
		_collection_CollectionChangedHandler = Weak.Event<NotifyCollectionChangedEventArgs>(Collection_CollectionChanged);
		_collection.CollectionChanged += _collection_CollectionChangedHandler.HandlerProxy.Handler;
		if (!(collection?.HasEvaluated ?? true))
		{
			collection.Evaluating += delegate(object sender, EvaluatingEventArgs<TElement> e)
			{
				foreach (var item in e.ItemsYieldedFromEvaluation)
				{
					HandleElement(NotifyCollectionChangedAction.Add, item);
					_copy.Add(item);
				}
			};
		}
		else
		{
			_collection.ForEach(delegate(TElement element)
			{
				HandleElement(NotifyCollectionChangedAction.Add, element);
				_copy.Add(element);
			});
		}
	}

	private void HandleElement(NotifyCollectionChangedAction action, TElement element)
	{
		switch (action)
		{
		case NotifyCollectionChangedAction.Add:
			_addAction(element);
			break;
		case NotifyCollectionChangedAction.Remove:
			_removeAction(element);
			break;
		}
	}

	private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		switch (e.Action)
		{
		case NotifyCollectionChangedAction.Move:
			break;
		case NotifyCollectionChangedAction.Add:
			foreach (TElement newItem in e.NewItems)
			{
				HandleElement(NotifyCollectionChangedAction.Add, newItem);
				_copy.Add(newItem);
			}
			break;
		case NotifyCollectionChangedAction.Remove:
			foreach (TElement oldItem in e.OldItems)
			{
				HandleElement(NotifyCollectionChangedAction.Remove, oldItem);
				_copy.Remove(oldItem);
			}
			break;
		case NotifyCollectionChangedAction.Replace:
			foreach (TElement oldItem2 in e.OldItems)
			{
				HandleElement(NotifyCollectionChangedAction.Remove, oldItem2);
				_copy.Remove(oldItem2);
			}
			foreach (TElement newItem2 in e.NewItems)
			{
				HandleElement(NotifyCollectionChangedAction.Add, newItem2);
				_copy.Add(newItem2);
			}
			break;
		case NotifyCollectionChangedAction.Reset:
			HandleReset();
			break;
		}
	}

	private void HandleReset()
	{
		_copy.ForEach(delegate(TElement a)
		{
			HandleElement(NotifyCollectionChangedAction.Remove, a);
		});

		_collection.ForEach(delegate(TElement a)
		{
			HandleElement(NotifyCollectionChangedAction.Add, a);
		});
		_copy.Clear();
		_copy.AddRange(_collection);
	}

	protected override void BeforeDisposeOverride()
	{
		if (_collection != null)
		{
			_copy.ForEach(delegate(TElement e)
			{
				HandleElement(NotifyCollectionChangedAction.Remove, e);
			});
			_collection.CollectionChanged -= _collection_CollectionChangedHandler.HandlerProxy.Handler;
		}
		base.BeforeDisposeOverride();
	}
}
}