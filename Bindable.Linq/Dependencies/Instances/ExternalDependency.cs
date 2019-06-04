using System;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.Instances
{
    internal sealed class ExternalDependency : IDependency, IDisposable
    {
        private readonly object _dependencyLock = new object();

        private readonly IToken _rootMonitor;

        private Action<object> _elementChangedCallback;

        public ExternalDependency(object targetObject, string propertyPath, IPathNavigator pathNavigator)
        {
            _rootMonitor = pathNavigator.TraverseNext(targetObject, propertyPath, Element_PropertyChanged);
        }

        public void SetReevaluateElementCallback(Action<object, string> action)
        {
        }

        public void SetReevaluateCallback(Action<object> action)
        {
            _elementChangedCallback = action;
        }

        public void Dispose()
        {
            _rootMonitor.Dispose();
        }

        private void Element_PropertyChanged(object element, string propertyPath)
        {
            _elementChangedCallback?.Invoke(element);
        }
    }

}