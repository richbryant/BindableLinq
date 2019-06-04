using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class Int32Numeric : INumeric<int, double>
    {
        public int Sum(IEnumerable<int> sourceCollection)
        {
            var num = 0;
            return sourceCollection?.Sum() ?? num;
        }

        public double Average(IEnumerable<int> sourceCollection)
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

        public int Min(IEnumerable<int> sourceCollection)
        {
            var num = 0;
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

        public int Max(IEnumerable<int> sourceCollection)
        {
            var num = 0;
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