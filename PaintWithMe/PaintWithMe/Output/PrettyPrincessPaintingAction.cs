using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintWithMe
{
    class PrettyPrincessPaintingAction : IPaintingAction
    {
        static Texture2D heartTexture;

        protected struct Heart
        {
            public int xcoord;
            public int ycoord;
            public int size;
            public float rotation;
        };

        List<Heart> listHearts = new List<Heart>();

        static PrettyPrincessPaintingAction()
        {
            heartTexture = ServiceManager.Instance.GetService<TextureManager>(ServiceType.TextureManager).GetTexture("cartoon\\heart");
        }

        public PrettyPrincessPaintingAction()
        {
            //randomize location
            RandomGenerator randomGenerator = ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator);

            int heartCount = randomGenerator.Next(3, 10);

            int sourceXcoord = randomGenerator.NextXLocation();
            int sourceYcoord = randomGenerator.NextYLocation();

            for (int i = 0; i < heartCount; i++)
            {
                Heart heart = new Heart();

                heart.rotation = (float)randomGenerator.NextDouble(-.524, .524); //-30 degress to +30 degrees
                heart.xcoord = randomGenerator.Next(sourceXcoord - 25, sourceXcoord + 25);
                heart.ycoord = randomGenerator.Next(sourceYcoord - 25, sourceYcoord + 25);

                heart.size = (int)(heartTexture.Width * randomGenerator.NextDouble(.03, .10));
                listHearts.Add(heart);
            }
        }

        public void Update(GameTime elapsedTime)
        {
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Heart heart in listHearts)
            {
                spriteBatch.Draw(heartTexture, new Rectangle(heart.xcoord, heart.ycoord, heart.size, heart.size), null, Color.White, heart.rotation, new Vector2(0, 0), SpriteEffects.None, 0); 
            }
        }
    }
}
