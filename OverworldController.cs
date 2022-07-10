using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace Mononoke
{
    class OverworldController
    {
        Mononoke Game;
        Camera2D Camera;
        bool mousedown = false;
        bool dragging = false;

        float PressTime = 0f;

        MapHolder Maps;
        ProvinceHolder Provinces;
        Player Player;
        GameEventQueue EventQueue;
        Pathfinder Pathfinder;
        // Note, for unit movement we want to drag to draw an exact path, tap to select, tap again to move will do proper pathfinding
        MapUnitHolder Units;

        MapUnit SelectedUnit;

        Vector2 HoveredPos;
        Vector2 HoveredTile;
        Vector2 ClickedTile;
        Vector2 PathPreviewTile;

        List<Vector2> PathPreview;

        RadialMenu ActiveRadial;
        public OverworldController( Camera2D camera, MapHolder maps, ProvinceHolder provinces, Player player, GameEventQueue eventQueue, GraphicsDeviceManager graphics, Mononoke game, MapUnitHolder units, Pathfinder pathfinder ) 
        {
            Game = game;
            Camera = camera;
            Maps = maps;
            Provinces = provinces;
            Player = player;
            EventQueue = eventQueue;
            Pathfinder = pathfinder;
            PathPreview = new List<Vector2>();
            Units = units;
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
            MouseState mstate = Mouse.GetState();
            HoveredPos = mstate.Position.ToVector2();
            Vector2 tile = ScreenPosToMapPos( HoveredPos );
            HoveredTile = tile;
            if ( !mousedown && mstate.LeftButton == ButtonState.Pressed )
            {
                ClickedTile = HoveredTile;
                dragging = Maps.IsDraggable( ClickedTile );
                mousedown = true;
            }
            else if ( mousedown && mstate.LeftButton == ButtonState.Pressed && ClickedTile == HoveredTile)
            {
                PressTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if ( PressTime > 0.3f )
                {
                    dragging = false;
                    PathPreview.Clear();
                    if ( TryShowRadialAt( HoveredTile, HoveredPos ) )
                    { 

                    }
                }
            }
            else if ( mousedown && mstate.LeftButton == ButtonState.Released )
            {
                Debug.WriteLine("Mouse Release");
                if (ActiveRadial != null)
                {
                    ActiveRadial.TryClickAt( ( HoveredPos - Camera.Position ) / Mononoke.ScreenScaleV2  );
                }
                else if ( ClickedTile != HoveredTile )
                    DragEnd();
                else
                    Click ( );
                mousedown = false;    
                dragging = false;
                ActiveRadial = null;
                PressTime = 0f;
            }
            
            if ( dragging && mousedown && ClickedTile != HoveredTile && HoveredTile != PathPreviewTile )
            {
                PathPreviewTile = HoveredTile;
                SetDragPath();
            }
                
            Camera.Move( sprint /** 100f*/ * camTranslate );
        }
        public void Draw( SpriteBatch spriteBatch)
        {
            if ( PathPreview.Count > 0)
                Pathfinder.DrawPathPreview( PathPreview, spriteBatch );
            if (ActiveRadial != null)
                ActiveRadial.Draw( spriteBatch );
        }
        void DragEnd( )
        {   
            Debug.WriteLine("Drag end");
            if (PathPreview.Count > 0 && Maps.TryDragAt( ClickedTile, HoveredTile, Player, PathPreview.GetRange(1, PathPreview.Count -1) ) )
            {
            }
            PathPreview.Clear();
        }
        void Click()
        {
            if ( SelectedUnit != null )
            {
                if ( TryUnitMoveClick( HoveredTile ) )
                {
                    SelectedUnit = null;
                    return;
                }
            }
            if ( TryUnitSelectClick( HoveredTile) )
                return;
            if ( TryEventClick( HoveredPos ) )
                return;
            if ( TryMapClick( HoveredTile ) )
                return;
        }
        bool TryEventClick( Vector2 mousePos )
        {
            return EventQueue.TryClickAt( mousePos );
        }
        bool TryMapClick( Vector2 tile)
        {
            return Maps.TryClickAt(tile, Player);
        }
        bool TryUnitSelectClick( Vector2 tile)
        {
            Debug.WriteLine( "Trying to select a unit at " + tile );
            if ( Units.TryGetUnitAt(tile, out SelectedUnit) )
            { 
            }
            return false;
        }
        bool TryUnitMoveClick(Vector2 tile)
        {
            Debug.WriteLine("Trying to move a unit to " + tile );

            List<Vector2> path = Pathfinder.GetPath( SelectedUnit.Location(), ClickedTile, true );

            if ( path.Count > 0 )
            {
                SelectedUnit.SetPath( path );
                return true;
            }
            return false;
        }
        public Vector2 ScreenPosToMapPos( Vector2 mPos)
        {
            Vector2 result = mPos - ( Camera.Position * Mononoke.ScreenScaleV2 );
            result /= ( MapHolder.PIXELS_PER_TILE * Mononoke.ScreenScaleV2 );
            return Vector2.Floor(result);
        }
        void SetDragPath( )
        {
            PathPreview = Pathfinder.GetPath( ClickedTile, PathPreviewTile, false );
        }
        bool TryShowRadialAt( Vector2 tilePos, Vector2 renderPos )
        {
            RadialMenu r;
            bool ok = Maps.TryGetRadialMenuAt( tilePos, out r );
            ActiveRadial = r;
            return ok;
        }
    }
}
