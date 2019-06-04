using System;
using Bindable.Linq.Dependencies.PathNavigation.TokenFactories;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation
{
    public class PathNavigator : IPathNavigator
    {
        private readonly ITokenFactory[] _traversers;

        public PathNavigator(params ITokenFactory[] traversers)
        {
            _traversers = traversers;
        }

        public IToken TraverseNext(object target, string propertyPath, Action<object, string> callback)
        {
            propertyPath = (propertyPath ?? string.Empty);
            IToken token = null;
            var traversers = _traversers;
            foreach (var tokenFactory in traversers)
            {
                token = tokenFactory.ParseNext(target, propertyPath, callback, this);
                if (token != null)
                {
                    break;
                }
            }
            return token;
        }
    }
}