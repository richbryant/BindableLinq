using System;

namespace Bindable.Linq.Helpers
{
    internal sealed class WeakEvent<TEventArgs> where TEventArgs : EventArgs
    {
        private readonly EventHandler<TEventArgs> _originalHandler;

        public WeakEventProxy<TEventArgs> HandlerProxy { get; }

        public WeakEvent(EventHandler<TEventArgs> originalHandler)
        {
            _originalHandler = originalHandler;
            HandlerProxy = new WeakEventProxy<TEventArgs>(_originalHandler);
        }
    }
}