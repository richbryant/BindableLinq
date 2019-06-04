using System;
using System.Reflection;
using System.Windows;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    internal sealed class SilverlightMemberToken : MemberToken
{
	private readonly EventHandler _actualHandler;

	private readonly DependencyProperty _dependencyProperty;

	public SilverlightMemberToken(DependencyObject objectToObserve, DependencyProperty dependencyProperty, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
		: base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
	{
		_dependencyProperty = dependencyProperty;
		_actualHandler = CurrentTarget_PropertyChanged;
		AcquireTarget(objectToObserve);
	}

	protected override void DiscardCurrentTargetOverride()
	{
        if (base.CurrentTarget is DependencyObject dependencyObject)
		{
			base.CurrentTarget.GetType().GetEvent(base.PropertyName + "Changed")?.GetRemoveMethod()?.Invoke(base.CurrentTarget, new object[1]
			{
				_actualHandler
			});
		}
	}

	protected override void MonitorCurrentTargetOverride()
	{
		DependencyObject dependencyObject = base.CurrentTarget as DependencyObject;
		if (dependencyObject == null)
		{
			return;
		}
		EventInfo @event = base.CurrentTarget.GetType().GetEvent(base.PropertyName + "Changed");
		if (@event != null)
		{
			MethodInfo addMethod = @event.GetAddMethod();
			if (addMethod != null)
			{
				ParameterInfo parameterInfo = addMethod.GetParameters()[0];
				Delegate @delegate = Delegate.CreateDelegate(parameterInfo.ParameterType, this, GetType().GetMethod("CurrentTarget_PropertyChanged", BindingFlags.Instance | BindingFlags.Public));
				addMethod.Invoke(base.CurrentTarget, new object[1]
				{
					@delegate
				});
			}
		}
	}

	protected override object ReadCurrentPropertyValueOverride()
	{
		if (_dependencyProperty != null && base.CurrentTarget != null)
		{
			return ((DependencyObject)base.CurrentTarget).GetValue(_dependencyProperty);
		}
		return null;
	}

	public void CurrentTarget_PropertyChanged(object sender, EventArgs e)
	{
		HandleCurrentTargetPropertyValueChanged();
	}

	protected override void DisposeOverride()
	{
		DiscardCurrentTargetOverride();
	}
}
}