using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Dependencies.Instances
{
    public sealed class ItemDependency<TElement> : IDependency
    {
        private readonly ElementActioner<TElement> _actioner;

        private readonly IPathNavigator _pathNavigator;

        private readonly string _propertyPath;

        private readonly Dictionary<TElement, IToken> _sourceElementObservers;

        private Action<object, string> _reevaluateElementCallback;

        private IBindableCollection<TElement> _sourceElements;

        private object DependencyLock { get; } = new object();

        public ItemDependency(string propertyPath, IBindableCollection<TElement> sourceElements, IPathNavigator pathNavigator)
        {
            _pathNavigator = pathNavigator;
            _sourceElementObservers = new Dictionary<TElement, IToken>();
            _propertyPath = propertyPath;
            _sourceElements = sourceElements;
            _actioner = new ElementActioner<TElement>(sourceElements, AddItem, RemoveItem, sourceElements.Dispatcher);
        }

        public void SetReevaluateElementCallback(Action<object, string> action)
        {
            _reevaluateElementCallback = action;
        }

        public void SetReevaluateCallback(Action<object> action)
        {
        }

        public void Dispose()
        {
            _actioner.Dispose();
        }

        private void AddItem(TElement addedItem)
        {
            lock (DependencyLock)
            {
                if (addedItem is INotifyPropertyChanged && !_sourceElementObservers.ContainsKey(addedItem))
                {
                    _sourceElementObservers[addedItem] = _pathNavigator.TraverseNext(addedItem, _propertyPath, Element_PropertyChanged);
                }
            }
        }

        private void RemoveItem(TElement removedItem)
        {
            lock (DependencyLock)
            {
                if (!_sourceElementObservers.ContainsKey(removedItem)) return;
                _sourceElementObservers[removedItem]?.Dispose();
                _sourceElementObservers.Remove(removedItem);
            }
        }

        private void Element_PropertyChanged(object element, string propertyPath)
        {
            _reevaluateElementCallback?.Invoke(element, propertyPath);
        }
    }
}