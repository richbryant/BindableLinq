using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis
{
    public sealed class ExpressionAnalyzer : IExpressionAnalyzer
    {
        private readonly IDependencyExtractor[] _extractors;

        public ExpressionAnalyzer(params IDependencyExtractor[] extractors)
        {
            _extractors = extractors;
        }

        public IEnumerable<IDependencyDefinition> DiscoverDependencies(Expression expression, ParameterExpression itemParameter)
        {
            return (from extractor in _extractors
                select extractor.Extract(expression)).UnionAll().Distinct(DependencyComparer.Instance);
        }
    }
}