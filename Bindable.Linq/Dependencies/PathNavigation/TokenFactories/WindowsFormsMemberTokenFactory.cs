using System;
using System.Windows.Forms;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation.TokenFactories
{
    public sealed class WindowsFormsMemberTokenFactory : ITokenFactory
    {
        public IToken ParseNext(object target, string propertyPath, Action<object, string> callback, IPathNavigator pathNavigator)
        {
            if (!(target is Control)) return null;
            var propertyName = propertyPath;
            var num = propertyPath.IndexOf('.');
            if (num < 0)
                return new WindowsFormsMemberToken(target, propertyName, null, callback, pathNavigator);

            propertyName = propertyPath.Substring(0, num);
            var remainingPath = propertyPath.Substring(num + 1);
            return new WindowsFormsMemberToken(target, propertyName, remainingPath, callback, pathNavigator);
        }
    }
}