using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SharpDX.DirectInput;

namespace PaintWithMe
{
    class SNESControllerDevice : JoystickDevice, IInputDevice
    {
        const string InstanceName = "Supe";

        /*
          New Shapes:
            B = Buttons7
            A = Buttons15
            Y = Buttons6
            X = Buttons14
          
          Move Current Shape:
            Right = Buttons0
            Left = Buttons1
            Up = Buttons3
            Down = Buttons2

          Smaller (L) / Bigger (R) Current Shape:
            LShoulder = Buttons13
            RShoulder = Buttons12
         */

        private ShapePaintingAction _currentShape;

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
                Debug.WriteLine("SNES Controller " + state.Offset.ToString() + " : " + state.Value.ToString());

                //B is pressed
                if (state.Offset == JoystickOffset.Buttons7 && state.Value > 0)
                {
                    _currentShape = new ShapePaintingAction(ShapePaintingAction.ShapeType.Square);
                    AddPaintingActionToList(_currentShape, ref newPaintStrokes);
                }

                //A is pressed
                if (state.Offset == JoystickOffset.Buttons15 && state.Value > 0)
                {
                    _currentShape = new ShapePaintingAction(ShapePaintingAction.ShapeType.Triangle);
                    AddPaintingActionToList(_currentShape, ref newPaintStrokes);
                }

                //Y is pressed
                if (state.Offset == JoystickOffset.Buttons6 && state.Value > 0)
                {
                    _currentShape = new ShapePaintingAction(ShapePaintingAction.ShapeType.Circle);
                    AddPaintingActionToList(_currentShape, ref newPaintStrokes);
                }

                //X is pressed
                if (state.Offset == JoystickOffset.Buttons14 && state.Value > 0)
                {
                    _currentShape = new ShapePaintingAction(ShapePaintingAction.ShapeType.Star);
                    AddPaintingActionToList(_currentShape, ref newPaintStrokes);
                }

                //Right is pressed
                if (state.Offset == JoystickOffset.Buttons0 && state.Value > 0)
                {
                    if (_currentShape != null)
                    {
                        _currentShape.MoveRight();
                    }
                }

                //Left is pressed
                if (state.Offset == JoystickOffset.Buttons1 && state.Value > 0)
                {
                    if (_currentShape != null)
                    {
                        _currentShape.MoveLeft();
                    }
                }

                //Up is pressed
                if (state.Offset == JoystickOffset.Buttons3 && state.Value > 0)
                {
                    if (_currentShape != null)
                    {
                        _currentShape.MoveUp();
                    }
                }

                //Down is pressed
                if (state.Offset == JoystickOffset.Buttons2 && state.Value > 0)
                {
                    if (_currentShape != null)
                    {
                        _currentShape.MoveDown();
                    }
                }

                //LShoulder is pressed
                if (state.Offset == JoystickOffset.Buttons13 && state.Value > 0)
                {
                    if (_currentShape != null)
                    {
                        _currentShape.DecreaseSize();
                    }
                }

                //RShoulder is pressed
                if (state.Offset == JoystickOffset.Buttons12 && state.Value > 0)
                {
                    if (_currentShape != null)
                    {
                        _currentShape.IncreaseSize();
                    }
                }
            }

            return (newPaintStrokes != null);
        }
    }
}
