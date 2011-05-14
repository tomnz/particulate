using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Particulate.Physics.Bodies;

namespace Particulate.Physics
{
    interface IForce
    {
        Vector2 GetForce(SimpleBody body);
    }
}
