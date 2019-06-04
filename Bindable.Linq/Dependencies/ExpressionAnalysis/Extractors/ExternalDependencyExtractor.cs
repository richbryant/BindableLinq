using System.ComponentModel;
using System.Linq.Expressions;
using Bindable.Linq.Dependencies.Defintions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    internal sealed class ExternalDependencyExtractor : DependencyExtractor
    {
        protected override IDependencyDefinition ExtractFromRoot(Expression rootExpression, string propertyPath)
        {
            IDependencyDefinition result = null;
            if (!(rootExpression is ConstantExpression constantExpression)) return null;
            if (propertyPath != null || (constantExpression.Value is INotifyPropertyChanged))
            {
                result = new ExternalDependencyDefinition(propertyPath, constantExpression.Value);
            }
            return result;
        }
    }
}