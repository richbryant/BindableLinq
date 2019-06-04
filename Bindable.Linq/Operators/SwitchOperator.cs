using System;
using System.Collections.Generic;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Operators
{
    internal sealed class SwitchOperator<TSource, TResult> : Operator<TSource, TResult>, ISwitch<TSource, TResult>
    {
        private readonly List<ISwitchCase<TSource, TResult>> _conditionalCases = new List<ISwitchCase<TSource, TResult>>();

        private ISwitchCase<TSource, TResult> _defaultCase;

        public SwitchOperator(IBindable<TSource> source, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
        }

        public ISwitch<TSource, TResult> AddCase(ISwitchCase<TSource, TResult> customCase)
        {
            _conditionalCases.Add(customCase);
            return this;
        }

        public ISwitch<TSource, TResult> AddDefault(ISwitchCase<TSource, TResult> defaultCase)
        {
            if (_defaultCase != null)
            {
                _defaultCase = defaultCase;
                return this;
            }
            throw new InvalidOperationException("A default state has already been defined for this switch statement.");
        }

        protected override void RefreshOverride()
        {
            var current = default(TResult);
            var current2 = Source.Current;
            var flag = false;
            foreach (var conditionalCase in _conditionalCases)
            {
                if (!conditionalCase.Evaluate(current2)) continue;
                current = conditionalCase.Return(current2);
                flag = true;
                break;
            }
            if (!flag && _defaultCase != null)
            {
                current = _defaultCase.Return(current2);
            }
            Current = current;
        }
    }
}