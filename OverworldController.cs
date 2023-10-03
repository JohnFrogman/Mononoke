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
        bool mousedown = false;

        float PressTime = 0f;

        Overworld mOverworld;
        // Note, for unit movement we want to drag to draw an exact path, tap to select, tap again to move will do proper pathfinding

        Vector2 HoveredPos;
        Vector2 HoveredTile;
        Vector2 ClickedTile;
        Vector2 PathPreviewOriginTile;
        Vector2 PathPreviewTile;

        List<Vector2> PathPreview;

        RadialMenu ActiveRadial;

        bool SavePressed; 
        public OverworldController( Overworld overworld, Camera2D camera, GraphicsDeviceManager graphics, Mononoke game ) 
        {
            mOverworld = overworld;
            Game = game;
            Camera = camera;
            PathPreview = new List<Vector2>();
        }
        public void Update( GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Quit();

            // TODO: Add your update logic here
            //Camera.Move ( new Vector2( -1,-1));
            KeyboardState kstate = Keyboard.GetState();
            Vector2 camTranslate = new Vector2(0,0);
            
            if (!SavePressed && kstate.IsKeyDown(Keys.S) && kstate.IsKeyDown(Keys.LeftControl) )
            { 
                mOverworld.Save();
                SavePressed = true;
                //Controller.Save();
            }
            if (kstate.IsKeyUp(Keys.S) && kstate.IsKeyUp(Keys.LeftControl))
            {
                //mOverworld.Load();
                SavePressed= false;
            }
            else if ( !SavePressed && kstate.IsKeyDown(Keys.L) && kstate.IsKeyDown(Keys.LeftControl))
            {
                SavePressed = true;
                //Controller.Load();
            }
            if (kstate.IsKeyUp(Keys.L) && kstate.IsKeyUp(Keys.LeftControl))
            {
                SavePressed = false;
            }


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
            MouseState mstate = Mouse.GetState();
            HoveredPos = mstate.Position.ToVector2();
            if ( !mousedown && mstate.LeftButton == ButtonState.Pressed )
            {
                ClickedTile = HoveredTile;
                mousedown = true;
                PathPreviewOriginTile = ClickedTile;
            }
            else if ( mousedown && mstate.LeftButton == ButtonState.Pressed && ClickedTile == HoveredTile)
            {
                PressTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if ( PressTime > 0.3f )
                {
                    PathPreview.Clear();
                    if ( TryShowRadialAt( HoveredTile, HoveredPos ) )
                    { 

                    }
                }
            }
            else if ( mousedown && mstate.LeftButton == ButtonState.Released )
            {
                if (ActiveRadial != null)
                {
                    ActiveRadial.TryClickAt( ( HoveredPos - Camera.Position ) );
                }
                else
                    Click ( );
                mousedown = false;    
                ActiveRadial = null;
                PressTime = 0f;
            }
            Camera.Move( sprint /** 100f*/ * camTranslate );
        }
        public void Draw( SpriteBatch spriteBatch )
        {
            if (ActiveRadial != null)
                ActiveRadial.Draw( spriteBatch );
        }
        void Click()
        {
        }

        bool TryShowRadialAt( Vector2 tilePos, Vector2 renderPos )
        {
            RadialMenu r;
            bool ok = false;// mMaps.TryGetRadialMenuAt( tilePos, out r );
            //ActiveRadial = r;
            return ok;
        }
        public void DrawCursor(SpriteBatch spriteBatch)
        {
            Texture2D tex = TextureAssetManager.GetCursor(eMouseCursorType.Default);
            //if ( mUnits.UnitExistsAt( HoveredTile ) )
            //{ 
            //    tex = TextureAssetManager.GetCursor(eMouseCursorType.SelectUnit);
            //}
            //else if (SelectedUnits.Count != 0)
            //{ 
                if ( /*TerrainTypeMap.Pathable( HoveredTile )*/ true )
                { 
                    tex = TextureAssetManager.GetCursor( eMouseCursorType.Move );
                }
                //else
                //{
                //    tex = TextureAssetManager.GetCursor(eMouseCursorType.IllegalMove);
                //}
            //}
            spriteBatch.Draw( tex , HoveredPos - Camera.Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
        public void SetCameraDestination(Vector2 pos)
        {
            Camera.Position = pos;
        }
    }
}
