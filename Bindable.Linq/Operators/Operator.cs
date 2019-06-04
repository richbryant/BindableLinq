using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Operators
{
    public abstract class Operator<TSource, TResult> : DispatcherBound, IBindable<TResult>, IAcceptsDependencies
    {
	    private readonly List<IDependency> _dependencies = new List<IDependency>();

        private TResult _current;

	    private bool _hasEvaluated;

	    public IBindable<TSource> Source { get; }

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

	    protected Operator(IBindable<TSource> source, IDispatcher dispatcher)
		    : base(dispatcher)
	    {
		    Source = source;
		    var source2 = Source;

            void Handler(object sender, PropertyChangedEventArgs e)
            {
                Dispatcher.Dispatch(Refresh);
            }

            source2.PropertyChanged += Weak.Event((EventHandler<PropertyChangedEventArgs>) Handler).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
	    }

	    protected abstract void RefreshOverride();

	    public void Refresh()
	    {
		    AssertDispatcherThread();
		    HasEvaluated = false;
		    Evaluate();
	    }

	    public void AcceptDependency(IDependencyDefinition definition)
	    {
            if (!definition.AppliesToSingleElement()) return;
            var dependency = definition.ConstructForElement(Source, BindingConfigurations.Default.CreatePathNavigator());
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