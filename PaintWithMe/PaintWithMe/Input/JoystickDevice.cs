using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SharpDX.DirectInput;

namespace PaintWithMe
{
    class JoystickDevice : IInputDevice
    {
        Joystick joystick = null;

        //TODO: better way to do this
        int previousRotationZValue = 0;

        public void Initialize()
        {
            // Initialize DirectInput
            DirectInput directInput = new DirectInput();

            // Find a Joystick Guid
            Guid joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                joystickGuid = deviceInstance.InstanceGuid;
            }

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
            {
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                }
            }

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Instantiate the joystick
            joystick = new Joystick(directInput, joystickGuid);

            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            joystick.Acquire();
        }
        /*
        public bool Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes)
        {
            newPaintStrokes = null;

            if (joystick != null)
            {
                joystick.Poll();
                JoystickUpdate[] datas = joystick.GetBufferedData();
                foreach (JoystickUpdate state in datas)
                {
                    Debug.WriteLine(string.Format("{0}: {1}", state.Offset, state.Value));
                }
            }
            return false;
        }
        */
        
        public bool Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes)
        {
            newPaintStrokes = null;

            if (joystick != null)
            {
                joystick.Poll();
                JoystickUpdate[] datas = joystick.GetBufferedData();
                foreach (JoystickUpdate state in datas)
                {
                    if (state.Offset == JoystickOffset.Buttons3 && state.Value > 0)
                    {
                        AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.JoystickButtons3), ref newPaintStrokes);
                    }

                    if (state.Offset == JoystickOffset.RotationZ)
                    {
                        Debug.WriteLine(string.Format("RotationZ: {0}", state.Value));

                        //Rotation goes from 6k -> 64k (with 50k potentiometer)
                        if (previousRotationZValue == 0)
                        {
                            //initialize the value
                            previousRotationZValue = state.Value;
                            continue;
                        }
                        else if (Math.Abs(previousRotationZValue - state.Value) > 500)  //TODO: for now, the 500 is arbitrary to prevent the 'flicker' of resistance values
                        {
                            previousRotationZValue = state.Value;
                        }
                    }
                }
            }

            return (newPaintStrokes != null);
        }

        private void AddPaintingActionToList(IPaintingAction paintingAction, ref List<IPaintingAction> paintingList)
        {
            if (paintingList == null)
            {
                paintingList = new List<IPaintingAction>();
            }

            paintingList.Add(paintingAction);
        }
    }
}