using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SharpDX.DirectInput;

namespace PaintWithMe
{
    class XboxDrumSetDevice : JoystickDevice, IInputDevice
    {
        const string InstanceName = "Controller (Harmonix Drum Kit for Xbox 360)";

        void IInputDevice.Initialize()
        {
            base.Initialize(InstanceName);
        }

        bool IInputDevice.Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes)
        {
            newPaintStrokes = null;

            JoystickUpdate[] updates = base.Update(elapsedTime);
            if (updates == null) return false;

            foreach (JoystickUpdate state in updates)
            {
                Debug.WriteLine("XboxDrumSetDevice " + state.Offset.ToString() + " : " + state.Value.ToString());

                //Green Drum is hit
                if (state.Offset == JoystickOffset.Buttons0 && state.Value > 0)
                {
                    AddPaintingActionToList(new FireworkPaintingAction(FireworkPaintingAction.FireworkType.FireworkType4), ref newPaintStrokes);
                }

                //Red Drum is hit
                if (state.Offset == JoystickOffset.Buttons1 && state.Value > 0)
                {
                    AddPaintingActionToList(new FireworkPaintingAction(FireworkPaintingAction.FireworkType.FireworkType2), ref newPaintStrokes);
                }

                //Blue Drum is hit
                if (state.Offset == JoystickOffset.Buttons2 && state.Value > 0)
                {
                    AddPaintingActionToList(new FireworkPaintingAction(FireworkPaintingAction.FireworkType.FireworkType3), ref newPaintStrokes);
                }

                //Yellow Drum is hit
                if (state.Offset == JoystickOffset.Buttons3 && state.Value > 0)
                {
                    AddPaintingActionToList(new FireworkPaintingAction(FireworkPaintingAction.FireworkType.FireworkType1), ref newPaintStrokes);
                }
            }

            return (newPaintStrokes != null);
        }
    }
}
