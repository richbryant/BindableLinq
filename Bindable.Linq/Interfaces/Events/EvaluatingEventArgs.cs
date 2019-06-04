using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces.Events
{
    public class EvaluatingEventArgs<TElement> : EventArgs
    {
        private readonly List<TElement> _itemsYeildedFromFirstEvaluation;

        public List<TElement> ItemsYieldedFromEvaluation => _itemsYeildedFromFirstEvaluation;

        public EvaluatingEventArgs(List<TElement> itemsYielded)
        {
            _itemsYeildedFromFirstEvaluation = itemsYielded;
        }
    }
}