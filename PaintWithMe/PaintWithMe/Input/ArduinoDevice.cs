using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using SharpDX.DirectInput;

namespace PaintWithMe
{
    class ArduinoDevice : JoystickDevice, IInputDevice
    {
        const string InstanceName = "TODO";

        CanvasBackground canvasBackground;

        PrettyPrincessPaintingAction _lastPrettyPrincessAction;

        public void Initialize()
        {
            base.Initialize(InstanceName);

            canvasBackground = ServiceManager.Instance.GetService<CanvasBackground>(ServiceType.CanvasBackground);
        }

        public bool Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes)
        {
            newPaintStrokes = null;

            foreach (JoystickUpdate state in base.Update(elapsedTime))
            {
                Debug.WriteLine("ArduinoDevice " + state.Offset.ToString() + " : " + state.Value.ToString());

                //TODO: verify the buttons

                //Red switch is changed
                if (state.Offset == JoystickOffset.Buttons0)
                {
                    canvasBackground.ActivateRed(state.Value > 0);
                }

                //Green switch is changed
                if (state.Offset == JoystickOffset.Buttons0)
                {
                    canvasBackground.ActivateGreen(state.Value > 0);
                }

                //Blue switch is changed
                if (state.Offset == JoystickOffset.Buttons0)
                {
                    canvasBackground.ActivateBlue(state.Value > 0);
                }

                //Pretty princess button pressed
                if (state.Offset == JoystickOffset.Buttons0)
                {
                    if (state.Value > 0)
                    {
                        _lastPrettyPrincessAction = new PrettyPrincessPaintingAction();
                        AddPaintingActionToList(_lastPrettyPrincessAction, ref newPaintStrokes);
                    }
                    else if (_lastPrettyPrincessAction != null)
                    {
                        _lastPrettyPrincessAction.Deactivate();
                    }
                }

                //Smiley button
                //TODO: ??

                //Doorbell
                //TODO: ??

                //Frame button
                //TODO

                //Squeeze giggles
                //TODO: ??

                //Breaker switch
                if (state.Offset == JoystickOffset.Buttons0)
                {
                    ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game).DisplayBlackAndWhite = (state.Value > 0);
                }

            }

            return (newPaintStrokes != null);
        }
    }
}
