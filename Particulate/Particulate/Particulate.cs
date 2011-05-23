using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Info;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Particulate.Graphics;
using Particulate.Physics;
using Particulate.Physics.Forces;

namespace Particulate
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Particulate : Microsoft.Xna.Framework.Game
    {
        // Members
        GraphicsDeviceManager graphics;
        
        private SpriteBatch _spriteBatch;
        private PrimitiveBatch _primitiveBatch;

#if DEBUG
        private SpriteFont _uiFont;
#endif
        private Texture2D _aboutTexture;
        private Texture2D _aboutIconTexture;

        private RenderTarget2D _lastFrame;
        private RenderTarget2D _renderTarget;

        private DistanceAttractor _fingerAttractor;
        private DistanceAttractor _fingerAttractor1;
        private DistanceAttractor _fingerAttractor2;
        private FlockingForce _flockingForceRef;
        private RandomForce _randomForceRef;
        private bool _activateRandomForce = true;

        private bool _touchActive = false;

        TimeSpan fpsElapsedTime = TimeSpan.Zero;
        int fpsFrames = 0;
        double fpsFrameRate = 0;

        public Particulate()
        {
            // Tell the state we exist
            WorldState.Program = this;

            // Setup graphics
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WorldState.ScreenWidth;
            graphics.PreferredBackBufferHeight = WorldState.ScreenHeight;
            graphics.IsFullScreen = true;
            graphics.PreferMultiSampling = false;

            // Frame rate is 30 fps by default for Windows Phone.
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
            IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferFormat = SurfaceFormat.Bgr565;

            // Setup world
            _fingerAttractor = new DistanceAttractor(new Vector2(), 0, 100);
            _fingerAttractor1 = new DistanceAttractor(new Vector2(), 0, 100);
            _fingerAttractor2 = new DistanceAttractor(new Vector2(), 0, 100);

            WorldState.WorldForces.Clear();
            //WorldState.WorldForces.Add(new DirectionalForce(new Vector2(0, 100)));
            //WorldState.WorldForces.Add(new DistanceAttractor(new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), 1, 200));
            WorldState.WorldForces.Add(new WallForce(100, 1.5, 0.15));
            WorldState.WorldForces.Add(_fingerAttractor);

            _flockingForceRef = new FlockingForce(WorldState.FlockingForceStrength);
            WorldState.WorldForces.Add(_flockingForceRef);

            _randomForceRef = new RandomForce(150, 10000, 0);
            WorldState.WorldForces.Add(_randomForceRef);

            // Setup rendering
            _renderTarget = new RenderTarget2D(GraphicsDevice, WorldState.ScreenWidth, WorldState.ScreenHeight);
            _lastFrame = new RenderTarget2D(GraphicsDevice, WorldState.ScreenWidth, WorldState.ScreenHeight);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.GraphicsDevice);
            _primitiveBatch = new PrimitiveBatch(this.GraphicsDevice);
            
#if DEBUG
            _uiFont = Content.Load<SpriteFont>("UIFont");
#endif
            _aboutTexture = Content.Load<Texture2D>("About");
            _aboutIconTexture = Content.Load<Texture2D>("AboutIcon");

            // Spawn particles
            WorldState.NumParticles = 60;
        }

        /// <summary>
        /// Balances the number of active particles to match WorldState.NumParticles
        /// </summary>
        public void RefreshParticleCount()
        {
            // Add new particles
            double h = WorldState.Sprites.Count;
            while (WorldState.Sprites.Count < WorldState.NumParticles)
            {
                Particle p = new Particle(new Vector2(WorldState.Rand.Next(50, WorldState.ScreenWidth - 50), WorldState.Rand.Next(50, WorldState.ScreenHeight - 50)), new RotatingColorProvider(h, 0.7, 0.6, WorldState.ColorRotateRate));
                p.Body.Forces.Add(WorldState.Sprites.Count % 2 == 0 ? _fingerAttractor1 : _fingerAttractor2);
                WorldState.Sprites.Add(p);
                h += (0.2 / (double)WorldState.NumParticles);
            }
            // Remove excess particles
            while (WorldState.Sprites.Count > WorldState.NumParticles)
            {
                WorldState.Sprites.RemoveAt(WorldState.Sprites.Count - 1);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Touch attractor
            TouchCollection touches = TouchPanel.GetState();
            if (WorldState.ShowAbout)
            {
                if (touches.Count > 0 && !_touchActive)
                {
                    WorldState.ShowAbout = false;
                    WorldState.Paused = false;
                }
                else if (touches.Count == 0)
                {
                    _touchActive = false;
                }
            }
            else
            {
                if (touches.Count == 1)
                {
                    // About
                    if (touches[0].Position.X < 50 && touches[0].Position.Y < 50 && !_touchActive)
                    {
                        WorldState.ShowAbout = true;
                        WorldState.Paused = true;
                        _touchActive = true;
                        return;
                    }

                    // Single finger attractor
                    _fingerAttractor.AttractorPoint = touches[0].Position;
                    _fingerAttractor.Attraction = 1;

                    // Disable other effects
                    _fingerAttractor1.Attraction = 0;
                    _fingerAttractor2.Attraction = 0;
                    _flockingForceRef.SeparationStrength = 1;
                    _randomForceRef.Strength = 0;
                    _activateRandomForce = true;
                }
                else if (touches.Count == 2)
                {
                    // Split finger attractors
                    _fingerAttractor1.AttractorPoint = touches[0].Position;
                    _fingerAttractor2.AttractorPoint = touches[1].Position;
                    _fingerAttractor1.Attraction = 1;
                    _fingerAttractor2.Attraction = 1;

                    // Disable other effects
                    _fingerAttractor.Attraction = 0;
                    _flockingForceRef.SeparationStrength = 1;
                    _randomForceRef.Strength = 0;
                    _activateRandomForce = true;
                }
                else if (touches.Count == 3)
                {
                    // Go crazy
                    _flockingForceRef.SeparationStrength = 10;
                    if (_activateRandomForce)
                    {
                        _randomForceRef.Strength = 1;
                        _activateRandomForce = false;
                    }
                    else
                    {
                        _randomForceRef.Strength = 0;
                    }

                    // Disable other effects
                    _fingerAttractor.Attraction = 0;
                    _fingerAttractor1.Attraction = 0;
                    _fingerAttractor2.Attraction = 0;
                }
                else
                {
                    // Disable all effects
                    _fingerAttractor.Attraction = 0;
                    _fingerAttractor1.Attraction = 0;
                    _fingerAttractor2.Attraction = 0;
                    _flockingForceRef.SeparationStrength = 1;
                    _randomForceRef.Strength = 0;
                    _activateRandomForce = true;
                    _touchActive = false;
                }

                if (touches.Count > 0)
                {
                    _touchActive = true;
                }
            }


            // Prepare step
            double frameTime = gameTime.ElapsedGameTime.TotalSeconds * WorldState.TimeFactor;

            if (!WorldState.Paused)
            {
                foreach (ISprite sprite in WorldState.Sprites)
                {
                    sprite.PrepareAnimate(frameTime);
                }

                // Update step
                foreach (ISprite sprite in WorldState.Sprites)
                {
                    sprite.Animate(frameTime);
                }

                // Fade color
                WorldState.FadeColor.Update(frameTime);
            }

            // Calculate frame rate
            fpsElapsedTime += gameTime.ElapsedGameTime;
            fpsFrames++;
            if (fpsElapsedTime > TimeSpan.FromSeconds(1))
            {
                fpsFrameRate = (double)fpsFrames / fpsElapsedTime.TotalSeconds;
                fpsFrames = 0;
                fpsElapsedTime = TimeSpan.Zero;
            }

            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (WorldState.ShowAbout)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_aboutTexture, new Rectangle(0, 0, WorldState.ScreenWidth, WorldState.ScreenHeight), Color.White);
                _spriteBatch.End();
            }
            else
            {
                // Clear rt
                GraphicsDevice.SetRenderTarget(_renderTarget);
                GraphicsDevice.Clear(Color.Black);

                // Start rendering

                // Render last frame into rt with transparency
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                if (_lastFrame != null)
                    _spriteBatch.Draw(_lastFrame, new Rectangle(0, 0, WorldState.ScreenWidth, WorldState.ScreenHeight), WorldState.FadeColor.GetColor());
                _spriteBatch.End();


                _primitiveBatch.Begin(PrimitiveType.TriangleList);
                // Render items into rt
                foreach (ISprite sprite in WorldState.Sprites)
                {
                    sprite.Draw(graphics, _spriteBatch, _primitiveBatch);
                }

                _primitiveBatch.End();

                // Save rt as "last frame"
                GraphicsDevice.SetRenderTarget(_lastFrame);
                _spriteBatch.Begin();
                _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, WorldState.ScreenWidth, WorldState.ScreenHeight), Color.White);
                _spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Render rt into backbuffer
                _spriteBatch.Begin();
                _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, WorldState.ScreenWidth, WorldState.ScreenHeight), Color.White);

                // Display memory
#if DEBUG
                const string current = "ApplicationCurrentMemoryUsage";
                long currentBytes = (long)DeviceExtendedProperties.GetValue(current);
                //_spriteBatch.DrawString(_uiFont, currentBytes.ToString(), new Vector2(10, 10), Color.White);
                _spriteBatch.DrawString(_uiFont, fpsFrameRate.ToString("0.0"), new Vector2(10, 10), Color.White);
#endif

                _spriteBatch.Draw(_aboutIconTexture, new Rectangle(0, 0, _aboutIconTexture.Width, _aboutIconTexture.Height), Color.White);

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
