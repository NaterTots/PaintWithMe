using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PaintWithMe
{
    class KeyboardDevice : IInputDevice
    {
        KeyboardState previousState;

        public void Initialize()
        {
            previousState = Keyboard.GetState();
        }

        public bool Update(GameTime elapsedTime, out List<IPaintingAction> newPaintStrokes)
        {
            newPaintStrokes = null;
            KeyboardState currentState = Keyboard.GetState();

            if (HasButtonBeenPressed(previousState, currentState, Keys.Space))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KeyboardSpaceBar), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.J))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KinectJump), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.R))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KinectRightArmUp), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.L))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KinectLeftArmUp), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.A))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KeyboardTest1), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.S))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KeyboardTest2), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.D))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KeyboardTest3), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.F))
            {
                AddPaintingActionToList(new TestPaintingAction(ShapeActivationSource.KeyboardTest4), ref newPaintStrokes);
            }

            previousState = currentState;
            return (newPaintStrokes != null);
        }

        private bool HasButtonBeenPressed(KeyboardState oldState, KeyboardState newState, Keys key)
        {
            return (newState.IsKeyDown(key) && !oldState.IsKeyDown(key));
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
