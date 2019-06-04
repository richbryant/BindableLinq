using System;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    internal sealed class WindowsFormsMemberToken : MemberToken
    {
        private readonly EventHandler _actualHandler;

        private IPropertyReader<object> _propertyReader;

        public WindowsFormsMemberToken(object objectToObserve, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _actualHandler = CurrentTarget_PropertyChanged;
            AcquireTarget(objectToObserve);
        }

        protected override void DiscardCurrentTargetOverride()
        {
            CurrentTarget?.GetType().GetEvent(PropertyName + "Changed")?.GetRemoveMethod()
                .Invoke(CurrentTarget, new object[]
            {
                _actualHandler
            });
        }

        protected override void MonitorCurrentTargetOverride()
        {
            CurrentTarget?.GetType().GetEvent(PropertyName + "Changed")?.GetAddMethod()
                .Invoke(CurrentTarget, new object[]
            {
                _actualHandler
            });
            _propertyReader = PropertyReaderFactory.Create<object>(CurrentTarget?.GetType(), PropertyName);
        }

        protected override object ReadCurrentPropertyValueOverride()
        {
            return _propertyReader?.GetValue(CurrentTarget);
        }

        private void CurrentTarget_PropertyChanged(object sender, EventArgs e)
        {
            HandleCurrentTargetPropertyValueChanged();
        }

        protected override void DisposeOverride()
        {
            DiscardCurrentTargetOverride();
        }
    }
}