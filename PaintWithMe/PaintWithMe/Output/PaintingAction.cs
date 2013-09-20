using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PaintWithMe
{
    public interface IPaintingAction
    {
        void Update(GameTime elapsedTime);
        void Draw(SpriteBatch spriteBatch);

        bool TimeToKill(DateTime currentDateTime);
        bool CanExpire();
        void RandomShift(int shiftAmount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>If true, then delete the entire painting action</returns>
        bool ClearIfLocatedInArea(int x, int y);
    }
}
