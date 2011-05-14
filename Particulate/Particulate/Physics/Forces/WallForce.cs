using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Particulate.Physics.Bodies;

namespace Particulate.Physics.Forces
{
    class WallForce : IForce
    {
        private double _forceDistance = 50;
        private double _strength = 1;
        private double _squareStrength = 1;

        public WallForce()
        {
        }

        public WallForce(double forceDistance, double strength, double squareStrength)
            : this()
        {
            _forceDistance = forceDistance;
            _strength = strength;
            _squareStrength = squareStrength;
        }

        public Vector2 GetForce(SimpleBody body)
        {
            Vector2 force = new Vector2(0, 0);
            Vector2 squareForce = new Vector2(0, 0);

            float leftDistance = ((float)_forceDistance - body.Position.X);
            float rightDistance = (body.Position.X - (WorldState.Width - (float)_forceDistance));
            float topDistance = ((float)_forceDistance - body.Position.Y);
            float bottomDistance = (body.Position.Y - (WorldState.Height - (float)_forceDistance));
            if (leftDistance > 0)
            {
                force.X += leftDistance;
                squareForce.X += leftDistance * leftDistance;
            }
            if (rightDistance > 0)
            {
                force.X -= rightDistance;
                squareForce.X -= rightDistance * rightDistance;
            }
            if (topDistance > 0)
            {
                force.Y += topDistance;
                squareForce.Y += topDistance * topDistance;
            }
            if (bottomDistance > 0)
            {
                force.Y -= bottomDistance;
                squareForce.Y -= bottomDistance * bottomDistance;
            }

            return Vector2.Multiply(force, (float)_strength) + Vector2.Multiply(squareForce, (float)_squareStrength);
        }
    }
}
