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
        Accelerate, Brake, TurnLeft, TurnRight, Radio
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
        }
    }
}
