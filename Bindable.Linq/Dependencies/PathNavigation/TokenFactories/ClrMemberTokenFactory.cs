using System;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation.TokenFactories
{
    public sealed class ClrMemberTokenFactory : ITokenFactory
    {
        public IToken ParseNext(object target, string propertyPath, Action<object, string> callback, IPathNavigator pathNavigator)
        {
            IToken result = null;
            if (target != null)
            {
                string propertyName = propertyPath;
                string remainingPath = null;
                int num = propertyPath.IndexOf('.');
                if (num >= 0)
                {
                    propertyName = propertyPath.Substring(0, num);
                    remainingPath = propertyPath.Substring(num + 1);
                }
                result = new ClrMemberToken(target, propertyName, remainingPath, callback, pathNavigator);
            }
            return result;
        }
    }
}