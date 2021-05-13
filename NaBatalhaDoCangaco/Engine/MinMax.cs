using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroides.Engine
{
    public class MinMax
    {
        public float Min { get; set; }
        public float Max { get; set; }

        public MinMax()
        {
        }

        public MinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public MinMax(float value)
        {
            Min = Max = value;
        }

        public float Random()
        {
            return Min + (float)RandomSingleton.Instance.NextDouble() * (Max - Min);
        }

        public int RandomInt()
        {
            return (int)Math.Round(Random());
        }
    }
}
