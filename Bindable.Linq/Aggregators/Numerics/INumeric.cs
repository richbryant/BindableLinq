using System.Collections.Generic;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal interface INumeric<TItem, TAverageResult>
    {
        TItem Sum(IEnumerable<TItem> itemsToSum);

        TAverageResult Average(IEnumerable<TItem> items);

        TItem Min(IEnumerable<TItem> items);

        TItem Max(IEnumerable<TItem> items);
    }
}