using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Particulate.Physics.Bodies;

namespace Particulate.Physics.Forces
{
    class GravityAttractor : IForce
    {
        private Vector2 _attractorPoint;
        public Vector2 AttractorPoint
        {
            get { return _attractorPoint; }
            set { _attractorPoint = value; }
        }

        private double _attraction = 1;
        public double Attraction
        {
            get { return _attraction; }
            set { _attraction = value; }
        }

        private double _distance = 1;
        public double Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        public GravityAttractor(Vector2 attractorPoint, double attraction)
        {
            _attractorPoint = attractorPoint;
            _attraction = attraction;
        }

        public Vector2 GetForce(SimpleBody body)
        {
            double magnitude = _attraction * body.Mass / (Sq(Vector2.Distance(_attractorPoint, body.Position)) / 100);
            Vector2 direction = _attractorPoint - body.Position;
            direction.Normalize();

            return Vector2.Multiply(direction, (float)magnitude);
        }

        private double Sq(double val)
        {
            return val * val;
        }
    }
}
