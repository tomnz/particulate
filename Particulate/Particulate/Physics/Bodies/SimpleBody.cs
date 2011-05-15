using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Particulate.Physics.Bodies
{
    class SimpleBody : IAnimatable
    {
        protected double _mass = 1;
        public double Mass
        {
            get { return _mass; }
            set { _mass = value; }
        }

        protected List<IForce> _forces;
        public List<IForce> Forces
        {
            get { return _forces; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Vector2 _lastPosition;
        public Vector2 LastPosition
        {
            get { return _lastPosition; }
        }

        private Vector2 _lastDrawnPosition;
        public Vector2 LastDrawnPosition
        {
            get { return _lastDrawnPosition; }
            set { _lastDrawnPosition = value; }
        }

        private Vector2 _velocity;
        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        private Vector2 _acceleration;

        private Vector2 _baseAcceleration; // Provides for constant override - preferred method is using forces
        public Vector2 BaseAcceleration
        {
            get { return _baseAcceleration; }
            set { _baseAcceleration = value; }
        }

        // Constructors
        public SimpleBody()
        {
            _forces = new List<IForce>();
            _position = new Vector2();
            _velocity = new Vector2();
            _baseAcceleration = new Vector2();
        }

        public SimpleBody(Vector2 position)
            : this()
        {
            _position = position;
            _lastPosition = position;
            _lastDrawnPosition = position;
        }

        public SimpleBody(Vector2 position, double mass)
            : this(position)
        {
            _mass = mass;
        }

        // Called to setup forces before final step
        public void PrepareAnimate(double time)
        {
            _acceleration = _baseAcceleration;
            foreach (IForce force in _forces)
            {
                _acceleration += force.GetForce(this);
            }
            foreach (IForce force in WorldState.WorldForces)
            {
                _acceleration += force.GetForce(this);
            }
        }

        // Final step
        public void Animate(double time)
        {
            _velocity += Vector2.Multiply(_acceleration, (float)time);

            if (_velocity.Length() > WorldState.MaxVel)
            {
                _velocity = Vector2.Multiply(_velocity, (float)(WorldState.MaxVel / _velocity.Length()));
            }

            _lastPosition = _position;
            _position += _velocity;

            if (_lastPosition == null)
                _lastPosition = _position;

            if (WorldState.WallCollide)
            {
                bool collision = true;
                while (collision)
                {
                    collision = false;
                    // Left wall
                    if (_position.X < 0)
                    {
                        _position.X += _lastPosition.X - _position.X;
                        _velocity.X *= -1;
                    }
                    // Right wall
                    if (_position.X > WorldState.ScreenWidth)
                    {
                        _position.X -= (_position.X - _lastPosition.X);
                        _velocity.X *= -1;
                    }
                    // Top wall
                    if (_position.Y < 0)
                    {
                        _position.Y += _lastPosition.Y - _position.Y;
                        _velocity.Y *= -1;
                    }
                    // Bottom wall
                    if (_position.Y > WorldState.ScreenHeight)
                    {
                        _position.Y -= (_position.Y - _lastPosition.Y);
                        _velocity.Y *= -1;
                    }
                }
            }
        }
    }
}
