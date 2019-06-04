using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Linq.Interfaces.Internal;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Collections
{
    internal sealed class BindableGrouping<TKey, TElement> : DispatcherBound, IBindableGrouping<TKey, TElement>
    {
	    private readonly IBindableCollection<TElement> _groupWhereQuery;

        public bool HasEvaluated => true;

	    public TKey Key { get; }

        public int Count => _groupWhereQuery.Count;

	    public TElement this[int index] => _groupWhereQuery[index];

        public event PropertyChangedEventHandler PropertyChanged;

	    public event EvaluatingEventHandler<TElement> Evaluating;

	    public event NotifyCollectionChangedEventHandler CollectionChanged;

	    public BindableGrouping(TKey key, IBindableCollection<TElement> groupWhereQuery, IDispatcher dispatcher)
		    : base(dispatcher)
	    {
		    Key = key;
		    _groupWhereQuery = groupWhereQuery;
		    var groupWhereQuery2 = _groupWhereQuery;

            void Handler(object sender, EvaluatingEventArgs<TElement> e)
            {
                OnEvaluating(e);
            }

            groupWhereQuery2.Evaluating += Handler;
		    _groupWhereQuery.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
		    {
			    OnCollectionChanged(e);
		    };
		    _groupWhereQuery.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
		    {
			    OnPropertyChanged(e);
		    };
	    }

	    public void Evaluate()
	    {
		    var _ = GetEnumerator();
            _.Dispose();
	    }

	    public IEnumerator<TElement> GetEnumerator()
	    {
		    return _groupWhereQuery.GetEnumerator();
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    public void Refresh()
	    {
		    _groupWhereQuery.Refresh();
	    }

	    public void AcceptDependency(IDependencyDefinition definition)
	    {
		    throw new NotSupportedException("This object cannot accept dependencies directly");
	    }

	    private void OnPropertyChanged(PropertyChangedEventArgs e)
	    {
		    AssertDispatcherThread();
		    PropertyChanged?.Invoke(this, e);
	    }

	    private void OnEvaluating(EvaluatingEventArgs<TElement> e)
	    {
		    AssertDispatcherThread();
		    Evaluating?.Invoke(this, e);
	    }

	    private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	    {
		    AssertDispatcherThread();
		    CollectionChanged?.Invoke(this, e);
	    }
    }

}