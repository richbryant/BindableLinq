using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class Int32NullNumeric : INumeric<int?, double?>
    {
        public int? Sum(IEnumerable<int?> sourceCollection)
        {
            var num = 0;
            return sourceCollection?.Where(item => item.HasValue).Sum(item => item.Value) ?? num;
        }

        public double? Average(IEnumerable<int?> sourceCollection)
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

        public int? Min(IEnumerable<int?> sourceCollection)
        {
            var num = 0;
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

        public int? Max(IEnumerable<int?> sourceCollection)
        {
            var num = 0;
            var flag = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (!item.HasValue || (!flag && !(item > num))) continue;
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