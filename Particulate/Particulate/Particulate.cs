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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PrimitiveBatch primitiveBatch;
        private SpriteFont _uiFont;
        private Texture2D _particleNormalTexture;

        private RenderTarget2D _lastFrame;
        private RenderTarget2D _renderTarget;

        private DistanceAttractor _fingerAttractor;

        public Particulate()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WorldState.ScreenWidth;
            graphics.PreferredBackBufferHeight = WorldState.ScreenHeight;
            graphics.IsFullScreen = true;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
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

            WorldState.WorldForces.Clear();
            //WorldState.WorldForces.Add(new DirectionalForce(new Vector2(0, 100)));
            //WorldState.WorldForces.Add(new DistanceAttractor(new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), 1, 200));
            WorldState.WorldForces.Add(new WallForce(100, 1.5, 0.15));
            WorldState.WorldForces.Add(_fingerAttractor);

            WorldState.WorldForces.Add(new FlockingForce(WorldState.FlockingForceStrength));

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
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(this.GraphicsDevice);
            
            _uiFont = Content.Load<SpriteFont>("UIFont");
            _particleNormalTexture = Content.Load<Texture2D>("Particle4");

            for (int i = 0; i < WorldState.NumParticles; i++)
            {
                WorldState.Sprites.Add(new Particle(new Vector2(WorldState.Rand.Next(50, WorldState.ScreenWidth - 50), WorldState.Rand.Next(50, WorldState.ScreenHeight - 50)), _particleNormalTexture));
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
            if (touches.Count > 0)
            {
                _fingerAttractor.AttractorPoint = touches[0].Position;
                _fingerAttractor.Attraction = 1;
            }
            else
            {
                _fingerAttractor.Attraction = 0;
            }

            // Prepare step
            double frameTime = gameTime.ElapsedGameTime.TotalSeconds * WorldState.TimeFactor;
            foreach (ISprite sprite in WorldState.Sprites)
            {
                sprite.PrepareAnimate(frameTime);
            }

            // Update step
            foreach (ISprite sprite in WorldState.Sprites)
            {
                sprite.Animate(frameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Clear rt
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);

            // Start rendering

            // Render last frame into rt with transparency
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (_lastFrame != null)
                spriteBatch.Draw(_lastFrame, new Rectangle(0, 0, WorldState.Width, WorldState.Height), new Color(0.88f, 0.8f, 0.6f));
            spriteBatch.End();


            primitiveBatch.Begin(PrimitiveType.TriangleList);
            // Render items into rt
            foreach (ISprite sprite in WorldState.Sprites)
            {
                sprite.Draw(graphics, spriteBatch, primitiveBatch);
            }

            primitiveBatch.End();
            
            // Save rt as "last frame"
            GraphicsDevice.SetRenderTarget(_lastFrame);
            spriteBatch.Begin();
            spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, WorldState.Width, WorldState.Height), Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            
            // Render rt into backbuffer
            spriteBatch.Begin();
            spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, WorldState.Width, WorldState.Height), Color.White);
            
            // Display memory
            const string current = "ApplicationCurrentMemoryUsage";
            long currentBytes = (long)DeviceExtendedProperties.GetValue(current);
            spriteBatch.DrawString(_uiFont, currentBytes.ToString(), new Vector2(10, 10), Color.White);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
