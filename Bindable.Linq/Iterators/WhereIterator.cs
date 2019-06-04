using System;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Iterators
{
    internal sealed class WhereIterator<TElement> : Iterator<TElement, TElement> where TElement : class
    {
        private readonly Func<TElement, bool> _predicate;

        public WhereIterator(IBindableCollection<TElement> sourceCollection, Func<TElement, bool> predicate, IDispatcher dispatcher)
            : base(sourceCollection, dispatcher)
        {
            _predicate = predicate;
        }

        protected override void EvaluateSourceCollection()
        {
            foreach (var item in SourceCollection)
            {
                ReactToAdd(-1, item);
            }
        }

        public bool Filter(TElement element)
        {
            return _predicate(element);
        }

        protected override void ReactToAdd(int insertionIndex, TElement addedItem)
        {
            if (Filter(addedItem))
            {
                ResultCollection.Insert(insertionIndex, addedItem);
            }
        }

        protected override void ReactToMove(int oldIndex, int newIndex, TElement movedItem)
        {
            if (Filter(movedItem))
            {
                ResultCollection.Move(newIndex, movedItem);
            }
        }

        protected override void ReactToRemove(int index, TElement removedItem)
        {
            ResultCollection.Remove(removedItem);
        }

        protected override void ReactToReplace(int oldIndex, TElement oldItem, TElement newItem)
        {
            if (Filter(newItem))
            {
                ResultCollection.Replace(oldItem, newItem);
            }
            else
            {
                ResultCollection.Remove(oldItem);
            }
        }

        protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
        {
            if (!Filter(item))
            {
                if (ResultCollection.Contains(item))
                {
                    ResultCollection.Remove(item);
                }
            }
            else if (!ResultCollection.Contains(item))
            {
                ResultCollection.Add(item);
            }
        }
    }
}