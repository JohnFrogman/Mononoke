using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    enum eInputType
    {
        Up, Down, Left, Right,
        Interact,
        Accelerate, Brake, TurnLeft, TurnRight, Radio,
        SwitchInventoryForward, SwitchInventoryBack, InventoryLeft, InventoryRight, InventoryUp, InventoryDown, OpenPlayerInventory, InventorySelect

    }
    internal class InputManager
    {
        KeyboardState mCurrentKeyboardState;
        KeyboardState mPreviousKeyboardState;
        Dictionary<eInputType, Keys> mKeyboardBindings;
        Dictionary<eInputType, Buttons> mGamepadBindings;
        public InputManager()
        {
            mKeyboardBindings = new();
            mGamepadBindings = new();
            DefaultBindings();
        }
        public void Update(GameTime gameTime)
        {
           // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
         //       Game.Quit();
            mPreviousKeyboardState = mCurrentKeyboardState;
            mCurrentKeyboardState = Keyboard.GetState();
        }
        public bool ButtonReleased(eInputType type)
        {
            //state.IsKeyUp(Keys.E)
            //state.IsKeyDown(Keys.E)
            if (mPreviousKeyboardState.IsKeyDown(mKeyboardBindings[type]) && mCurrentKeyboardState.IsKeyUp(mKeyboardBindings[type]))
            { 
                return true;
            }
            return false;
        }
        public bool ButtonPressed(eInputType type)
        {
            if (mCurrentKeyboardState.IsKeyDown(mKeyboardBindings[type]) && mPreviousKeyboardState.IsKeyUp(mKeyboardBindings[type]))
            {
                return true;
            }
            return false;
        }
        public bool ButtonUp(eInputType type)
        { 
            return (mCurrentKeyboardState.IsKeyUp(mKeyboardBindings[type]));
        }
        public bool ButtonDown(eInputType type)
        {
            return (mCurrentKeyboardState.IsKeyDown(mKeyboardBindings[type]));
        }
        void DefaultBindings()
        {
            // Up, Down, Left, Right,
            // Interact,
            // Accelerate, Brake, TurnLeft, TurnRight,
            mKeyboardBindings.Clear();
            mKeyboardBindings.Add(eInputType.Up, Keys.W);
            mKeyboardBindings.Add(eInputType.Down, Keys.S);
            mKeyboardBindings.Add(eInputType.Left, Keys.A);
            mKeyboardBindings.Add(eInputType.Right, Keys.D);

            mKeyboardBindings.Add(eInputType.Interact, Keys.E);

            mKeyboardBindings.Add(eInputType.Accelerate, Keys.W);
            mKeyboardBindings.Add(eInputType.Brake, Keys.S);
            mKeyboardBindings.Add(eInputType.TurnLeft, Keys.A);
            mKeyboardBindings.Add(eInputType.TurnRight, Keys.D);
            mKeyboardBindings.Add(eInputType.Radio, Keys.R);

            mKeyboardBindings.Add(eInputType.SwitchInventoryForward, Keys.O);
            mKeyboardBindings.Add(eInputType.SwitchInventoryBack, Keys.U);
            mKeyboardBindings.Add(eInputType.InventoryUp, Keys.K);
            mKeyboardBindings.Add(eInputType.InventoryDown, Keys.I);
            mKeyboardBindings.Add(eInputType.InventoryLeft, Keys.J);
            mKeyboardBindings.Add(eInputType.InventoryRight, Keys.L);
            mKeyboardBindings.Add(eInputType.OpenPlayerInventory, Keys.B);
            mKeyboardBindings.Add(eInputType.InventorySelect, Keys.Space);

            //mGamepadBindings.Clear();
            //mGamepadBindings.Add(eInputType.Up, Keys.W);
            //mGamepadBindings.Add(eInputType.Down, Keys.S);
            //mGamepadBindings.Add(eInputType.Left, Keys.A);
            //mGamepadBindings.Add(eInputType.Right, Keys.D);
             
            //mGamepadBindings.Add(eInputType.Interact, Keys.E);

            //mGamepadBindings.Add(eInputType.Accelerate, Keys.W);
            //mGamepadBindings.Add(eInputType.Brake, Keys.S);
            //mGamepadBindings.Add(eInputType.TurnLeft, Keys.A);
            //mGamepadBindings.Add(eInputType.TurnRight, Keys.D);
            //mGamepadBindings.Add(eInputType.Radio, Keys.R);

            //mGamepadBindings.Add(eInputType.SwitchInventoryForward, Keys.O);
            //mGamepadBindings.Add(eInputType.SwitchInventoryBack, Keys.U);
            //mGamepadBindings.Add(eInputType.InventoryUp, Keys.K);
            //mGamepadBindings.Add(eInputType.InventoryDown, Keys.I);
            //mGamepadBindings.Add(eInputType.InventoryLeft, Keys.J);
            //mGamepadBindings.Add(eInputType.InventoryRight, Keys.L);
            //mGamepadBindings.Add(eInputType.OpenPlayerInventory, Keys.B);
            //mGamepadBindings.Add(eInputType.InventorySelect, Keys.Space);
        }
    }
}
