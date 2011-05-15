using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particulate
{
    class Utility
    {
        private Utility()
        {
            // Prevent instantiation
        }

        public static double Clamp(double input, double min, double max)
        {
            if (input < min)
                return min;
            if (input > max)
                return max;
            return input;
        }
    }
}
