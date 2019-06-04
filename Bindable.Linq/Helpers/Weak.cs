using System;

namespace Bindable.Linq.Helpers
{
    internal sealed class Weak
    {
        public static WeakEvent<TEventArgs> Event<TEventArgs>(EventHandler<TEventArgs> eventHandler) where TEventArgs : EventArgs
        {
            return new WeakEvent<TEventArgs>(eventHandler);
        }
    }
}