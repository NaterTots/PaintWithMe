using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PaintWithMe
{
    class BodyMovementPaintingAction : IPaintingAction
    {
        static Texture2D spiralTexture;
        static Texture2D grassTexture;
        static Texture2D jump1Texture;
        static Texture2D jump2Texture;

        private BodyMovementType type;
        private int xcoord;
        private int ycoord;
        private float rotation;
        private int height;
        private int width;

        public enum BodyMovementType
        {
            HipShake,
            LeftKick,
            RightKick,
            JumpStyle1,
            JumpStyle2
        };

        static BodyMovementPaintingAction()
        {
            spiralTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\Spiral");
            grassTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\grass");
            jump1Texture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\quickstroke_upward");
            jump2Texture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\quickstroke_upward2");
        }

        public BodyMovementPaintingAction(BodyMovementType bodyMovementType)
        {
            type = bodyMovementType;

            //randomize location
            RandomGenerator randomGenerator = ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator);

            rotation = (float)randomGenerator.NextDouble(-.261, .261); //-15 degress to +15 degrees
            xcoord = randomGenerator.NextXLocation();

            double sizeMultiplier = 1;

            switch (type)
            {
                case BodyMovementType.HipShake:
                    ycoord = randomGenerator.NextYLocation(33, 66);
                    sizeMultiplier = randomGenerator.NextDouble(.05, .10); //texture is large.  random size 5% to 10% of texture
                    break;
                case BodyMovementType.LeftKick:
                case BodyMovementType.RightKick:
                    ycoord = randomGenerator.NextYLocation(50,100);
                    break;
                case BodyMovementType.JumpStyle1:
                case BodyMovementType.JumpStyle2:
                    ycoord = randomGenerator.NextYLocation(0, 40);
                    sizeMultiplier = randomGenerator.NextDouble(.15, .3);
                    break;
                default:
                    ycoord = randomGenerator.NextYLocation();
                    break;
            }

            
            Texture2D texture = GetTexture(type);
            width = (int)(texture.Width * sizeMultiplier);
            height = (int)(texture.Height * sizeMultiplier);
        }

        public void Update(GameTime elapsedTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GetTexture(type), new Rectangle(xcoord, ycoord, width, height), null, Color.White, rotation, new Vector2(0, 0), GetEffects(type), 0); 
        }

        Texture2D GetTexture(BodyMovementType bodyMovementType)
        {
            Texture2D texture = null;

            switch (type)
            {
                case BodyMovementType.HipShake:
                    texture = spiralTexture;
                    break;
                case BodyMovementType.LeftKick:
                    texture = grassTexture;
                    break;
                case BodyMovementType.RightKick:
                    texture = grassTexture;
                    break;
                case BodyMovementType.JumpStyle1:
                    texture = jump1Texture;
                    break;
                case BodyMovementType.JumpStyle2:
                    texture = jump2Texture;
                    break;
            }

            return texture;
        }

        SpriteEffects GetEffects(BodyMovementType bodyMovementType)
        {
            SpriteEffects effects = SpriteEffects.None;

            if (bodyMovementType == BodyMovementType.LeftKick)
            {
                effects = SpriteEffects.FlipHorizontally;    
            }
            else if (bodyMovementType == BodyMovementType.JumpStyle1 || bodyMovementType == BodyMovementType.JumpStyle2)
            {
                effects = SpriteEffects.FlipVertically;
            }

            return effects;
        }
    }
}
