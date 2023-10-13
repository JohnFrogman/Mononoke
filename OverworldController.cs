using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
namespace Mononoke
{
    enum eOverworldControlState
    {
        Default
        ,SelectedUnit
        ,DraggingMapDraggable
        ,DragSelect
    }
    class OverworldController
    {
        Mononoke Game;
        Camera2D Camera;
        float PressTime = 0f;
        Overworld mOverworld;

        bool SavePressed; 
        public OverworldController( Overworld overworld, Camera2D camera, GraphicsDeviceManager graphics, Mononoke game ) 
        {
            mOverworld = overworld;
            Game = game;
            Camera = camera;
        }
        public void Update( GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Quit();

            KeyboardState kstate = Keyboard.GetState();


        }
       

    }
}
