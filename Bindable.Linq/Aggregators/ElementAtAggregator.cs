using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Aggregators
{
    internal sealed class ElementAtAggregator<TElement> : Aggregator<TElement, TElement>
    {
        private readonly TElement _default;

        private readonly int _index;

        public ElementAtAggregator(IBindableCollection<TElement> source, int index, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            _index = index;
            _default = default(TElement);
        }

        protected override void RefreshOverride()
        {
            var num = 0;
            var flag = false;
            var current = _default;
            foreach (var item in SourceCollection)
            {
                current = item;
                if (num != _index) continue;
                flag = true;
                break;
            }
            if (!flag && num == -1)
            {
                current = _default;
            }
            Current = current;
        }
    }
}