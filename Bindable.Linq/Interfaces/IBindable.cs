using System;
using System.ComponentModel;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Interfaces
{
    public interface IBindable : INotifyPropertyChanged, IRefreshable, IDisposable
    {
        IDispatcher Dispatcher
        {
            get;
        }

        bool HasEvaluated
        {
            get;
        }

        void Evaluate();
    }

    public interface IBindable<TValue> : IBindable, INotifyPropertyChanged, IRefreshable, IDisposable
    {
        TValue Current
        {
            get;
        }
    }
}