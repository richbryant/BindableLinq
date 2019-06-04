using System;
using System.Collections.Generic;

namespace Bindable.Linq.Helpers
{
    internal sealed class DefaultComparer<TCompared> : IComparer<TCompared>, IEqualityComparer<TCompared>
    {
        public int Compare(TCompared left, TCompared right)
        {
            if (left == null && right == null)
            {
                return 0;
            }
            if (left == null)
            {
                return -1;
            }
            if (right == null)
            {
                return 1;
            }
            return (left as IComparable<TCompared>)?.CompareTo(right) ?? (left as IComparable)?.CompareTo(right) ?? left.GetHashCode().CompareTo(right.GetHashCode());
        }

        public bool Equals(TCompared x, TCompared y)
        {
            var flag = x == null && y == null;
            if (!flag && x != null && y != null)
            {
                flag = ((x as IEquatable<TCompared>)?.Equals(y) ?? x.Equals(y));
            }
            return flag;
        }

        public int GetHashCode(TCompared obj)
        {
            return obj.GetHashCode();
        }
    }
}