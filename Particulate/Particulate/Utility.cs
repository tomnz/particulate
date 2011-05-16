using Microsoft.Xna.Framework;
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

        public static Color ColorMultiply(Color a, Color b)
        {
            return new Color((int)(((double)a.R / 255) * ((double)b.R / 255) * 255),
                (int)(((double)a.G / 255) * ((double)b.G / 255) * 255),
                (int)(((double)a.B / 255) * ((double)b.B / 255) * 255));
        }
    }
}
