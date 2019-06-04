using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class DoubleNumeric : INumeric<double, double>
    {
        public double Sum(IEnumerable<double> sourceCollection)
        {
            var num = 0.0;
            return sourceCollection?.Sum() ?? num;
        }

        public double Average(IEnumerable<double> sourceCollection)
        {
            var num = 0.0;
            var num2 = 0;
            if (sourceCollection != null)
            {
                foreach (var item in sourceCollection)
                {
                    var num3 = item;
                    num += num3;
                    num2++;
                }
            }
            if (num2 == 0)
            {
                return 0.0;
            }
            return num / num2;
        }

        public double Min(IEnumerable<double> sourceCollection)
        {
            var num = 0.0;
            var flag = true;
            if (sourceCollection == null) return num;
            foreach (var item in sourceCollection)
            {
                var num2 = item;
                if (!flag && !(num2 < num)) continue;
                num = num2;
                flag = false;
            }
            return num;
        }

        public double Max(IEnumerable<double> sourceCollection)
        {
            var num = 0.0;
            var flag = true;
            if (sourceCollection == null) return num;
            foreach (var item in sourceCollection)
            {
                var num2 = item;
                if (!flag && !(num2 > num)) continue;
                num = num2;
                flag = false;
            }
            return num;
        }
    }
}