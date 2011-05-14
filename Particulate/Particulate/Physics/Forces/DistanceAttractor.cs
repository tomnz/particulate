using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Particulate.Physics.Bodies;

namespace Particulate.Physics.Forces
{
    class DistanceAttractor : IForce
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

        public DistanceAttractor(Vector2 attractorPoint, double attraction, double distance)
        {
            _attractorPoint = attractorPoint;
            _attraction = attraction;
            _distance = distance;
        }

        public Vector2 GetForce(SimpleBody body)
        {
            double magnitude = _attraction * body.Mass * Sq(Vector2.Distance(_attractorPoint, body.Position)) / _distance;
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
