using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Particulate.Physics.Bodies;

namespace Particulate.Physics.Forces
{
    class DirectionalForce : IForce
    {
        private Vector2 _force;

        public DirectionalForce(Vector2 force)
        {
            _force = force;
        }

        public Vector2 GetForce(SimpleBody body)
        {
            return _force;
        }
    }
}
