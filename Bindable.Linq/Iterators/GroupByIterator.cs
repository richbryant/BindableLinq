using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using Bindable.Linq.Collections;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Internal;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Iterators
{
    internal sealed class GroupByIterator<TKey, TSource, TElement> : Iterator<TSource, IBindableGrouping<TKey, TElement>> where TSource : class
    {
	    private readonly Expression<Func<TSource, TElement>> _elementSelector;

	    private readonly IEqualityComparer<TKey> _keyComparer;

	    private readonly Expression<Func<TSource, TKey>> _keySelector;

	    private readonly Func<TSource, TKey> _keySelectorCompiled;

	    public GroupByIterator(IBindableCollection<TSource> sourceCollection, Expression<Func<TSource, TKey>> keySelector,
                                Expression<Func<TSource, TElement>> elementSelector, 
                                IEqualityComparer<TKey> keyComparer, IDispatcher dispatcher)
		    : base(sourceCollection, dispatcher)
	    {
		    _keySelector = keySelector;
		    _keySelectorCompiled = keySelector.Compile();
		    _elementSelector = elementSelector;
		    _keyComparer = keyComparer;
	    }

	    protected override void EvaluateSourceCollection()
	    {
		    foreach (var item in SourceCollection)
		    {
			    ReactToAdd(-1, item);
		    }
	    }

	    public TKey KeySelector(TSource sourceItem)
	    {
		    return _keySelectorCompiled(sourceItem);
	    }

	    private bool CompareKeys(TKey lhs, TKey rhs)
	    {
		    return _keyComparer.Equals(lhs, rhs);
	    }

	    private bool FindGroup(TKey key, IEnumerable<IBindableGrouping<TKey, TElement>> groups)
        {
            return groups.Cast<IBindableGrouping<TKey, TSource>>().Any(@group => CompareKeys(@group.Key, key));
        }

	    private void EnsureGroupsExists(TSource element)
	    {
		    var key = _keySelectorCompiled(element);
            if (FindGroup(key, ResultCollection)) return;
            IBindableGrouping<TKey, TElement> bindableGrouping = new BindableGrouping<TKey, TElement>(key, (from e in SourceCollection
                where CompareKeys(_keySelectorCompiled(e), key)
                select e).WithDependencyExpression(_keySelector.Body, _keySelector.Parameters[0]).Select(_elementSelector), Dispatcher);
            bindableGrouping.CollectionChanged += Group_CollectionChanged;
            ResultCollection.Add(bindableGrouping);
        }

	    protected override void ReactToAdd(int sourceStartingIndex, TSource addedItem)
	    {
		    EnsureGroupsExists(addedItem);
	    }

	    protected override void ReactToMove(int oldIndex, int newIndex, TSource movedItem)
	    {
	    }

	    protected override void ReactToRemove(int oldIndex, TSource removedItem)
	    {
	    }

	    protected override void ReactToReplace(int oldIndex, TSource oldItem, TSource newItem)
	    {
		    EnsureGroupsExists(newItem);
	    }

	    protected override void ReactToItemPropertyChanged(TSource item, string propertyName)
	    {
		    if (SourceCollection.Contains(item).Current)
		    {
			    EnsureGroupsExists(item);
		    }
	    }

	    private void Group_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove) return;
            if (sender is IBindableGrouping<TKey, TElement> bindableGrouping && bindableGrouping.Count == 0)
            {
                ResultCollection.Remove(bindableGrouping);
            }
        }
    }
}