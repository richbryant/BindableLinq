using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class DecimalNullNumeric : INumeric<decimal?, decimal?>
    {
        public decimal? Sum(IEnumerable<decimal?> sourceCollection)
        {
            var num = 0.00m;
            return sourceCollection?.Where(item => item.HasValue).Sum(item => item.Value) ?? num;
        }

        public decimal? Average(IEnumerable<decimal?> sourceCollection)
        {
            var d = 0m;
            var num = 0;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (!item.HasValue) continue;
                    d += item.Value;
                    num++;
                }
            }
            if (num == 0)
            {
                return null;
            }
            return d / num;
        }

        public decimal? Min(IEnumerable<decimal?> sourceCollection)
        {
            var num = 0m;
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

        public decimal? Max(IEnumerable<decimal?> sourceCollection)
        {
            var num = 0m;
            var flag = true;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    if (!item.HasValue) continue;
                    int num3;
                    if (!flag)
                    {
                        var num2 = item;
                        var d = num;
                        num3 = ((!(num2.GetValueOrDefault() > d)) ? 1 : 0);
                    }
                    else
                    {
                        num3 = 0;
                    }

                    if (num3 != 0) continue;
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