using System;
using System.ComponentModel;
using System.Windows;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    internal sealed class WpfMemberToken : MemberToken
    {
        private readonly DependencyProperty _dependencyProperty;

        public WpfMemberToken(DependencyObject objectToObserve, DependencyProperty dependencyProperty, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _dependencyProperty = dependencyProperty;
            AcquireTarget(objectToObserve);
        }

        protected override void DiscardCurrentTargetOverride()
        {
            if (CurrentTarget is DependencyObject dependencyObject)
            {
                DependencyPropertyDescriptor.FromProperty(_dependencyProperty, dependencyObject.GetType())?.RemoveValueChanged(dependencyObject, CurrentTarget_PropertyChanged);
            }
        }

        protected override void MonitorCurrentTargetOverride()
        {
            if (CurrentTarget is DependencyObject dependencyObject)
            {
                DependencyPropertyDescriptor.FromProperty(_dependencyProperty, dependencyObject.GetType())?.AddValueChanged(dependencyObject, CurrentTarget_PropertyChanged);
            }
        }

        protected override object ReadCurrentPropertyValueOverride()
        {
            if (_dependencyProperty != null && CurrentTarget != null)
            {
                return ((DependencyObject)CurrentTarget).GetValue(_dependencyProperty);
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