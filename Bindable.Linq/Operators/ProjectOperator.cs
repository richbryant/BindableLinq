using System;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Operators
{
    internal sealed class ProjectOperator<TSource, TResult> : Operator<TSource, TResult>
    {
        private readonly Func<TSource, TResult> _projector;

        public ProjectOperator(IBindable<TSource> source, Func<TSource, TResult> projector, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            _projector = projector;
        }

        protected override void RefreshOverride()
        {
            var current = Source.Current;
            Current = current != null ? _projector(current) : default(TResult);
        }
    }
}