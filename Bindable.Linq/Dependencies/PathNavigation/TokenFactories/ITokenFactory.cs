using System;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation.TokenFactories
{
    public interface ITokenFactory
    {
        IToken ParseNext(object target, string propertyPath, Action<object, string> callback, IPathNavigator pathNavigator);
    }
}