using System;

namespace Bindable.Linq.Threading
{
    public interface IDispatcher
    {
        void Dispatch(Action actionToInvoke);

        TResult Dispatch<TResult>(Func<TResult> actionToInvoke);

        bool DispatchRequired();
    }
}