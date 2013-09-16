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
                if (ServiceManager.Instance.GetService<RandomGenerator>(ServiceType.RandomGenerator).NextBool())
                {
                    AddPaintingActionToList(new BodyMovementPaintingAction(BodyMovementPaintingAction.BodyMovementType.JumpStyle1), ref newPaintStrokes);
                }
                else
                {
                    AddPaintingActionToList(new BodyMovementPaintingAction(BodyMovementPaintingAction.BodyMovementType.JumpStyle2), ref newPaintStrokes);
                }
                
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.R))
            {
                AddPaintingActionToList(new BodyMovementPaintingAction(BodyMovementPaintingAction.BodyMovementType.RightKick), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.L))
            {
                AddPaintingActionToList(new BodyMovementPaintingAction(BodyMovementPaintingAction.BodyMovementType.LeftKick), ref newPaintStrokes);
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

            if (HasButtonBeenPressed(previousState, currentState, Keys.Q))
            {
                AddPaintingActionToList(new FireworkPaintingAction(FireworkPaintingAction.FireworkType.FireworkType1), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.W))
            {
                AddPaintingActionToList(new FireworkPaintingAction(FireworkPaintingAction.FireworkType.FireworkType2), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.E))
            {
                AddPaintingActionToList(new FireworkPaintingAction(FireworkPaintingAction.FireworkType.FireworkType3), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.P))
            {
                AddPaintingActionToList(new PrettyPrincessPaintingAction(), ref newPaintStrokes);
            }

            if (HasButtonBeenPressed(previousState, currentState, Keys.H))
            {
                AddPaintingActionToList(new BodyMovementPaintingAction(BodyMovementPaintingAction.BodyMovementType.HipShake), ref newPaintStrokes);
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
