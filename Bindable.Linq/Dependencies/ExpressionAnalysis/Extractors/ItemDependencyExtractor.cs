using System.Linq.Expressions;
using Bindable.Linq.Dependencies.Defintions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    internal sealed class ItemDependencyExtractor : DependencyExtractor
    {
        protected override IDependencyDefinition ExtractFromRoot(Expression rootExpression, string propertyPath)
        {
            IDependencyDefinition result = null;
            if (rootExpression is ParameterExpression parameterExpression)
            {
                result = new ItemDependencyDefinition(propertyPath, parameterExpression.Name);
            }
            return result;
        }
    }
}