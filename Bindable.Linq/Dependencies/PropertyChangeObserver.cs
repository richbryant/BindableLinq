using System;
using System.ComponentModel;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies
{
    internal sealed class PropertyChangeObserver : EventDependency<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        private readonly EventHandler<PropertyChangedEventArgs> _callback;

        private readonly WeakEventProxy<PropertyChangedEventArgs> _weakHandler;

        public PropertyChangeObserver(EventHandler<PropertyChangedEventArgs> callback)
        {
            _callback = callback;
            _weakHandler = new WeakEventProxy<PropertyChangedEventArgs>(callback);
        }

        protected override void AttachOverride(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged += _weakHandler.Handler;
        }

        protected override void DetachOverride(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged -= _weakHandler.Handler;
        }

        protected override void DisposeOverride()
        {
        }
    }
}