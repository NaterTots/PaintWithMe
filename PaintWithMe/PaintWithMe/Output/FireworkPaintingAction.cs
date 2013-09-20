using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintWithMe
{
    class FireworkPaintingAction : IPaintingAction
    {
        static Texture2D fireworkType1StrokeTexture;
        static Texture2D fireworkType2StrokeTexture;
        static Texture2D fireworkType3StrokeTexture;
        static Texture2D fireworkType4StrokeTexture;

        DateTime _deathDate;
        static TimeSpan LifeSpan = new TimeSpan(0, 3, 0); //3 minutes

        private FireworkType type;
        private int xcoord;
        private int ycoord;
        private int height, width;
        private float rotation;

        public enum FireworkType
        {
            FireworkType1,
            FireworkType2,
            FireworkType3,
            FireworkType4
        };

        static FireworkPaintingAction()
        {
            fireworkType1StrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\fireworks1");
            fireworkType2StrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\fireworks2");
            fireworkType3StrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\fireworks3");
            fireworkType4StrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\fireworks4");
        }

        public FireworkPaintingAction(FireworkType fireworkType)
        {
            type = fireworkType;

            //randomize location
            RandomGenerator randomGenerator = ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator);

            //rotation = (float)randomGenerator.NextDouble(-.174, .174); //-10 degress to +10 degrees
            rotation = (float)randomGenerator.NextDouble(-.524, .524); //-30 degress to +30 degrees
            xcoord = randomGenerator.NextXLocation();
            ycoord = randomGenerator.NextYLocation();

            double sizeMultiplier = randomGenerator.NextDouble(.05, .10); //firework texture is large.  random size 5% to 10% of texture
            width = (int)(fireworkType1StrokeTexture.Width * sizeMultiplier);
            height = (int)(fireworkType1StrokeTexture.Height * sizeMultiplier);

            _deathDate = DateTime.Now + LifeSpan;
        }

        public void Update(GameTime elapsedTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D fireworkStrokeTexture = null;

            switch (type)
            {
                case FireworkType.FireworkType1:
                    fireworkStrokeTexture = fireworkType1StrokeTexture;
                    break;
                case FireworkType.FireworkType2:
                    fireworkStrokeTexture = fireworkType2StrokeTexture;
                    break;
                case FireworkType.FireworkType3:
                    fireworkStrokeTexture = fireworkType3StrokeTexture;
                    break;
                case FireworkType.FireworkType4:
                    fireworkStrokeTexture = fireworkType4StrokeTexture;
                    break;
            }

            spriteBatch.Draw(fireworkStrokeTexture, new Rectangle(xcoord, ycoord, width, height), null, Color.White, rotation, new Vector2(0, 0), SpriteEffects.None, 0); 
        }

        public bool TimeToKill(DateTime currentDateTime)
        {
            return (currentDateTime > _deathDate);
        }

        public bool CanExpire()
        {
            return true;
        }

        public void RandomShift(int shiftAmount)
        {
            //randomize location
            RandomGenerator randomGenerator = ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator);
            xcoord = xcoord + randomGenerator.Next(shiftAmount * 2) - shiftAmount;
            ycoord = ycoord + randomGenerator.Next(shiftAmount * 2) - shiftAmount;
        }

        public bool ClearIfLocatedInArea(int x, int y)
        {
            bool bOverlap = false;
            if (x > xcoord && x < xcoord + width && y > ycoord && y < ycoord + height)
            {
                bOverlap = true;
            }
            return bOverlap;
        }
    }
}
