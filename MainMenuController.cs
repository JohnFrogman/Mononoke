using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mononoke
{
    class MainMenuController
    {
        bool mousedown = false;
        MainMenu menu;
        public MainMenuController( MainMenu m)
        {
            menu = m;
        }
        public void Update(GameTime gameTime)
        {

            MouseState mstate = Mouse.GetState(); 
            if (!mousedown && mstate.LeftButton == ButtonState.Pressed)
            {
                mousedown = true;
            }
            else if (mousedown && mstate.LeftButton == ButtonState.Released)
            {
                // Debug.WriteLine("Mouse Release");
                menu.ClickAt(mstate.Position.ToVector2());
                mousedown = false;
            }
        }
    }
}
