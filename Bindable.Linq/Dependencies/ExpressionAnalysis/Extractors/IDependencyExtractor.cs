using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    public interface IDependencyExtractor
    {
        IEnumerable<IDependencyDefinition> Extract(Expression expression);
    }
}