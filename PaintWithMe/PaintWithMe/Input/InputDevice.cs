using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PaintWithMe
{
    public interface IInputDevice
	{
        void Initialize();

        bool Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes);
	}
}
