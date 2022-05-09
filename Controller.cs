using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    class Controller
    {
        Mononoke Game;
        Camera2D Camera;
        bool mousedown = false;
        public Controller( Camera2D camera, Mononoke game ) 
        {
            Game = game;
            Camera = camera;
        }
        public void Update( GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Quit();

            // TODO: Add your update logic here
            //Camera.Move ( new Vector2( -1,-1));
            KeyboardState kstate = Keyboard.GetState();
            Vector2 camTranslate = new Vector2(0,0);

            if (kstate.IsKeyDown(Keys.W))
                camTranslate.Y = 1f;// * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kstate.IsKeyDown(Keys.S))
                camTranslate.Y = -1f;// * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.A))
                camTranslate.X = 1f;// * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kstate.IsKeyDown(Keys.D))
                camTranslate.X = -1f;// * (float)gameTime.ElapsedGameTime.TotalSeconds;

            float sprint = 1f;
            if (kstate.IsKeyDown( Keys.LeftShift ) )
            {
                sprint = 4f;
            }
            if (kstate.IsKeyDown(Keys.Escape))
                Game.Quit();
            //Vector2 mpos = ScreenPosToMapPos();
            //Debug.WriteLine("Mouse pos " +  mpos);
            MouseState mstate = Mouse.GetState();
            if ( !mousedown && mstate.LeftButton == ButtonState.Pressed )
            {
                mousedown = true;
                Game.ClickAt( ScreenPosToMapPos() );
            }
            else if (mstate.LeftButton == ButtonState.Released )
            {
                mousedown = false;
            }
            Camera.Move( sprint * 100f * camTranslate );
        }
        public Vector2 ScreenPosToMapPos()
        {
            MouseState mstate = Mouse.GetState();
            Vector2 result = ( mstate.Position.ToVector2() - Camera.Position );
            result /= MapHolder.PIXELS_PER_TILE;
            return Vector2.Floor(result);
        }
    }
}
