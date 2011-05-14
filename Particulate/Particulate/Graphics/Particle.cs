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
        private SimpleBody _body;
        internal SimpleBody Body
        {
            get { return _body; }
        }

        private Texture2D _texture;

        public Particle(Vector2 initialPosition, Texture2D texture)
        {
            _body = new SimpleBody(initialPosition);
            _texture = texture;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, PrimitiveBatch primitiveBatch)
        {
            float lineWidth = 1.0f;

            List<Vector2> vertices = new List<Vector2>();
            float length = (_body.Position - _body.LastDrawnPosition).Length();

            Vector2 trajectory = _body.Position - _body.LastDrawnPosition;
            float angle = (float)(Math.Atan2(trajectory.Y, trajectory.X) - Math.Atan2(1, 0));
            Matrix transform = Matrix.Multiply(Matrix.CreateRotationZ(angle), Matrix.CreateTranslation(_body.LastDrawnPosition.X, _body.LastDrawnPosition.Y, 0));


            primitiveBatch.AddVertex(Vector2.Transform(new Vector2(lineWidth, length + lineWidth), transform), Color.White);
            primitiveBatch.AddVertex(Vector2.Transform(new Vector2(-lineWidth, length + lineWidth), transform), Color.White);
            primitiveBatch.AddVertex(Vector2.Transform(new Vector2(-lineWidth, -lineWidth), transform), Color.White);
            primitiveBatch.AddVertex(Vector2.Transform(new Vector2(-lineWidth, -lineWidth), transform), Color.White);
            primitiveBatch.AddVertex(Vector2.Transform(new Vector2(lineWidth, -lineWidth), transform), Color.White);
            primitiveBatch.AddVertex(Vector2.Transform(new Vector2(lineWidth, length + lineWidth), transform), Color.White);

            _body.LastDrawnPosition = _body.Position;
        }

        public void PrepareAnimate(double time)
        {
            _body.PrepareAnimate(time);
        }

        public void Animate(double time)
        {
            _body.Animate(time);
        }
    }
}
