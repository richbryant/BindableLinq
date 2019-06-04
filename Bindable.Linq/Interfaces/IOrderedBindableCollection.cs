using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces
{
    public interface IOrderedBindableCollection<TResult> : IBindableCollection<TResult> where TResult : class
    {
        IOrderedBindableCollection<TResult> CreateOrderedIterator<TKey>(Func<TResult, TKey> keySelector, IComparer<TKey> comparer, bool descending);
    }
}