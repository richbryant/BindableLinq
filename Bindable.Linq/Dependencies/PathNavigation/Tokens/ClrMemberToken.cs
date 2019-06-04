using System;
using System.ComponentModel;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    internal sealed class ClrMemberToken : MemberToken
    {
        private readonly EventHandler<PropertyChangedEventArgs> _actualHandler;

        private readonly WeakEventProxy<PropertyChangedEventArgs> _weakHandler;

        private readonly PropertyChangedEventHandler _weakHandlerWrapper;

        private IPropertyReader<object> _propertyReader;

        public ClrMemberToken(object objectToObserve, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _actualHandler = CurrentTarget_PropertyChanged;
            _weakHandler = new WeakEventProxy<PropertyChangedEventArgs>(_actualHandler);
            _weakHandlerWrapper = _weakHandler.Handler;
            AcquireTarget(objectToObserve);
        }

        protected override void DiscardCurrentTargetOverride()
        {
            if (CurrentTarget is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= _weakHandlerWrapper;
            }
        }

        protected override void MonitorCurrentTargetOverride()
        {
            if (CurrentTarget is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += _weakHandlerWrapper;
            }
            _propertyReader = PropertyReaderFactory.Create<object>(CurrentTarget.GetType(), PropertyName);
        }

        protected override object ReadCurrentPropertyValueOverride()
        {
            return _propertyReader?.GetValue(CurrentTarget);
        }

        private void CurrentTarget_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName)
            {
                HandleCurrentTargetPropertyValueChanged();
            }
        }

        protected override void DisposeOverride()
        {
            DiscardCurrentTargetOverride();
        }
    }

}