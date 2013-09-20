using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SharpDX.DirectInput;

namespace PaintWithMe
{
    class JoystickDevice
    {
        Joystick joystick = null;

        public bool Initialize(string instanceName)
        {
            bool bFoundDevice = false;

            // Initialize DirectInput
            DirectInput directInput = new DirectInput();

            // Find a Joystick Guid
            Guid joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                if (deviceInstance.InstanceName == instanceName)
                {
                    //found the device
                    joystickGuid = deviceInstance.InstanceGuid;
                }
            }

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
            {
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    if (deviceInstance.InstanceName == instanceName)
                    {
                        joystickGuid = deviceInstance.InstanceGuid;
                    }
                }
            }

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found: " + instanceName);
            }
            else
            {
                bFoundDevice = true;

                // Instantiate the joystick
                joystick = new Joystick(directInput, joystickGuid);

                Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

                // Query all suported ForceFeedback effects
                var allEffects = joystick.GetEffects();
                foreach (var effectInfo in allEffects)
                {
                    Console.WriteLine("Effect available {0}", effectInfo.Name);
                }

                // Set BufferSize in order to use buffered data.
                joystick.Properties.BufferSize = 128;

                // Acquire the joystick
                joystick.Acquire();
            }

            return bFoundDevice;
        }

        public JoystickUpdate[] Update(GameTime elapsedTime)
        {
            try
            {
                if (joystick != null)
                {
                    joystick.Poll();
                    JoystickUpdate[] datas = joystick.GetBufferedData();
                    return datas;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        protected void AddPaintingActionToList(IPaintingAction paintingAction, ref List<IPaintingAction> paintingList)
        {
            if (paintingList == null)
            {
                paintingList = new List<IPaintingAction>();
            }

            paintingList.Add(paintingAction);
        }
    }
}