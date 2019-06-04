using System;

namespace Bindable.Linq.Dependencies
{
    public interface IDependency : IDisposable
    {
        void SetReevaluateElementCallback(Action<object, string> action);

        void SetReevaluateCallback(Action<object> action);
    }
}