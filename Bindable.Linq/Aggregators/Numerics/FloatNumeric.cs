using System.Collections.Generic;
using System.Linq;

namespace Bindable.Linq.Aggregators.Numerics
{
    internal class FloatNumeric : INumeric<float, float>
    {
        public float Sum(IEnumerable<float> sourceCollection)
        {
            var num = 0f;
            return sourceCollection?.Sum() ?? num;
        }

        public float Average(IEnumerable<float> sourceCollection)
        {
            var num = 0f;
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
                return 0f;
            }
            return num / num2;
        }

        public float Min(IEnumerable<float> sourceCollection)
        {
            var num = 0f;
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

        public float Max(IEnumerable<float> sourceCollection)
        {
            var num = 0f;
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