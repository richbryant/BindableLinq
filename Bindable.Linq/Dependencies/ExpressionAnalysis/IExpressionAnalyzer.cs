using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis
{
    public interface IExpressionAnalyzer
    {
        IEnumerable<IDependencyDefinition> DiscoverDependencies(Expression expression, ParameterExpression itemParameter);
    }
}