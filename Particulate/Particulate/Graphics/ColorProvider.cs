using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particulate.Graphics
{
    class ColorProvider
    {
        private Color _color;

        public ColorProvider()
        {
            _color = Color.White;
        }

        public ColorProvider(Color color)
        {
            _color = color;
        }

        public virtual Color GetColor()
        {
            return _color;
        }

        public virtual void Update(double frameTime)
        {
        }
    }
}
