using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Aggregators
{
    public abstract class Aggregator<TSource, TResult> : DispatcherBound, IBindable<TResult>, IAcceptsDependencies
    {
	    private readonly List<IDependency> _dependencies = new List<IDependency>();

        private TResult _current;

	    private bool _hasEvaluated;

	    protected IBindableCollection<TSource> SourceCollection { get; }

        public TResult Current
	    {
		    get
		    {
			    AssertDispatcherThread();
			    Evaluate();
			    return _current;
		    }
		    protected set
		    {
			    AssertDispatcherThread();
			    _current = value;
			    OnPropertyChanged(CommonEventArgsCache.Current);
		    }
	    }

	    public bool HasEvaluated
	    {
		    get => _hasEvaluated;
            set
		    {
			    AssertDispatcherThread();
			    _hasEvaluated = value;
			    OnPropertyChanged(CommonEventArgsCache.HasEvaluated);
		    }
	    }

	    public event PropertyChangedEventHandler PropertyChanged;

	    protected Aggregator(IBindableCollection<TSource> sourceCollection, IDispatcher dispatcher)
		    : base(dispatcher)
	    {
		    SourceCollection = sourceCollection;
		    var sourceCollection2 = SourceCollection;

            void Handler(object sender, NotifyCollectionChangedEventArgs e)
            {
                Dispatcher.Dispatch(Refresh);
            }

            sourceCollection2.CollectionChanged += Weak.Event((EventHandler<NotifyCollectionChangedEventArgs>) Handler).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
	    }

	    public void AcceptDependency(IDependencyDefinition definition)
	    {
            if (!definition.AppliesToCollections()) return;
            var dependency = definition.ConstructForCollection(SourceCollection, BindingConfigurations.Default.CreatePathNavigator());
            dependency.SetReevaluateCallback(delegate
            {
                Refresh();
            });
            _dependencies.Add(dependency);
        }

	    public void Evaluate()
	    {
		    AssertDispatcherThread();
		    Seal();
            if (HasEvaluated) return;
            HasEvaluated = true;
            RefreshOverride();
        }

	    public void Refresh()
	    {
		    AssertDispatcherThread();
		    HasEvaluated = false;
		    Evaluate();
	    }

	    protected abstract void RefreshOverride();

	    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	    {
		    AssertDispatcherThread();
		    PropertyChanged?.Invoke(this, e);
	    }

	    protected override void BeforeDisposeOverride()
	    {
		    base.BeforeDisposeOverride();
		    foreach (var dependency in _dependencies)
		    {
			    dependency.Dispose();
		    }
	    }
    }
}