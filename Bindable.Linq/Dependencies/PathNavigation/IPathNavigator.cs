using System;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation
{
    public interface IPathNavigator
    {
        IToken TraverseNext(object target, string propertyPath, Action<object, string> callback);
    }

}