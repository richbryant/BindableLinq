using System;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Helpers
{
    public abstract class DispatcherBound : IDisposable
    {
        private bool _isSealed;

        protected object InstanceLock { get; } = new object();

        protected LifetimeCouplings InstanceLifetime { get; } = new LifetimeCouplings();

        public IDispatcher Dispatcher { get; }

        protected DispatcherBound(IDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        protected void AssertDispatcherThread()
        {
            if (!Dispatcher.DispatchRequired()) return;
            var message = "Must be called on UI thread.";
            throw new Exception(message);
        }

        protected void AssertUnsealed()
        {
            if (!_isSealed) return;
            var message = "Must not be sealed.";
            throw new Exception(message);
        }

        protected void Seal()
        {
            BeforeSealingOverride();
            _isSealed = true;
        }

        protected virtual void BeforeSealingOverride()
        {
        }

        protected virtual void BeforeDisposeOverride()
        {
        }

        public void Dispose()
        {
            BeforeDisposeOverride();
            InstanceLifetime.Dispose();
        }
    }
}