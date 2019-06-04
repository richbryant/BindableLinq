using System.Collections.Generic;

namespace Bindable.Linq.Helpers
{
    internal static class ElementComparerFactory
    {
        private class ReferenceTypeComparer<TElement> : IEqualityComparer<TElement>
        {
            public bool Equals(TElement x, TElement y)
            {
                return object.ReferenceEquals(x, y);
            }

            public int GetHashCode(TElement obj)
            {
                return obj.GetHashCode();
            }
        }

        private class ValueTypeComparer<TElement> : IEqualityComparer<TElement>
        {
            public bool Equals(TElement x, TElement y)
            {
                return object.Equals(x, y);
            }

            public int GetHashCode(TElement obj)
            {
                return obj.GetHashCode();
            }
        }

        public static IEqualityComparer<TElement> Create<TElement>()
        {
            if (typeof(TElement).IsValueType)
            {
                return new ValueTypeComparer<TElement>();
            }
            return new ReferenceTypeComparer<TElement>();
        }
    }
}