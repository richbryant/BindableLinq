using System;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    internal abstract class MemberToken : IToken, IDisposable
{
	private readonly Action<object, string> _changeDetectedCallback;

	private readonly IPathNavigator _pathNavigator;

	private readonly object _propertyMonitorLock = new object();

	private readonly string _propertyName;

	private readonly string _remainingPath;

	private object _currentTarget;

	private IToken _nextMonitor;

	public string RemainingPath => _remainingPath;

	public string PropertyName => _propertyName;

	protected object CurrentTarget => _currentTarget;

	protected object PropertyMonitorLock => _propertyMonitorLock;

	protected IPathNavigator PathNavigator => _pathNavigator;

	public IToken NextToken
	{
		get
		{
			return _nextMonitor;
		}
		private set
		{
			if (_nextMonitor != null)
			{
				_nextMonitor.Dispose();
			}
			_nextMonitor = value;
		}
	}

	public MemberToken(object currentTarget, string propertyName, string remainingPath, Action<object, string> changeDetectedCallback, IPathNavigator traverser)
	{
		_changeDetectedCallback = changeDetectedCallback;
		_remainingPath = remainingPath;
		_propertyName = propertyName;
		_pathNavigator = traverser;
	}

	public void AcquireTarget(object target)
	{
		lock (PropertyMonitorLock)
		{
			if (CurrentTarget != null)
			{
				DiscardCurrentTargetOverride();
			}
			_currentTarget = target;
			if (CurrentTarget != null)
			{
				MonitorCurrentTargetOverride();
				NextToken = PathNavigator.TraverseNext(ReadCurrentPropertyValueOverride(), _remainingPath, NextMonitor_ChangeDetected);
			}
		}
	}

	public void Dispose()
	{
		DisposeOverride();
		NextToken = null;
	}

	protected abstract void DiscardCurrentTargetOverride();

	protected abstract void MonitorCurrentTargetOverride();

	protected abstract object ReadCurrentPropertyValueOverride();

	private void NextMonitor_ChangeDetected(object changedObject, string propertyName)
	{
		ChangeDetected(_propertyName + "." + propertyName);
	}

	protected void HandleCurrentTargetPropertyValueChanged()
	{
		lock (PropertyMonitorLock)
		{
			object target = ReadCurrentPropertyValueOverride();
			NextToken = PathNavigator.TraverseNext(target, _remainingPath, NextMonitor_ChangeDetected);
		}
		ChangeDetected(_propertyName);
	}

	private void ChangeDetected(string propertyName)
	{
		if (_changeDetectedCallback != null)
		{
			_changeDetectedCallback(_currentTarget, propertyName);
		}
	}

	protected abstract void DisposeOverride();
}

}