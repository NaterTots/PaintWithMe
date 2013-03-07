using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PaintWithMe
{
    class KinectDevice : IInputDevice
    {

        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        public bool Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes)
        {
            //throw new NotImplementedException();
            newPaintStrokes = null;
            return false;
        }
    }
}
