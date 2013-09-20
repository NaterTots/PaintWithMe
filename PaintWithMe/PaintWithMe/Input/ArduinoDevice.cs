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
        const string InstanceName = "UnoJoy Joystick";

        CanvasBackground canvasBackground;

        PrettyPrincessPaintingAction _lastPrettyPrincessAction;
        DateTime _lastGiggle;
        static TimeSpan MinTimeBetweenGiggles = new TimeSpan(0, 0, 4);

        public void Initialize()
        {
            base.Initialize(InstanceName);

            canvasBackground = ServiceManager.Instance.GetService<CanvasBackground>(ServiceType.CanvasBackground);
            _lastGiggle = DateTime.Now;
        }

        public bool Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes)
        {
            newPaintStrokes = null;

            JoystickUpdate[] updates = base.Update(elapsedTime);
            if (updates == null) return false;

            foreach (JoystickUpdate state in updates)
            {
                //the analog ports are hanging so they fluctuate a lot.  skip 'em
                if (state.Offset == JoystickOffset.X ||
                    state.Offset == JoystickOffset.Y ||
                    state.Offset == JoystickOffset.Z ||
                    state.Offset == JoystickOffset.RotationZ)
                {
                    continue;
                }

                Debug.WriteLine("ArduinoDevice " + state.Offset.ToString() + " : " + state.Value.ToString());

                //Red switch is changed -> D2
                if (state.Offset == JoystickOffset.Buttons3)
                {
                    canvasBackground.ActivateRed(state.Value > 0);
                }

                //Green switch is changed -> D3
                if (state.Offset == JoystickOffset.Buttons2)
                {
                    canvasBackground.ActivateGreen(state.Value > 0);
                }

                //Blue switch is changed -> D4
                if (state.Offset == JoystickOffset.Buttons0)
                {
                    canvasBackground.ActivateBlue(state.Value > 0);
                }

                //Pretty princess button pressed -> A4
                if (state.Offset == JoystickOffset.Buttons9)
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

                //Frame button -> A5
                if (state.Offset == JoystickOffset.Buttons12)
                {
                    if (state.Value > 0) //button is pushed - create the frame
                    {
                        AddPaintingActionToList(new FramePaintingAction(), ref newPaintStrokes);
                    }
                    else //button is released - remove all frames
                    {
                        ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game).RemoveFrame();
                    }
                }

                //Smiley button -> D5
                //Buttons1
                if (state.Offset == JoystickOffset.Buttons1)
                {
                    if (state.Value > 0) //button is pushed - create the smiley
                    {
                        AddPaintingActionToList(new SmileyPaintingAction(), ref newPaintStrokes);
                    }
                    else //button is released - remove all frames
                    {
                        ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game).RemoveFrame();
                    }
                }

                //Squeeze giggles -> D11
                if (state.Offset == JoystickOffset.Buttons5 && state.Value > 0)
                {
                    if (_lastGiggle + MinTimeBetweenGiggles < DateTime.Now)
                    {
                        _lastGiggle = DateTime.Now;
                        ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game).RandomShiftPaintingStrokes();
                    }
                }

                //Doorbell -> D10
                if (state.Offset == JoystickOffset.Buttons4 && state.Value > 0)
                {
                    RandomGenerator randomGenerator = ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator);
                    ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game).ClearAroundPoint(randomGenerator.NextXLocation(), randomGenerator.NextYLocation());
                }

                //Breaker switch -> D12
                if (state.Offset == JoystickOffset.Buttons0)
                {
                    ServiceManager.Instance.GetService<PaintWithMeGame>(ServiceType.Game).DisplayBlackAndWhite = (state.Value > 0);
                }
            }

            return (newPaintStrokes != null);
        }
    }
}
