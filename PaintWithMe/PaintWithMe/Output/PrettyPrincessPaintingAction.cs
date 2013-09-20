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

        static TimeSpan LifeSpan = new TimeSpan(0, 15, 0); //15 minutes
        static TimeSpan TimeBetweenHearts = new TimeSpan(0, 0, 2); //1 second
        const int MaxClusters = 30;
        int totalHeartClusters = 0;
        DateTime nextHeartCluster;
        DateTime _deathDate;

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
            _deathDate = DateTime.Now + LifeSpan;
            AddHeartCluster();
        }

        private void AddHeartCluster()
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
            totalHeartClusters++;
            nextHeartCluster = DateTime.Now + TimeBetweenHearts;
        }

        public void Update(GameTime elapsedTime)
        {
            if (totalHeartClusters < MaxClusters && DateTime.Now > nextHeartCluster)
            {
                AddHeartCluster();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Heart heart in listHearts)
            {
                spriteBatch.Draw(heartTexture, new Rectangle(heart.xcoord, heart.ycoord, heart.size, heart.size), null, Color.White, heart.rotation, new Vector2(0, 0), SpriteEffects.None, 0);
            }
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
            for (int i = 0; i < listHearts.Count; i++)
            {
                Heart h = listHearts[i];
                h.xcoord = h.xcoord + randomGenerator.Next(shiftAmount) - shiftAmount / 2;
                h.ycoord = h.ycoord + randomGenerator.Next(shiftAmount) - shiftAmount / 2;
            }
        }

        public bool ClearIfLocatedInArea(int x, int y)
        {
            listHearts.RemoveAll(h =>
                    x > h.xcoord &&
                    x < h.xcoord + h.size &&
                    y > h.ycoord &&
                    y < h.xcoord + h.size);
            //only delete the stroke if there are no more hearts
            return (listHearts.Count == 0);
        }

        public void Deactivate()
        {
            totalHeartClusters = MaxClusters;
        }
    }
}
