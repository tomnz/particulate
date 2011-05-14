using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Particulate.Graphics;
using Particulate.Physics.Bodies;
using Microsoft.Xna.Framework;

namespace Particulate.Physics.Forces
{
    class FlockingForce : IForce
    {
        private double _strength = 1;

        public FlockingForce()
        {
        }

        public FlockingForce(double strength)
            : this()
        {
        }

        public Vector2 GetForce(SimpleBody body)
        {
            List<SimpleBody> neighbours = new List<SimpleBody>();
            foreach (ISprite sprite in WorldState.Sprites)
            {
                if (sprite is Particle)
                    neighbours.Add(((Particle)sprite).Body);
            }

            neighbours.Remove(body);

            for (int i = neighbours.Count - 1; i >= 0; i--)
            {
                if (Vector2.Distance(body.Position, (neighbours[i]).Position) > WorldState.NeighbourRadius)
                {
                    neighbours.RemoveAt(i);
                }
            }

            if (neighbours.Count > 0)
                return Flock(neighbours, body);
            else
                return new Vector2(0, 0);
        }

        private Vector2 Flock(List<SimpleBody> neighbours, SimpleBody body)
        {
            Vector2 force = new Vector2(0, 0);

            force += Vector2.Multiply(Separate(neighbours, body), (float)WorldState.FlockingSeparationWeight);
            force += Vector2.Multiply(Align(neighbours, body), (float)WorldState.FlockingAlignWeight);
            force += Vector2.Multiply(Cohere(neighbours, body), (float)WorldState.FlockingCohereWeight);

            return Vector2.Multiply(force, (float)_strength);
        }

        private Vector2 Separate(List<SimpleBody> neighbours, SimpleBody body)
        {
            // Makes sure bodies don't stick together too much
            Vector2 force = new Vector2();

            int closeNeighbourCount = 0;

            foreach (SimpleBody neighbour in neighbours)
            {
                double distance = Vector2.Distance(neighbour.Position, body.Position);
                if (distance < WorldState.FlockingDesiredSeparation)
                {
                    Vector2 diff = Vector2.Subtract(body.Position, neighbour.Position);
                    diff.Normalize();
                    force += Vector2.Divide(diff, (float)distance);
                    closeNeighbourCount++;
                }
            }

            //if (closeNeighbourCount > 0)
            //    force = Vector2.Divide(force, closeNeighbourCount);

            return force;
        }

        private Vector2 Align(List<SimpleBody> neighbours, SimpleBody body)
        {
            // Keeps everything moving roughly together
            Vector2 centerLoc = new Vector2();

            foreach (SimpleBody neighbour in neighbours)
            {
                centerLoc += neighbour.Position;
            }

            centerLoc = Vector2.Divide(centerLoc, neighbours.Count);

            Vector2 force = Vector2.Subtract(centerLoc, body.Position);

            return force;
        }

        private Vector2 Cohere(List<SimpleBody> neighbours, SimpleBody body)
        {
            // Keeps everything moving roughly in the same direction
            Vector2 force = new Vector2();

            foreach (SimpleBody neighbour in neighbours)
            {
                force += neighbour.Velocity;
            }

            force = Vector2.Divide(force, neighbours.Count);

            return force;
        }
    }
}
