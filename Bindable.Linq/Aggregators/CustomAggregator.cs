using System;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Aggregators
{
    internal sealed class CustomAggregator<TSource, TAccumulate> : Aggregator<TSource, TAccumulate>
    {
        private readonly Func<IBindableCollection<TSource>, TAccumulate> _aggregator;

        public CustomAggregator(IBindableCollection<TSource> source, Func<IBindableCollection<TSource>, TAccumulate> aggregator, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            _aggregator = aggregator;
        }

        protected override void RefreshOverride()
        {
            base.Current = _aggregator(base.SourceCollection);
        }
    }
}