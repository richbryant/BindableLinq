using System.ComponentModel;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Operators
{
    internal sealed class EmptySwitchDefinition<TSource> : DispatcherBound, ISwitchDeclaration<TSource>
    {
        private readonly IBindable<TSource> _source;

        public EmptySwitchDefinition(IBindable<TSource> source, IDispatcher dispatcher)
            : base(dispatcher)
        {
            _source = source;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ISwitch<TSource, TResult> CreateForResultType<TResult>()
        {
            return new SwitchOperator<TSource, TResult>(_source, base.Dispatcher);
        }
    }
}