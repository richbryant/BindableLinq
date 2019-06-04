using System;
using System.Collections.Generic;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Iterators
{
    internal sealed class OrderByIterator<TElement, TKey> : Iterator<TElement, TElement>, IOrderedBindableCollection<TElement> where TElement : class
    {
        private readonly ItemSorter<TElement, TKey> _itemSorter;

        public OrderByIterator(IBindableCollection<TElement> source, ItemSorter<TElement, TKey> itemSorter, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            _itemSorter = itemSorter;
        }

        protected override void EvaluateSourceCollection()
        {
            ResultCollection.AddRange(SourceCollection.OrderBy(x => _itemSorter._keySelector));
        }

        public int Compare(TElement lhs, TElement rhs)
        {
            return _itemSorter.Compare(lhs, rhs);
        }

        protected override void ReactToAdd(int insertionIndex, TElement addedItem)
        {
            ResultCollection.InsertOrdered(addedItem, Compare);
        }

        protected override void ReactToMove(int oldIndex, int newIndex, TElement movedItem)
        {
        }

        protected override void ReactToRemove(int oldIndex, TElement removedItem)
        {
            ResultCollection.Remove(removedItem);
        }

        protected override void ReactToReplace(int oldIndex, TElement oldItem, TElement newItem)
        {
            ReactToRemove(oldIndex, oldItem);
            ReactToAdd(-1, newItem);
        }

        protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
        {
            ResultCollection.MoveOrdered(item, Compare);
        }

        public IOrderedBindableCollection<TElement> CreateOrderedIterator<TNewKey>(Func<TElement, TNewKey> keySelector, IComparer<TNewKey> comparer, bool descending)
        {
            return new OrderByIterator<TElement, TNewKey>(SourceCollection, new ItemSorter<TElement, TNewKey>(_itemSorter, keySelector, comparer, !descending), Dispatcher);
        }
    }
}