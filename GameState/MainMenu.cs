using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

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
            //_spriteBatch.Draw( Background, new Rectangle( new Point(0,0), new Point(Mononoke.RENDER_WIDTH, Mononoke.RENDER_WIDTH) ), Color.White );

            foreach (GUIButton btn in buttons)
                btn.Draw( _spriteBatch);
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
                    Debug.WriteLine("Clicked button: " + btn.text);
                }
            }
        }

    }
}
