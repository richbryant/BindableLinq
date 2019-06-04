using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Bindable.Linq.Helpers;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Adapters.Incoming
{
    internal sealed class BindingListToBindableCollectionAdapter<TElement> : BindableCollectionAdapterBase<TElement>
{
	public BindingListToBindableCollectionAdapter(IEnumerable sourceCollection, IDispatcher dispatcher)
		: base(sourceCollection, dispatcher)
	{
		var bindingList = (IBindingList)sourceCollection;
		var bindingList2 = bindingList;
		EventHandler<ListChangedEventArgs> eventHandler = delegate(object sender, ListChangedEventArgs e)
		{
			var bindingListToBindableCollectionAdapter = this;
			Dispatcher.Dispatch(delegate
			{
				bindingListToBindableCollectionAdapter.SourceCollection_ListChanged(sender, e);
			});
		};
		bindingList2.ListChanged += Weak.Event(eventHandler).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
	}

	private void SourceCollection_ListChanged(object sender, ListChangedEventArgs e)
	{
        if (!(sender is IBindingList bindingList))
		{
			return;
		}
		NotifyCollectionChangedEventArgs e2 = null;
		switch (e.ListChangedType)
		{
		case ListChangedType.ItemAdded:
		{
			var array = new object[bindingList.Count];
			bindingList.CopyTo(array, 0);
			if (e.NewIndex >= 0 && e.NewIndex < array.Length)
			{
				var changedItem = bindingList[e.NewIndex];
				e2 = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItem, e.NewIndex);
			}
			break;
		}
		case ListChangedType.ItemDeleted:
		{
			var array = new object[bindingList.Count];
			bindingList.CopyTo(array, 0);
			if (e.OldIndex >= 0 && e.OldIndex < array.Length)
			{
				var changedItem = bindingList[e.OldIndex];
				e2 = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, e.NewIndex);
			}
			break;
		}
		case ListChangedType.ItemMoved:
		{
			var array = new object[bindingList.Count];
			bindingList.CopyTo(array, 0);
			if (e.OldIndex >= 0 && e.OldIndex < array.Length)
			{
				var changedItem = bindingList[e.NewIndex];
				e2 = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, changedItem, e.NewIndex, e.OldIndex);
			}
			break;
		}
		case ListChangedType.Reset:
		case ListChangedType.ItemChanged:
			e2 = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			break;
		}
		OnCollectionChanged(e2);
	}
}
}