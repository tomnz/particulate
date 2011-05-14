using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Particulate.Physics;
using Particulate.Graphics;

namespace Particulate
{
    class WorldState
    {
        private WorldState()
        {
            // Prevent instantiation
        }

        private static Random _rand = new Random();
        public static Random Rand
        {
            get { return WorldState._rand; }
        }


        // Basic properties
        private static int _width = 480;
        public static int Width
        {
            get { return WorldState._width; }
        }

        private static int _height = 800;
        public static int Height
        {
            get { return WorldState._height; }
        }

        private static double _maxVel = 10;
        public static double MaxVel
        {
            get { return _maxVel; }
            set { _maxVel = value; }
        }

        // State
        private static List<ISprite> _sprites = new List<ISprite>();
        public static List<ISprite> Sprites
        {
            get { return _sprites; }
            set { _sprites = value; }
        }

        private static List<IForce> _worldForces = new List<IForce>();
        public static List<IForce> WorldForces
        {
            get { return _worldForces; }
        }

        // Parameters
        private static double _timeFactor = 0.04;
        public static double TimeFactor
        {
            get { return _timeFactor; }
            set { _timeFactor = value; }
        }

        private static bool _wallCollide = false;
        public static bool WallCollide
        {
            get { return WorldState._wallCollide; }
            set { WorldState._wallCollide = value; }
        }

        // Flocking parameters
        private static double _neighbourRadius = 100;
        public static double NeighbourRadius
        {
            get { return WorldState._neighbourRadius; }
            set { WorldState._neighbourRadius = value; }
        }

        private static double _flockingDesiredSeparation = 20;
        public static double FlockingDesiredSeparation
        {
            get { return WorldState._flockingDesiredSeparation; }
            set { WorldState._flockingDesiredSeparation = value; }
        }

        private static double _flockingSeparationWeight = 400;
        public static double FlockingSeparationWeight
        {
            get { return WorldState._flockingSeparationWeight; }
            set { WorldState._flockingSeparationWeight = value; }
        }

        private static double _flockingAlignWeight = 1;
        public static double FlockingAlignWeight
        {
            get { return WorldState._flockingAlignWeight; }
            set { WorldState._flockingAlignWeight = value; }
        }

        private static double _flockingCohereWeight = 10;
        public static double FlockingCohereWeight
        {
            get { return WorldState._flockingCohereWeight; }
            set { WorldState._flockingCohereWeight = value; }
        }
    }
}
