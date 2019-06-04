using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class DecimalNumeric : INumeric<decimal, decimal>
    {
        public decimal Sum(IEnumerable<decimal> sourceCollection)
        {
            var num = 0m;
            if (sourceCollection != null)
            {
                num += sourceCollection.Sum();
            }
            return num;
        }

        public decimal Average(IEnumerable<decimal> sourceCollection)
        {
            var d = 0m;
            var num = 0;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    d += item;
                    num++;
                }
            }
            if (num == 0)
            {
                return 0m;
            }
            return d / num;
        }

        public decimal Min(IEnumerable<decimal> sourceCollection)
        {
            var num = 0m;
            var flag = true;
            if (sourceCollection == null) return num;
            foreach (var item in sourceCollection)
            {
                if (!flag && item >= num) continue;
                num = item;
                flag = false;
            }
            return num;
        }

        public decimal Max(IEnumerable<decimal> sourceCollection)
        {
            var num = 0m;
            var flag = true;
            if (sourceCollection == null) return num;
            foreach (var item in sourceCollection)
            {
                if (!flag && item <= num) continue;
                num = item;
                flag = false;
            }
            return num;
        }
    }
}