using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Particulate.Physics.Bodies;

namespace Particulate.Physics.Forces
{
    class RandomForce : IForce
    {
        private double _minAmplitude;
        private double _maxAmplitude;
        private double _strength;

        public double Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        public RandomForce(double minAmplitude, double maxAmplitude, double strength)
        {
            _minAmplitude = minAmplitude;
            _maxAmplitude = maxAmplitude;
            _strength = strength;
        }

        public Vector2 GetForce(SimpleBody body)
        {
            double angle = WorldState.Rand.NextDouble() * 2 * Math.PI;
            Vector2 force = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            force.Normalize();
            force = Vector2.Multiply(force, (float)((WorldState.Rand.NextDouble() * (_maxAmplitude - _minAmplitude) * _strength) + _minAmplitude));
            return force;
        }
    }
}
