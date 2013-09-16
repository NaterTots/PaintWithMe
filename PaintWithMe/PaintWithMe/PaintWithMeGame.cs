using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace PaintWithMe
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PaintWithMeGame : Microsoft.Xna.Framework.Game, IService
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        System.Threading.Timer _backgroundCycleTimer;

        const int BackgroundTimerPeriod = 1000 * 60 * 15; //Fifteen Minutes

        List<IInputDevice> inputDevices = new List<IInputDevice>();
        List<IPaintingAction> paintingActions = new List<IPaintingAction>();

        ServiceManager serviceManager = ServiceManager.Instance;

        Effect effectPost;
        RenderTarget2D renderTarget;

        public PaintWithMeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //TODO: is this the best place to decide which devices we need?
            serviceManager.Add(this);
            serviceManager.Add(new ConfigurationManager());
            serviceManager.Add(new LogManager());
            serviceManager.Add(new RandomGenerator());
            serviceManager.Add(new TextureManager());

            inputDevices.Add(new KinectDevice());
            inputDevices.Add(new KeyboardDevice());
            //inputDevices.Add(new JoystickDevice());
            inputDevices.Add(new XboxDrumSetDevice());
            inputDevices.Add(new SNESControllerDevice());

            _backgroundCycleTimer = new Timer(OnCycleBackground, null, -1, Timeout.Infinite);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            try
            {
                foreach (IService service in serviceManager)
                {
                    service.Initialize();
                }

                foreach (IInputDevice inputDevice in inputDevices)
                {
                    inputDevice.Initialize();
                }

                _backgroundCycleTimer.Change(BackgroundTimerPeriod, Timeout.Infinite);
            }
            catch (Exception e)
            {
                serviceManager.GetService<ILogger>(ServiceType.Logger).Log(e);
            }

            effectPost = Content.Load<Effect>("GrayscalePixelShader");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);//, 1, graphics.GraphicsDevice.DisplayMode.Format);
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            foreach (IPaintingAction paintingAction in paintingActions)
            {
                paintingAction.Update(gameTime);
            }

            foreach (IInputDevice inputDevice in inputDevices)
            {
                List<IPaintingAction> newPaintingActions;
                if (inputDevice.Update(gameTime, out newPaintingActions))
                {
                    foreach (IPaintingAction paintingAction in newPaintingActions)
                    {
                        paintingActions.Add(paintingAction);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            {
                // Apply the post process shader
                //effectPost.CurrentTechnique.Passes[0].Apply();

                foreach (IPaintingAction paintingAction in paintingActions)
                {
                    paintingAction.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void OnCycleBackground(object stateInfo)
        {
            //For now - remove all painting actions
            paintingActions.Clear();

            _backgroundCycleTimer.Change(BackgroundTimerPeriod, Timeout.Infinite);
        }

        #region IService

        void IService.Initialize()
        {
            //don't do anything, it already explicitly initializes
        }

        ServiceType IService.GetServiceType()
        {
            return ServiceType.Game;
        }

        #endregion IService
    }
}
