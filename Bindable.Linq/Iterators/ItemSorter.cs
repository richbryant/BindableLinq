using System;
using System.Collections.Generic;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Iterators
{
    internal abstract class ItemSorter<T>
    {
        public abstract int Compare(T left, T right);
    }

    internal sealed class ItemSorter<T, U> : ItemSorter<T>
    {
        private readonly bool _ascending;

        private readonly IComparer<U> _comparer;

        public readonly Func<T, U> _keySelector;

        private readonly ItemSorter<T> _superior;

        public ItemSorter(ItemSorter<T> superior, Func<T, U> keySelector, IComparer<U> comparer, bool ascending)
        {
            _keySelector = keySelector;
            _superior = superior;
            _comparer = (comparer ?? new DefaultComparer<U>());
            _ascending = ascending;
        }

        public override int Compare(T left, T right)
        {
            var num = 0;
            if (_superior != null)
            {
                num = _superior.Compare(left, right);
            }

            if (num != 0) return num;
            var x = _keySelector(left);
            var y = _keySelector(right);
            num = _comparer.Compare(x, y);
            if (!_ascending)
            {
                num = -num;
            }
            return num;
        }
    }

}