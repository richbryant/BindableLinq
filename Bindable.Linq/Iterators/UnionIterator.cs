using System.Collections.Generic;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Iterators
{
    internal sealed class UnionIterator<TElement> : Iterator<IBindableCollection<TElement>, TElement> where TElement : class
    {
        private readonly ElementActioner<IBindableCollection<TElement>> _rootActioner;

        private readonly Dictionary<IBindableCollection<TElement>, ElementActioner<TElement>> _childCollectionActioners;

        public UnionIterator(IBindableCollection<IBindableCollection<TElement>> elements, IDispatcher dispatcher)
            : base(elements, dispatcher)
        {
            _childCollectionActioners = new Dictionary<IBindableCollection<TElement>, ElementActioner<TElement>>();
            _rootActioner = new ElementActioner<IBindableCollection<TElement>>(SourceCollection, ChildCollectionAdded, ChildCollectionRemoved, Dispatcher);
        }

        private void ChildCollectionRemoved(IBindableCollection<TElement> collection)
        {
        }

        private void ChildCollectionAdded(IBindableCollection<TElement> collection)
        {
            _childCollectionActioners[collection] = new ElementActioner<TElement>(collection, delegate(TElement item)
            {
                ResultCollection.Add(item);
            }, delegate(TElement item)
            {
                ResultCollection.Remove(item);
            }, Dispatcher);
        }

        protected override void EvaluateSourceCollection()
        {
            SourceCollection.Evaluate();
        }

        protected override void ReactToItemPropertyChanged(IBindableCollection<TElement> item, string propertyName)
        {
        }

        protected override void ReactToAdd(int insertionIndex, IBindableCollection<TElement> addedItem)
        {
        }

        protected override void ReactToReplace(int index, IBindableCollection<TElement> oldItem, IBindableCollection<TElement> newItem)
        {
        }

        protected override void ReactToMove(int originalIndex, int newIndex, IBindableCollection<TElement> movedItem)
        {
        }

        protected override void ReactToRemove(int index, IBindableCollection<TElement> removedItem)
        {
        }

        protected override void BeforeResetOverride()
        {
            _childCollectionActioners.Clear();
        }
    }
}