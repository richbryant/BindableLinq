using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Bindable.Linq.Dependencies;

namespace Bindable.Linq.Interfaces.Internal
{
    internal interface IEditableBindableGrouping<TKey, TElement> : IBindableGrouping<TKey, TElement>
    {
        void AddRange(IEnumerable<TElement> elements);

        void RemoveRange(IEnumerable<TElement> elements);

        void ReplaceRange(IEnumerable<TElement> oldItems, IEnumerable<TElement> newItems);
    }
}