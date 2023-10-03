using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Mononoke
{
    class MainMenu : IGameState
    {
        Texture2D Background;
        float backgroundScale = 0.5f;
        List<GUIButton> buttons;
        MainMenuController controller;
        Mononoke Game;
        public MainMenu( GraphicsDeviceManager graphics, Mononoke game)
        {
            Game = game;
            string str = "data/textures/gui/mainmenu/background.jpg";
            if (!File.Exists(str))
            {
                throw new Exception("Background does not exist at this path " + str);
            }
            Background = Texture2D.FromFile(graphics.GraphicsDevice, str);
            

            float scaleX = (float)Mononoke.RENDER_WIDTH / Background.Width ;
            float scaleY = (float)Mononoke.RENDER_HEIGHT / Background.Height ;
            backgroundScale = Mononoke.RENDER_WIDTH > Mononoke.RENDER_HEIGHT ? scaleX : scaleY;
            //backgroundScale = scaleX;

            str = "data/textures/gui/mainmenu/test.jpg";
            if (!File.Exists(str))
            {
                throw new Exception("test does not exist at this path " + str);
            }
            Texture2D test = Texture2D.FromFile(graphics.GraphicsDevice, str);

            buttons = new List<GUIButton>();
            buttons.Add(
                new GUIButton( 
                    new Rectangle( new Point( (int)Math.Floor( 0.1f * Mononoke.RENDER_WIDTH)
                 , (int)Math.Floor(0.9f * Mononoke.RENDER_HEIGHT)), new Point(250, 50) )
                 , test
                 , "New Game" 
                 , () => { game.NewGame(); }
                )
            );

            controller = new MainMenuController( this );
        }
        void IGameState.Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            _spriteBatch.Draw(Background, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), backgroundScale, SpriteEffects.None, 0f);
            foreach (GUIButton btn in buttons)
                btn.Draw( _spriteBatch);
            DrawCursor( _spriteBatch);
        }

        void IGameState.Update(GameTime _gameTime)
        {
            controller.Update( _gameTime);
        }
        public void ClickAt( Vector2 pos )
        {
            foreach( GUIButton btn in buttons )
            {
                if ( btn.TryClick(pos))
                { 
                    Debug.WriteLine("Clicked button: " + btn.Text);
                }
            }
        }
        public void DrawCursor(SpriteBatch spriteBatch)
        {
            Texture2D tex = TextureAssetManager.GetCursor(eMouseCursorType.Default);
            MouseState mstate = Mouse.GetState();
            Vector2 MousePos = mstate.Position.ToVector2();
            spriteBatch.Draw(tex, MousePos, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
    }
}
