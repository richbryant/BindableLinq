using System;
using System.Collections.Generic;

namespace Bindable.Linq.Helpers
{
    public sealed class LifetimeCouplings : IDisposable
    {
        private readonly List<object> _objects;

        public LifetimeCouplings()
        {
            _objects = new List<object>();
        }

        public void Add(object instance)
        {
            _objects.Add(instance);
        }

        public void Dispose()
        {
            _objects.Clear();
        }
    }
}