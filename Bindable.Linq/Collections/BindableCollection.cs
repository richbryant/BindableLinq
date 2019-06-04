using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Events;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Collections
{
    public class BindableCollection<TElement> : DispatcherBound, IBindableCollection<TElement>
{
	private readonly IEqualityComparer<TElement> _comparer = ElementComparerFactory.Create<TElement>();

	private readonly List<TElement> _innerList = new List<TElement>(10000);

	private List<TElement> InnerList => _innerList;

	internal bool HasPropertyChangedSubscribers => PropertyChanged != null;

	public bool HasEvaluated => true;

	public TElement this[int index] => _innerList[index];

    public int Count => InnerList.Count;

	public event PropertyChangedEventHandler PropertyChanged;

	public event EvaluatingEventHandler<TElement> Evaluating;

	public event NotifyCollectionChangedEventHandler CollectionChanged;

	public BindableCollection(IDispatcher dispatcher)
		: base(dispatcher)
	{
		_innerList = new List<TElement>();
	}

	void IBindableCollection.Evaluate()
	{
	}

	public void Add(TElement item)
	{
		AssertDispatcherThread();
		var index = ((IList)InnerList).Add(item);
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
	}

	public void AddRange(IEnumerable<TElement> range)
	{
		AssertDispatcherThread();
        if (range == null) return;
        foreach (var item in range)
        {
            Add(item);
        }
    }

	public void AddOrInsertRange(int index, IEnumerable<TElement> items)
	{
		AssertDispatcherThread();
		if (index == -1)
		{
			AddRange(items);
		}
		else
		{
			InsertRange(index, items);
		}
	}

	public void Insert(int index, TElement item)
	{
		AssertDispatcherThread();
		if (index < 0 || index > InnerList.Count)
		{
			Add(item);
			return;
		}
		InnerList.Insert(index, item);
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
	}

	public void InsertRange(int index, IEnumerable<TElement> range)
	{
		AssertDispatcherThread();
        if (range == null) return;
        if (index < 0 || index > InnerList.Count)
        {
            AddRange(range);
            return;
        }
        var num = 0;
        foreach (var item in range)
        {
            var index2 = index + num;
            Insert(index2, item);
        }
    }

	public void InsertOrdered(TElement element, Comparison<TElement> comparer)
	{
		AssertDispatcherThread();
		var flag = false;
		for (var i = 0; i < InnerList.Count; i++)
		{
			var num = comparer(element, InnerList[i]);
            if (num > 0) continue;
            Insert(i, element);
            flag = true;
            break;
        }
		if (!flag)
		{
			Add(element);
		}
	}

	public void Move(int newIndex, TElement item)
	{
		AssertDispatcherThread();
		var num = IndexOf(item);
		var num2 = newIndex;
		var flag = false;
		if (num >= 0)
		{
			InnerList.Remove(item);
			flag = true;
		}
		if (num2 >= InnerList.Count)
		{
			num2 = ((IList)InnerList).Add(item);
		}
		else
		{
			InnerList.Insert(num2, item);
		}

        OnCollectionChanged(flag
            ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, num2, num)
            : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, num2));
    }

	public void MoveOrdered(TElement element, Comparison<TElement> comparer)
	{
		AssertDispatcherThread();
		var num = IndexOf(element);
		if (num < 0)
		{
			return;
		}
		var num2 = 0;
		while (true)
		{
            if (num2 >= InnerList.Count) return;
            var num3 = comparer(element, InnerList[num2]);
            if (num3 <= 0)
            {
                break;
            }
            num2++;
        }
		var flag = true;
		for (var i = num2; i < num && i < InnerList.Count; i++)
		{
            if (comparer(element, InnerList[i]) <= 0) continue;
            flag = false;
            break;
        }
		if (!flag && num2 != num)
		{
			Move(num2, element);
		}
	}

	public void Replace(TElement oldItem, TElement newItem)
	{
		AssertDispatcherThread();
		var val = oldItem != null ? oldItem : default(TElement);
		var val2 = newItem != null ? newItem : default(TElement);
		if (oldItem != null && newItem != null)
		{
			var num = IndexOf(val);
			if (num >= 0)
			{
				InnerList[num] = val2;
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, val2, val, num));
			}
			else
			{
				Add(val2);
			}
		}
		else if (newItem != null || oldItem != null)
		{
			if (newItem == null)
			{
				Remove(val);
			}
			else
            {
                Add(val2);
            }
        }
	}

	public bool Remove(TElement element)
	{
		AssertDispatcherThread();
        var num = IndexOf(element);
        if (num < 0) return false;
        var flag = InnerList.Remove(element);
        if (flag)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, element, num));
        }
        return flag;
	}

	public void RemoveAt(int index)
	{
		AssertDispatcherThread();
		var val = InnerList[index];
		if (val != null)
		{
			Remove(val);
		}
	}

	public void Clear()
	{
		InnerList.Clear();
		OnCollectionChanged(CommonEventArgsCache.Reset);
	}

	public IEnumerator<TElement> GetEnumerator()
	{
		AssertDispatcherThread();
		return InnerList.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public bool Contains(TElement item)
	{
		AssertDispatcherThread();
		return IndexOf(item) >= 0;
	}

	public int IndexOf(TElement item)
	{
		AssertDispatcherThread();
		var result = -1;
		for (var i = 0; i < InnerList.Count; i++)
		{
            if (!_comparer.Equals(item, InnerList[i])) continue;
            result = i;
            break;
        }
		return result;
	}

	void IRefreshable.Refresh()
	{
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "BindableCollection - Count: " + Count);
	}

	void IAcceptsDependencies.AcceptDependency(IDependencyDefinition definition)
	{
		throw new NotSupportedException("This object cannot accept dependencies directly.");
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		AssertDispatcherThread();
		PropertyChanged?.Invoke(this, e);
	}

	protected virtual void OnEvaluating(EvaluatingEventArgs<TElement> e)
	{
		AssertDispatcherThread();
		Evaluating?.Invoke(this, e);
		OnPropertyChanged(CommonEventArgsCache.Count);
	}

	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		AssertDispatcherThread();
		CollectionChanged?.Invoke(this, e);
		OnPropertyChanged(CommonEventArgsCache.Count);
	}
}
}