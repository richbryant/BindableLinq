using System;
using System.Linq.Expressions;

namespace Bindable.Linq.Operators
{
    internal sealed class SwitchDefault<TInput, TResult> : ISwitchCase<TInput, TResult>
    {
        private readonly Expression<Func<TInput, TResult>> _resultExpression;

        private readonly Func<TInput, TResult> _resultExpressionCompiled;

        public SwitchDefault(Expression<Func<TInput, TResult>> resultExpression)
        {
            _resultExpression = resultExpression;
            _resultExpressionCompiled = _resultExpression.Compile();
        }

        public bool Evaluate(TInput input)
        {
            return true;
        }

        public TResult Return(TInput input)
        {
            return _resultExpressionCompiled(input);
        }
    }
}