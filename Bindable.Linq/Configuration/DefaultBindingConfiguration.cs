﻿using Bindable.Linq.Dependencies.ExpressionAnalysis;
using Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.PathNavigation.TokenFactories;

namespace Bindable.Linq.Configuration
{
    internal sealed class DefaultBindingConfiguration : IBindingConfiguration
    {
        private readonly IExpressionAnalyzer _expressionAnalyzer;

        private readonly IPathNavigator _pathNavigator;

        public DefaultBindingConfiguration()
        {
            _expressionAnalyzer = new ExpressionAnalyzer(new ItemDependencyExtractor(), new ExternalDependencyExtractor(), new StaticDependencyExtractor());
            _pathNavigator = new PathNavigator(new WpfMemberTokenFactory(), new SilverlightMemberTokenFactory(), new WindowsFormsMemberTokenFactory(), new ClrMemberTokenFactory());
        }

        public IExpressionAnalyzer CreateExpressionAnalyzer()
        {
            return _expressionAnalyzer;
        }

        public IPathNavigator CreatePathNavigator()
        {
            return _pathNavigator;
        }
    }
}