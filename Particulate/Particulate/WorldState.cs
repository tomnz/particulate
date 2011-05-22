using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Particulate.Physics;
using Particulate.Graphics;

namespace Particulate
{
    public enum LineWidthMode
    {
        Fixed,
        Velocity
    }
    class WorldState
    {
        private WorldState()
        {
            // Prevent instantiation
        }

        // Single Particulate reference
        public static Particulate Program { get; set; }
        

        // Helper members
        private static Random _rand = new Random();
        public static Random Rand
        {
            get { return WorldState._rand; }
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

        // Basic settings
        private static int _screenWidth = 480;
        public static int ScreenWidth
        {
            get { return WorldState._screenWidth; }
            set { WorldState._screenWidth = value; }
        }

        private static int _screenHeight = 800;
        public static int ScreenHeight
        {
            get { return WorldState._screenHeight; }
            set { WorldState._screenHeight = value; }
        }

        private static int _numParticles;
        public static int NumParticles
        {
            get { return WorldState._numParticles; }
            set { WorldState._numParticles = value; Program.RefreshParticleCount(); }
        }

        // Particle appearance
        private static float _lineWidthMin = 1.0f;
        public static float LineWidthMin
        {
            get { return WorldState._lineWidthMin; }
            set { WorldState._lineWidthMin = value; }
        }

        private static float _lineWidthMax = 3.0f;
        public static float LineWidthMax
        {
            get { return WorldState._lineWidthMax; }
            set { WorldState._lineWidthMax = value; }
        }

        private static LineWidthMode _lineWidthMode = LineWidthMode.Velocity;
        public static LineWidthMode LineWidthMode
        {
            get { return WorldState._lineWidthMode; }
            set { WorldState._lineWidthMode = value; }
        }

        private static double _colorRotateRate = 1;
        public static double ColorRotateRate
        {
            get { return WorldState._colorRotateRate; }
            set { WorldState._colorRotateRate = value; }
        }

        private static ColorProvider _fadeColor = new RotatingColorProvider(0.119, 0.278, 0.77, WorldState.ColorRotateRate);
        public static ColorProvider FadeColor
        {
            get { return _fadeColor; }
            set { _fadeColor = value; }
        }


        // Flocking parameters
        private static double _maxVel = 10;
        public static double MaxVel
        {
            get { return _maxVel; }
            set { _maxVel = value; }
        }

        private static double _flockingForceStrength = 0.001;
        public static double FlockingForceStrength
        {
            get { return WorldState._flockingForceStrength; }
            set { WorldState._flockingForceStrength = value; }
        }

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
