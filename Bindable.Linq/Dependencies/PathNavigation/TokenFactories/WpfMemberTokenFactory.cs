using System;
using System.Reflection;
using System.Windows;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation.TokenFactories
{
    public sealed class WpfMemberTokenFactory : ITokenFactory
    {
        public IToken ParseNext(object target, string propertyPath, Action<object, string> callback, IPathNavigator pathNavigator)
        {
            IToken result = null;
            if (!(target is DependencyObject dependencyObject)) return null;
            var text = propertyPath;
            string remainingPath = null;
            var num = propertyPath.IndexOf('.');
            if (num >= 0)
            {
                text = propertyPath.Substring(0, num);
                remainingPath = propertyPath.Substring(num + 1);
            }

            var field = dependencyObject.GetType().GetField(text + "Property", BindingFlags.Static | BindingFlags.Public);
            if (field == null) return null;
            var dependencyProperty = (DependencyProperty)field.GetValue(null);
            if (dependencyProperty != null)
            {
                result = new WpfMemberToken(dependencyObject, dependencyProperty, text, remainingPath, callback, pathNavigator);
            }
            return result;
        }
    }
}