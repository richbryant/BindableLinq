using System;

namespace Bindable.Linq.Helpers
{
    internal sealed class WeakEventProxy<TEventArgs> where TEventArgs : EventArgs
    {
        private readonly WeakReference _callbackReference;

        private readonly object _lock = new object();

        public WeakEventProxy(EventHandler<TEventArgs> callback)
        {
            _callbackReference = new WeakReference(callback, trackResurrection: true);
        }

        public void Handler(object sender, TEventArgs e)
        {
            (_callbackReference.Target as EventHandler<TEventArgs>)?.Invoke(sender, e);
        }
    }
}