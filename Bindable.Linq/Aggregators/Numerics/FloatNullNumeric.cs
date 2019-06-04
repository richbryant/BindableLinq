using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class FloatNullNumeric : INumeric<float?, float?>
    {
        public float? Sum(IEnumerable<float?> sourceCollection)
        {
            var num = 0f;
            return sourceCollection?.Where(item => item.HasValue).Sum(item => item.Value) ?? num;
        }

        public float? Average(IEnumerable<float?> sourceCollection)
        {
            var num = 0f;
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

        public float? Min(IEnumerable<float?> sourceCollection)
        {
            var num = 0f;
            var flag = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (!item.HasValue || !flag && !(item.Value < num)) continue;
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

        public float? Max(IEnumerable<float?> sourceCollection)
        {
            var num = 0f;
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