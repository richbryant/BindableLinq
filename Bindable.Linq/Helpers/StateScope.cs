using System;

namespace Bindable.Linq.Helpers
{
    public sealed class StateScope : IDisposable
    {
        private readonly Action _callback;

        private readonly object _stateScopeLock = new object();

        private int _childrenCount;

        public bool IsWithin => _childrenCount > 0;

        public StateScope()
        {
        }

        public StateScope(Action callback)
        {
            _callback = callback;
        }

        public StateScope Enter()
        {
            bool flag = false;
            lock (_stateScopeLock)
            {
                bool isWithin = IsWithin;
                _childrenCount++;
                if (isWithin != IsWithin && _callback != null)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                _callback();
            }
            return this;
        }

        public void Leave()
        {
            bool flag = false;
            lock (_stateScopeLock)
            {
                if (_childrenCount > 0)
                {
                    bool isWithin = IsWithin;
                    _childrenCount--;
                    if (isWithin != IsWithin && _callback != null)
                    {
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                _callback();
            }
        }

        void IDisposable.Dispose()
        {
            Leave();
        }
    }
}