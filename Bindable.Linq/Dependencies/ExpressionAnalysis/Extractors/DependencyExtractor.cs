using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors
{
    internal abstract class DependencyExtractor : IDependencyExtractor
    {
        public IEnumerable<IDependencyDefinition> Extract(Expression expression)
        {
            var list = new List<IDependencyDefinition>();
            var expressionFlattener = new ExpressionFlattener(expression, ExpressionType.MemberAccess);
            foreach (var expression3 in expressionFlattener.Expressions)
            {
                var flag = false;
                var expression2 = expression3;
                string text = null;
                if (expression3 is MemberExpression memberExpression)
                {
                    text = memberExpression.Member.Name;
                    expression2 = memberExpression.Expression;
                    flag = true;
                    while (true)
                    {
                        if (!(expression2 is MemberExpression))
                        {
                            break;
                        }
                        var memberExpression2 = (MemberExpression)expression2;
                        text = memberExpression2.Member.Name + "." + text;
                        if (memberExpression2.Expression == null)
                        {
                            break;
                        }
                        expression2 = memberExpression2.Expression;
                    }
                }

                if (expression2 == null) continue;
                var dependencyDefinition = ExtractFromRoot(expression2, text);
                if (dependencyDefinition != null)
                {
                    list.Add(dependencyDefinition);
                }
                else if (flag)
                {
                    list.AddRange(Extract(expression2));
                }
            }
            return list;
        }

        protected abstract IDependencyDefinition ExtractFromRoot(Expression rootExpression, string propertyPath);
    }
}