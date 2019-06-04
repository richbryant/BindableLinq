using System;
using System.Collections;
using System.Collections.Generic;

namespace Bindable.Linq.Dependencies
{
    internal abstract class EventDependency<TPublisher, TEventArgs> : IDisposable where TEventArgs : EventArgs
{
	private readonly Dictionary<int, WeakReference> _observables;

	private readonly object _observablesLock = new object();

	protected EventDependency()
	{
		_observables = new Dictionary<int, WeakReference>();
	}

	public void Dispose()
	{
		DisposeOverride();
		Clear();
	}

	protected abstract void AttachOverride(TPublisher publisher);

	protected abstract void DetachOverride(TPublisher publisher);

	public void AttachRange(IEnumerable range)
	{
		foreach (var item in range)
		{
			Attach(item);
		}
	}

	public void Attach(object objectToObserve)
    {
        if (!(objectToObserve is TPublisher val)) return;
        lock (_observablesLock)
        {
            if (Find(val) != null) return;
            _observables.Add(val.GetHashCode(), new WeakReference(val, trackResurrection: false));
            AttachOverride(val);
        }
    }

	public void DetachRange(IEnumerable range)
	{
		foreach (var item in range)
		{
			Detach(item);
		}
	}

	public void Detach(object objectThatWasObserved)
	{
		Detach(objectThatWasObserved, remove: true);
	}

	public void Detach(object objectThatWasObserved, bool remove)
    {
        if (!(objectThatWasObserved is TPublisher val)) return;
        lock (_observablesLock)
        {
            var weakReference = Find(val);
            if (weakReference == null) return;
            DetachOverride(val);
            if (remove)
            {
                _observables.Remove(val.GetHashCode());
            }
        }
    }

	public void Clear()
	{
		Each(delegate(TPublisher o)
		{
			Detach(o, remove: false);
		});
		_observables.Clear();
	}

	private void Each(Action<TPublisher> callback)
    {
        if (_observables == null) return;
        lock (_observablesLock)
        {
            foreach (var value in _observables.Values)
            {
                var target = value.Target;
                if (target is TPublisher obj)
                {
                    callback(obj);
                }
            }
        }
    }

	private WeakReference Find(TPublisher target)
	{
		WeakReference result = null;
        if (_observables == null) return null;
        lock (_observablesLock)
        {
            var hashCode = target.GetHashCode();
            if (_observables.ContainsKey(hashCode))
            {
                result = _observables[hashCode];
            }
        }
        return result;
	}

	protected abstract void DisposeOverride();
}
}