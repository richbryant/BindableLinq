using Bindable.Linq.Dependencies.ExpressionAnalysis;
using Bindable.Linq.Dependencies.PathNavigation;

namespace Bindable.Linq.Configuration
{
    public interface IBindingConfiguration
    {
        IExpressionAnalyzer CreateExpressionAnalyzer();

        IPathNavigator CreatePathNavigator();
    }
}