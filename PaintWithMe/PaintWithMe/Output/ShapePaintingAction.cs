using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintWithMe
{
    class ShapePaintingAction : IPaintingAction
    {
        const int MovementVector = 20;
        const float ResizeVector = .02f;

        static Texture2D squareStrokeTexture;
        static Texture2D triangleStrokeTexture;
        static Texture2D circleStrokeTexture;
        static Texture2D starStrokeTexture;

        DateTime _deathDate;
        static TimeSpan LifeSpan = new TimeSpan(0, 3, 0); //3 minutes

        private ShapeType type;
        private int xcoord;
        private int ycoord;
        private int height, width;
        private float rotation;
        private Color color;

        //this is kept in order to recalculate size when increasing and decreasing size
        private double sizeMultiplier;

        public enum ShapeType
        {
            Square,
            Triangle,
            Circle,
            Star
        };

        static ShapePaintingAction()
        {
            squareStrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\square");
            triangleStrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\triangle");
            circleStrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\circle");
            starStrokeTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\star");
        }

        public ShapePaintingAction(ShapeType fireworkType)
        {
            type = fireworkType;

            //randomize location
            RandomGenerator randomGenerator = ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator);

            //rotation = (float)randomGenerator.NextDouble(-.174, .174); //-10 degress to +10 degrees
            rotation = (float)randomGenerator.NextDouble(-.524, .524); //-30 degress to +30 degrees
            xcoord = randomGenerator.NextXLocation();
            ycoord = randomGenerator.NextYLocation();

            sizeMultiplier = randomGenerator.NextDouble(.05, .15); //shape texture is large.  random size 25% to 50% of texture
            width = (int)(squareStrokeTexture.Width * sizeMultiplier);
            height = (int)(squareStrokeTexture.Height * sizeMultiplier);

            color = new Color(randomGenerator.Next(255), randomGenerator.Next(255), randomGenerator.Next(255), 200);

            _deathDate = DateTime.Now + LifeSpan;
        }

        public void Update(GameTime elapsedTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D shapeStrokeTexture = null;

            switch (type)
            {
                case ShapeType.Square:
                    shapeStrokeTexture = squareStrokeTexture;
                    break;
                case ShapeType.Triangle:
                    shapeStrokeTexture = triangleStrokeTexture;
                    break;
                case ShapeType.Circle:
                    shapeStrokeTexture = circleStrokeTexture;
                    break;
                case ShapeType.Star:
                    shapeStrokeTexture = starStrokeTexture;
                    break;
            }

            spriteBatch.Draw(shapeStrokeTexture, new Rectangle(xcoord, ycoord, width, height), null, color, rotation, new Vector2(0, 0), SpriteEffects.None, 0); 
        }

        public void MoveLeft()
        {
            xcoord -= MovementVector;
        }

        public void MoveRight()
        {
            xcoord += MovementVector;
        }

        public void MoveUp()
        {
            ycoord -= MovementVector;
        }

        public void MoveDown()
        {
            ycoord += MovementVector;
        }

        public void IncreaseSize()
        {
            sizeMultiplier += ResizeVector;
            if (sizeMultiplier > .3) sizeMultiplier = .3;

            width = (int)(squareStrokeTexture.Width * sizeMultiplier);
            height = (int)(squareStrokeTexture.Height * sizeMultiplier);
        }

        public void DecreaseSize()
        {
            sizeMultiplier -= ResizeVector;
            if (sizeMultiplier < ResizeVector) sizeMultiplier = ResizeVector;

            width = (int)(squareStrokeTexture.Width * sizeMultiplier);
            height = (int)(squareStrokeTexture.Height * sizeMultiplier);
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
