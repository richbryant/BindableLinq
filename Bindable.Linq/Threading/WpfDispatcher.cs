using System;
using System.Windows.Threading;

namespace Bindable.Linq.Threading
{
    public class WpfDispatcher : IDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public WpfDispatcher()
            : this(Dispatcher.CurrentDispatcher)
        {
        }

        public WpfDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Dispatch(Action actionToInvoke)
        {
            if (!DispatchRequired())
            {
                actionToInvoke();
            }
            else
            {
                _dispatcher.Invoke(DispatcherPriority.Normal, actionToInvoke);
            }
        }

        public TResult Dispatch<TResult>(Func<TResult> actionToInvoke)
        {
            if (_dispatcher.CheckAccess())
            {
                return actionToInvoke();
            }
            return (TResult)_dispatcher.Invoke(DispatcherPriority.Normal, actionToInvoke);
        }

        public bool DispatchRequired()
        {
            return !_dispatcher.CheckAccess();
        }
    }
}