using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Bindable.Linq.Collections;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Iterators
{
    public abstract class Iterator<TSource, TResult> : DispatcherBound, IBindableCollection<TResult>
    {
        private readonly List<IDependency> _dependencies = new List<IDependency>();

        private bool _hasEvaluated;

	    protected StateScope CollectionChangedSuspendedState { get; } = new StateScope();

        protected BindableCollection<TResult> ResultCollection { get; }

        protected IBindableCollection<TSource> SourceCollection { get; }

        public TResult this[int index]
	    {
		    get
		    {
			    AssertDispatcherThread();
			    Evaluate();
			    return ResultCollection[index];
		    }
	    }

	    public int Count
	    {
		    get
		    {
			    AssertDispatcherThread();
			    Evaluate();
			    return ResultCollection.Count;
		    }
	    }

	    public bool HasEvaluated
	    {
		    get => _hasEvaluated;
            private set
		    {
			    _hasEvaluated = value;
			    OnPropertyChanged(CommonEventArgsCache.HasEvaluated);
		    }
	    }

	    public event PropertyChangedEventHandler PropertyChanged;

	    public event EvaluatingEventHandler<TResult> Evaluating;

	    public event NotifyCollectionChangedEventHandler CollectionChanged;

	    protected Iterator(IBindableCollection<TSource> sourceCollection, IDispatcher dispatcher)
		    : base(dispatcher)
	    {
		    ResultCollection = new BindableCollection<TResult>(dispatcher);
		    var resultCollection = ResultCollection;

            void Handler(object sender, NotifyCollectionChangedEventArgs e)
            {
                OnCollectionChanged(e);
            }

            resultCollection.CollectionChanged += Handler;
		    SourceCollection = sourceCollection;
		    SourceCollection.CollectionChanged += Weak.Event(delegate(object sender, NotifyCollectionChangedEventArgs e)
		    {
			    var iterator = this;
			    Dispatcher.Dispatch(delegate
			    {
				    iterator.ReactToCollectionChanged(e);
			    });
		    }).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
	    }

	    public void Evaluate()
	    {
		    AssertDispatcherThread();
            if (HasEvaluated) return;
            Seal();
            HasEvaluated = true;
            EvaluateSourceCollection();
            OnEvaluating(new EvaluatingEventArgs<TResult>(ResultCollection.EnumerateSafely()));
        }

	    public void Refresh()
	    {
		    AssertDispatcherThread();
		    if (HasEvaluated)
		    {
			    SourceCollection.Refresh();
		    }
	    }

	    public IEnumerator<TResult> GetEnumerator()
	    {
		    AssertDispatcherThread();
		    using (CollectionChangedSuspendedState.Enter())
		    {
			    Evaluate();
		    }
		    return ResultCollection.GetEnumerator();
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    public void AcceptDependency(IDependencyDefinition definition)
	    {
		    AssertDispatcherThread();
		    AssertUnsealed();
            if (!definition.AppliesToCollections()) return;

            var dependency = definition.ConstructForCollection(SourceCollection, BindingConfigurations.Default.CreatePathNavigator());

            dependency.SetReevaluateElementCallback(delegate(object element, string propertyName)
            {
                ReactToItemPropertyChanged((TSource)element, propertyName);
            });

            dependency.SetReevaluateCallback(delegate
            {
                ReactToReset();
            });

            _dependencies.Add(dependency);
        }

	    private static void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
	    {
		    if (e.NewItems != null && e.NewItems.Count > 1)
		    {
			    throw new NotSupportedException();
		    }
		    if (e.OldItems != null && e.OldItems.Count > 1)
		    {
			    throw new NotSupportedException();
		    }
		    if (e.OldItems != null && (e.NewItems != null && (e.Action == NotifyCollectionChangedAction.Replace && e.NewItems.Count != e.OldItems.Count)))
		    {
			    throw new NotSupportedException();
		    }
	    }

	    protected void ReactToReset()
	    {
		    BeforeResetOverride();
		    ResultCollection.Clear();
		    HasEvaluated = false;
		    Evaluate();
	    }

	    private void ReactToCollectionChanged(NotifyCollectionChangedEventArgs e)
	    {
		    AssertDispatcherThread();
		    if (!HasEvaluated)
		    {
			    return;
		    }
		    ValidateCollectionChangedEventArgs(e);
		    switch (e.Action)
		    {
		    case NotifyCollectionChangedAction.Add:
			    if (e.NewItems.Count == 1)
			    {
				    ReactToAdd(e.NewStartingIndex, (TSource)e.NewItems[0]);
			    }
			    break;
		    case NotifyCollectionChangedAction.Move:
			    if (e.OldItems.Count == 1)
			    {
				    ReactToMove(e.OldStartingIndex, e.NewStartingIndex, (TSource)e.OldItems[0]);
			    }
			    break;
		    case NotifyCollectionChangedAction.Remove:
			    if (e.OldItems.Count == 1)
			    {
				    ReactToRemove(e.OldStartingIndex, (TSource)e.OldItems[0]);
			    }
			    break;
		    case NotifyCollectionChangedAction.Replace:
			    if (e.NewItems.Count == 1 && e.OldItems.Count == 1)
			    {
				    ReactToReplace(e.OldStartingIndex, (TSource)e.OldItems[0], (TSource)e.NewItems[0]);
			    }
			    break;
		    case NotifyCollectionChangedAction.Reset:
			    ReactToReset();
			    break;
		    }
	    }

	    protected abstract void EvaluateSourceCollection();

	    protected abstract void ReactToItemPropertyChanged(TSource item, string propertyName);

	    protected abstract void ReactToAdd(int insertionIndex, TSource addedItem);

	    protected abstract void ReactToReplace(int index, TSource oldItem, TSource newItem);

	    protected abstract void ReactToMove(int originalIndex, int newIndex, TSource movedItem);

	    protected abstract void ReactToRemove(int index, TSource removedItem);

	    protected virtual void BeforeResetOverride()
	    {
	    }

	    public override string ToString()
	    {
		    var str = string.Format(CultureInfo.InvariantCulture, "{0}<{1},{2}>", GetType().Name, typeof(TSource).Name, typeof(TResult).Name);
		    if (HasEvaluated)
		    {
			    return str + $" - Evaluated - Count: {Count}";
		    }
		    return str + " - Not Evaluated";
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

	    protected virtual void OnEvaluating(EvaluatingEventArgs<TResult> e)
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

	    protected override void BeforeDisposeOverride()
	    {
		    foreach (var dependency in _dependencies)
		    {
			    dependency.Dispose();
		    }
	    }
    }
}