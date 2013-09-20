using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintWithMe
{
    class SmileyPaintingAction : IPaintingAction
    {
        static Texture2D smileyTexture;

        static int screenHeight;
        static int screenWidth;

        static SmileyPaintingAction()
        {
            smileyTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\smiley");

            PaintWithMeGame game = ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game);

            screenHeight = game.GraphicsDevice.DisplayMode.Height;
            screenWidth = game.GraphicsDevice.DisplayMode.Width;
        }

        public void Update(GameTime elapsedTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(smileyTexture, new Rectangle(0, 0, screenHeight, screenWidth), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, -1);
        }

        public bool TimeToKill(DateTime currentDateTime)
        {
            //the frame is killed by an action, not by time
            return false;
        }

        public bool CanExpire()
        {
            return false;
        }

        public void RandomShift(int shiftAmount)
        {
            //don't do anything
        }

        public bool ClearIfLocatedInArea(int x, int y)
        {
            //don't do anything
            return false;
        }
    }
}
