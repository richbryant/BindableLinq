using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Adapters.Outgoing
{
    internal sealed class BindingListAdapter<TElement> : DispatcherBound, IBindingList where TElement : class
    {
	    private readonly EventHandler<NotifyCollectionChangedEventArgs> _eventHandler;

	    private readonly IBindableCollection<TElement> _originalSource;

	    private readonly Dictionary<string, PropertyDescriptor> _propertyDescriptors;

	    private readonly WeakEventProxy<NotifyCollectionChangedEventArgs> _weakHandler;

	    private ElementActioner<TElement> _addActioner;

	    private PropertyChangeObserver _propertyChangeObserver;

	    private ListSortDirection _sortDirection;

	    private PropertyDescriptor _sortProperty;

	    private IBindableCollection<TElement> _source;

	    public bool AllowEdit => false;

	    public bool AllowNew => false;

	    public bool AllowRemove => false;

	    public bool IsSorted => _source != _originalSource;

	    public ListSortDirection SortDirection => _sortDirection;

	    public PropertyDescriptor SortProperty => _sortProperty;

	    public bool SupportsChangeNotification => true;

	    public bool SupportsSearching => true;

	    public bool SupportsSorting => true;

	    public bool IsFixedSize => true;

	    public bool IsReadOnly => false;

	    public object this[int index]
	    {
		    get => _source.ElementAt(index);
            set => throw new NotSupportedException();
        }

	    public int Count => _source.Count;

	    public bool IsSynchronized => true;

	    public object SyncRoot => new object();

	    public event ListChangedEventHandler ListChanged;

	    public BindingListAdapter(IBindableCollection<TElement> source, IDispatcher dispatcher)
		    : base(dispatcher)
	    {
		    source.ShouldNotBeNull("source");
		    _originalSource = source;
		    _eventHandler = Source_CollectionChanged;
		    _weakHandler = new WeakEventProxy<NotifyCollectionChangedEventArgs>(_eventHandler);
		    _sortDirection = ListSortDirection.Ascending;
		    WireInterceptor(_originalSource);
		    _propertyDescriptors = new Dictionary<string, PropertyDescriptor>();
		    var properties = TypeDescriptor.GetProperties(typeof(TElement));
		    foreach (PropertyDescriptor item in properties)
		    {
			    if (item?.Name != null)
			    {
				    _propertyDescriptors[item.Name] = item;
			    }
		    }
	    }

	    public void AddIndex(PropertyDescriptor property)
	    {
	    }

	    public object AddNew()
	    {
		    throw new NotSupportedException();
	    }

	    public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
	    {
            if (property.Equals(_sortProperty) && direction == _sortDirection) return;
            if (IsSorted)
            {
                UnwireInterceptor();
            }
            _sortProperty = property;
            _sortDirection = direction;
            Expression<Func<TElement, object>> keySelector = item => KeySelector(item);
            var query = _sortDirection == ListSortDirection.Ascending ? _originalSource.OrderBy(keySelector) : _originalSource.OrderByDescending(keySelector);
            WireInterceptor(query.WithDependency(_sortProperty.Name));
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

	    public int Find(PropertyDescriptor property, object key)
	    {
		    var source = _source;
		    if (source == null)
		    {
			    throw new NotSupportedException();
		    }
		    for (var i = 0; i < source.Count; i++)
		    {
			    var val = source[0];
			    if (null != val && property.GetValue(val) == key)
			    {
				    return i;
			    }
		    }
		    return -1;
	    }

	    public void RemoveIndex(PropertyDescriptor property)
	    {
	    }

	    public void RemoveSort()
	    {
            if (!IsSorted) return;
            UnwireInterceptor();
            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;
            WireInterceptor(_originalSource);
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

	    public int Add(object value)
	    {
		    throw new NotSupportedException();
	    }

	    public void Clear()
	    {
		    throw new NotSupportedException();
	    }

	    public bool Contains(object value)
	    {
		    return IndexOf(value) >= 0;
	    }

	    public int IndexOf(object value)
	    {
		    var num = 0;
		    foreach (var item in _source)
		    {
			    if (value == item)
			    {
				    return num;
			    }
			    num++;
		    }
		    return -1;
	    }

	    public void Insert(int index, object value)
	    {
		    throw new NotSupportedException();
	    }

	    public void Remove(object value)
	    {
		    throw new NotSupportedException();
	    }

	    public void RemoveAt(int index)
	    {
		    throw new NotSupportedException();
	    }

	    public void CopyTo(Array array, int index)
	    {
		    foreach (var item in _source)
		    {
			    if (index >= array.Length)
			    {
				    break;
			    }
			    array.SetValue(item, index);
			    index++;
		    }
	    }

	    public IEnumerator GetEnumerator()
	    {
		    return _source.GetEnumerator();
	    }

	    private void WireInterceptor(IBindableCollection<TElement> source)
	    {
		    _source = source;
		    _source.CollectionChanged += _weakHandler.Handler;
		    _propertyChangeObserver = new PropertyChangeObserver(Element_PropertyChanged);
		    _addActioner = new ElementActioner<TElement>(_source, delegate(TElement element)
		    {
			    _propertyChangeObserver.Attach(element);
		    }, delegate(TElement element)
		    {
			    _propertyChangeObserver.Detach(element);
		    }, Dispatcher);
	    }

	    private void UnwireInterceptor()
	    {
		    _addActioner.Dispose();
		    _propertyChangeObserver.Dispose();
		    _source.CollectionChanged -= _weakHandler.Handler;
		    if (_source != _originalSource)
		    {
			    _source.Dispose();
		    }
		    _source = null;
	    }

	    private object KeySelector(TElement item)
	    {
		    if (item == null || null == _sortProperty)
		    {
			    return null;
		    }
		    return _sortProperty.GetValue(item);
	    }

	    private void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
	    {
            if (!_propertyDescriptors.ContainsKey(e.PropertyName)) return;
            var propDesc = _propertyDescriptors[e.PropertyName];
            var newIndex = IndexOf(sender);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, newIndex, propDesc));
        }

	    private void Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	    {
		    switch (e.Action)
		    {
		    case NotifyCollectionChangedAction.Add:
		    {
			    var num = e.NewStartingIndex;
			    foreach (var unused in e.NewItems)
			    {
				    var e2 = new ListChangedEventArgs(ListChangedType.ItemAdded, num);
				    OnListChanged(e2);
				    num++;
			    }
			    break;
		    }
		    case NotifyCollectionChangedAction.Move:
		    {
			    var newStartingIndex = e.NewStartingIndex;
			    var oldStartingIndex = e.OldStartingIndex;
			    foreach (var unused in e.NewItems)
			    {
				    var e2 = new ListChangedEventArgs(ListChangedType.ItemMoved, newStartingIndex, oldStartingIndex);
				    OnListChanged(e2);
			    }
			    break;
		    }
		    case NotifyCollectionChangedAction.Remove:
		    {
			    var num = e.OldStartingIndex;
			    foreach (var unused in e.OldItems)
			    {
				    var e2 = new ListChangedEventArgs(ListChangedType.ItemDeleted, num);
				    OnListChanged(e2);
				    num++;
			    }
			    break;
		    }
		    case NotifyCollectionChangedAction.Replace:
		    {
			    var num = e.NewStartingIndex;
			    foreach (var unused in e.NewItems)
			    {
				    var e2 = new ListChangedEventArgs(ListChangedType.ItemChanged, num);
				    OnListChanged(e2);
				    num++;
			    }
			    break;
		    }
		    case NotifyCollectionChangedAction.Reset:
		    {
			    var e2 = new ListChangedEventArgs(ListChangedType.Reset, -1);
			    OnListChanged(e2);
			    break;
		    }
		    }
	    }

	    private void OnListChanged(ListChangedEventArgs e)
	    {
		    ListChanged?.Invoke(this, e);
	    }

	    protected override void BeforeDisposeOverride()
	    {
		    _addActioner.Dispose();
		    _propertyChangeObserver.Dispose();
		    base.BeforeDisposeOverride();
	    }
    }
}