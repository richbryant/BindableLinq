using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class Int64NullNumeric : INumeric<long?, double?>
    {
        public long? Sum(IEnumerable<long?> sourceCollection)
        {
            var num = 0L;
            return sourceCollection?.Where(item => item.HasValue).Sum(item => item.Value) ?? num;
        }

        public double? Average(IEnumerable<long?> sourceCollection)
        {
            var num = 0.0;
            var num2 = 0;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (!item.HasValue) continue;
                    num += item.Value;
                    num2++;
                }
            }
            if (num2 == 0)
            {
                return null;
            }
            return num / num2;
        }

        public long? Min(IEnumerable<long?> sourceCollection)
        {
            var num = 0L;
            var flag = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (!item.HasValue || (!flag && item.Value >= num)) continue;
                    num = item.Value;
                    flag = false;
                }
            }
            if (flag)
            {
                return null;
            }
            return num;
        }

        public long? Max(IEnumerable<long?> sourceCollection)
        {
            var num = 0L;
            var flag = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (!item.HasValue || !flag && !(item > num)) continue;
                    num = item.Value;
                    flag = false;
                }
            }
            if (flag)
            {
                return null;
            }
            return num;
        }
    }
}