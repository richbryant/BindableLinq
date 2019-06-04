using System.Windows.Threading;

namespace Bindable.Linq.Threading
{
    internal sealed class DispatcherFactory
    {
        public static IDispatcher Create(Dispatcher dispatcher)
        {
            IDispatcher result = null;
            if (dispatcher != null)
            {
                result = new WpfDispatcher(dispatcher);
            }
            return result;
        }

        public static IDispatcher Create()
        {
            return Create(Dispatcher.CurrentDispatcher);
        }
    }
}