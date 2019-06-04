using System.Collections.Generic;

namespace Bindable.Linq.Dependencies
{
    internal sealed class DependencyComparer : IEqualityComparer<IDependencyDefinition>
    {
        public static DependencyComparer Instance { get; } = new DependencyComparer();

        private DependencyComparer()
        {
        }

        public bool Equals(IDependencyDefinition x, IDependencyDefinition y)
        {
            return (x == null && y == null) || (x != null && y != null && x.ToString() == y.ToString());
        }

        public int GetHashCode(IDependencyDefinition obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}