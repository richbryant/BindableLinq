using System;
using System.Linq.Expressions;

namespace Bindable.Linq.Operators
{
    internal sealed class SwitchCase<TInput, TResult> : ISwitchCase<TInput, TResult>
    {
        private readonly Func<TInput, bool> _inputConditionCompiled;

        private readonly Func<TInput, TResult> _resultExpressionCompiled;

        public SwitchCase(Expression<Func<TInput, bool>> inputCondition, Expression<Func<TInput, TResult>> resultExpression)
        {
            _inputConditionCompiled = inputCondition.Compile();
            _resultExpressionCompiled = resultExpression.Compile();
        }

        public bool Evaluate(TInput input)
        {
            return _inputConditionCompiled(input);
        }

        public TResult Return(TInput input)
        {
            return _resultExpressionCompiled(input);
        }
    }
}