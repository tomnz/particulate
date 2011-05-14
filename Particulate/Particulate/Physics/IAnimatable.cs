using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particulate.Physics
{
    interface IAnimatable
    {
        void PrepareAnimate(double time);
        void Animate(double time);
    }
}
