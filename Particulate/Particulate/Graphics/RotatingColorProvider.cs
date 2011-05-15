using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particulate.Graphics
{
    class RotatingColorProvider : ColorProvider
    {
        private double _h;
        private double _s;
        private double _l;

        public double H
        {
            get { return _h; }
            set
            {
                _h = Utility.Clamp(value, 0, 1);
            }
        }

        public double S
        {
            get { return _s; }
            set
            {
                _s = Utility.Clamp(value, 0, 1);
            }
        }

        public double L
        {
            get { return _l; }
            set
            {
                _l = Utility.Clamp(value, 0, 1);
            }
        }
        
        // Animation parameters
        private double _rotateRate;

        private bool _colorValid = false;
        private Color _lastColor;

        public RotatingColorProvider(double h, double s, double l, double rotateRate)
        {
            _h = h;
            _s = s;
            _l = l;
            _rotateRate = rotateRate;
        }

        public override Color GetColor()
        {
            // Check we don't need to do any more work
            if (_colorValid && _lastColor != null)
            {
                return _lastColor;
            }

            // Convert "HSL" style color to RGB
            _lastColor = HSL2RGB(_h, _s, _l);

            _colorValid = true;
            return _lastColor;
        }

        public static Color HSL2RGB(double h, double s, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + s)) : (l + s - l * s);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb = new Color();
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            return rgb;
        }

        public override void Update(double frameTime)
        {
            _h += frameTime * _rotateRate;
            while (_h > 1)
            {
                _h -= 1;
            }
            while (_h < 0)
            {
                _h += 1;
            }

            _colorValid = false;
        }
    }
}
