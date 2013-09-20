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

        const int MaxStrokeCount = 500;
        const int StrokeShiftCoefficient = 10;
        static TimeSpan TimeBetweenKillingPaintingStrokes = new TimeSpan(0, 0, 10); //10 seconds

        DateTime nextTimeToKillPaintingStrokes;


        List<IInputDevice> inputDevices = new List<IInputDevice>();
        List<IPaintingAction> paintingActions = new List<IPaintingAction>();

        ServiceManager serviceManager = ServiceManager.Instance;

        CanvasBackground _canvasBackground;

        Effect effectPost;
        RenderTarget2D renderTarget;

        public bool DisplayBlackAndWhite { get; set; }

        public PaintWithMeGame()
        {
            graphics = new GraphicsDeviceManager(this);

            //graphics.PreferredBackBufferWidth = 768;
            //graphics.PreferredBackBufferHeight = 1024;

            //graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            serviceManager.Add(this);
            serviceManager.Add(new ConfigurationManager());
            serviceManager.Add(new LogManager());
            serviceManager.Add(new RandomGenerator());
            serviceManager.Add(new TextureManager());
            serviceManager.Add(new CanvasBackground());

            inputDevices.Add(new KinectDevice());
            inputDevices.Add(new KeyboardDevice());
            //inputDevices.Add(new XboxDrumSetDevice());
            //inputDevices.Add(new SNESControllerDevice());
            inputDevices.Add(new ArduinoDevice());
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

                nextTimeToKillPaintingStrokes = DateTime.Now + TimeBetweenKillingPaintingStrokes;
            }
            catch (Exception e)
            {
                serviceManager.GetService<ILogger>(ServiceType.Logger).Log(e);
            }

            effectPost = Content.Load<Effect>("GrayscalePixelShader");
            DisplayBlackAndWhite = false;

            _canvasBackground = serviceManager.GetService<CanvasBackground>(ServiceType.CanvasBackground);

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

            DateTime currentTime = DateTime.Now;
            if (nextTimeToKillPaintingStrokes < currentTime)
            {
                nextTimeToKillPaintingStrokes += TimeBetweenKillingPaintingStrokes;
                paintingActions.RemoveAll(x => x.TimeToKill(currentTime) == true);
            }

            if (paintingActions.Count > MaxStrokeCount)
            {
                int strokesToRemove = paintingActions.Count - MaxStrokeCount;
                for (int i = 0; i < paintingActions.Count && strokesToRemove > 0; i++)
                {
                    if (paintingActions[i].CanExpire())
                    {
                        paintingActions.RemoveAt(i);
                        i--;
                        strokesToRemove--;
                    }
                }
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
            GraphicsDevice.Clear(_canvasBackground.GetBackgroundColor());

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            {
                // Apply the post process shader
                if (DisplayBlackAndWhite)
                {
                    effectPost.CurrentTechnique.Passes[0].Apply();
                }

                foreach (IPaintingAction paintingAction in paintingActions)
                {
                    paintingAction.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void RandomShiftPaintingStrokes()
        {
            foreach (IPaintingAction paintingAction in paintingActions)
            {
                paintingAction.RandomShift(StrokeShiftCoefficient);
            }
        }

        public void ClearAroundPoint(int x, int y)
        {
            for (int i = 0; i < paintingActions.Count; i++)
            {
                if (paintingActions[i].ClearIfLocatedInArea(x, y))
                {
                    paintingActions.RemoveAt(i);
                    --i;
                }
            }
        }

        public void RemoveFrame()
        {
            paintingActions.RemoveAll(x => x.GetType() == typeof(FramePaintingAction));
        }

        public void RemoveSmiley()
        {
            paintingActions.RemoveAll(x => x.GetType() == typeof(SmileyPaintingAction));
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
