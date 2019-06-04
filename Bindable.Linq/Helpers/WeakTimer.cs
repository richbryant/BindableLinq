using System;
using System.Threading;

namespace Bindable.Linq.Helpers
{
    internal sealed class WeakTimer : IDisposable
    {
        private readonly WeakReference _callbackReference;

        private readonly TimeSpan _pollTime;

        private Timer _timer;

        public WeakTimer(TimeSpan pollTime, Action callback)
        {
            _pollTime = pollTime;
            _callbackReference = new WeakReference(callback, trackResurrection: true);
        }

        public void Start()
        {
            _timer = new Timer(TimerTickCallback, null, _pollTime, _pollTime);
        }

        public void Pause()
        {
            _timer?.Change(-1, -1);
        }

        public void Continue()
        {
            _timer?.Change(_pollTime, _pollTime);
        }

        private void TimerTickCallback(object o)
        {
            (_callbackReference.Target as Action)?.Invoke();
        }

        public void Dispose()
        {
            Pause();
            _timer.Dispose();
        }
    }
}