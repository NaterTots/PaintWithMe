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
    }
}
