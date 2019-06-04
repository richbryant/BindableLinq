using System.Collections;
using System.Collections.Specialized;
using Bindable.Linq.Helpers;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Adapters.Incoming
{
    internal class ObservableCollectionToBindableCollectionAdapter<TElement> : BindableCollectionAdapterBase<TElement>
    {
        public ObservableCollectionToBindableCollectionAdapter(IEnumerable sourceCollection, IDispatcher dispatcher)
            : base(sourceCollection, dispatcher)
        {
            if (sourceCollection is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += Weak.Event(delegate(object sender, NotifyCollectionChangedEventArgs e)
                {
                    var observableCollectionToBindableCollectionAdapter = this;
                    Dispatcher.Dispatch(delegate
                    {
                        observableCollectionToBindableCollectionAdapter.OnCollectionChanged(e);
                    });
                }).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
            }
        }
    }
}