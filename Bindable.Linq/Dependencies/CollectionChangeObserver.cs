using System;
using System.Collections.Specialized;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies
{
    internal sealed class CollectionChangeObserver : EventDependency<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        private readonly WeakEventProxy<NotifyCollectionChangedEventArgs> _weakEvent;

        private NotifyCollectionChangedEventHandler _callback;

        public CollectionChangeObserver(EventHandler<NotifyCollectionChangedEventArgs> callback)
        {
            _weakEvent = new WeakEventProxy<NotifyCollectionChangedEventArgs>(callback);
            _callback = callback.Invoke;
        }

        protected override void AttachOverride(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged += _weakEvent.Handler;
        }

        protected override void DetachOverride(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged -= _weakEvent.Handler;
        }

        protected override void DisposeOverride()
        {
        }
    }

}