using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Particulate.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Particulate.Physics;
using Particulate.Physics.Bodies;


namespace Particulate.Graphics
{
    class Particle : ISprite
    {
        private static readonly double ATAN2_VERTICAL = 1.5708;

        private Vector2 _lastLeftTip;
        private Vector2 _lastRightTip;
        private bool _drawnOnce = false;

        private ColorProvider _color;

        private SimpleBody _body;
        internal SimpleBody Body
        {
            get { return _body; }
        }

        public Particle(Vector2 initialPosition)
        {
            _body = new SimpleBody(initialPosition);
            _color = new ColorProvider(Color.White);
        }

        public Particle(Vector2 initialPosition, ColorProvider color)
        {
            _body = new SimpleBody(initialPosition);
            _color = color;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            float lineWidth = GetLineWidth();
            float length = (_body.Position - _body.LastDrawnPosition).Length();
            Color fadedColor = Utility.ColorMultiply(WorldState.FadeColor.GetColor(), _color.GetColor());

            // Set up the transform matrix
            Vector2 trajectory = _body.Position - _body.LastDrawnPosition;
            float angle = (float)(Math.Atan2(trajectory.Y, trajectory.X) - ATAN2_VERTICAL);
            Matrix transform = Matrix.Multiply(Matrix.CreateRotationZ(angle), Matrix.CreateTranslation(_body.LastDrawnPosition.X, _body.LastDrawnPosition.Y, 0));

            Vector2 newLeftTip = Vector2.Transform(new Vector2(-lineWidth, length), transform);
            Vector2 newRightTip = Vector2.Transform(new Vector2(lineWidth, length), transform);

            if (_drawnOnce)
            {
                primitiveBatch.AddVertex(newRightTip, _color.GetColor());
                primitiveBatch.AddVertex(newLeftTip, _color.GetColor());
                primitiveBatch.AddVertex(_lastLeftTip, fadedColor);
                primitiveBatch.AddVertex(_lastLeftTip, fadedColor);
                primitiveBatch.AddVertex(_lastRightTip, fadedColor);
                primitiveBatch.AddVertex(newRightTip, _color.GetColor());
            }
            else
            {
                _drawnOnce = true;
            }
            
            _lastLeftTip = newLeftTip;
            _lastRightTip = newRightTip;

            _body.LastDrawnPosition = _body.Position;
        }

        public void PrepareAnimate(double time)
        {
            _body.PrepareAnimate(time);
        }

        public void Animate(double time)
        {
            _body.Animate(time);
            _color.Update(time);
        }

        private float GetLineWidth()
        {
            switch (WorldState.LineWidthMode)
            {
                case LineWidthMode.Velocity:
                    return (float)((WorldState.MaxVel - _body.Velocity.Length()) / WorldState.MaxVel) * (WorldState.LineWidthMax - WorldState.LineWidthMin) + WorldState.LineWidthMin;
                default:
                    return WorldState.LineWidthMin;
            }
        }
    }
}
