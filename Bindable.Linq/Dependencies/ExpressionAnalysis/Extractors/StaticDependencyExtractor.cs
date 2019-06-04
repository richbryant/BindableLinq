using System.Linq.Expressions;
using Bindable.Linq.Dependencies.Defintions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    internal sealed class StaticDependencyExtractor : DependencyExtractor
    {
        protected override IDependencyDefinition ExtractFromRoot(Expression rootExpression, string propertyPath)
        {
            IDependencyDefinition result = null;
            if (!(rootExpression is MemberExpression memberExpression)) return null;
            if (memberExpression.Expression == null)
            {
                result = new StaticDependencyDefinition(propertyPath, memberExpression.Member);
            }
            return result;
        }
    }

}