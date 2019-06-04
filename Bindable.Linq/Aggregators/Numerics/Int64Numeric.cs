using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class Int64Numeric : INumeric<long, double>
    {
        public long Sum(IEnumerable<long> sourceCollection)
        {
            return sourceCollection?.Sum() ?? 0L;
        }

        public double Average(IEnumerable<long> sourceCollection)
        {
            var num = 0.0;
            var num2 = 0;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    num += item;
                    num2++;
                }
            }
            if (num2 == 0)
            {
                return 0.0;
            }
            return num / num2;
        }

        public long Min(IEnumerable<long> sourceCollection)
        {
            var num = 0L;
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

        public long Max(IEnumerable<long> sourceCollection)
        {
            var num = 0L;
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