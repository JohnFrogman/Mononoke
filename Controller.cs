using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace Mononoke
{
    class Controller
    {
        Mononoke Game;
        Camera2D Camera;
        bool mousedown = false;
        bool dragging = false;
        MapHolder Maps;
        ProvinceHolder Provinces;
        Player Player;
        GameEventQueue EventQueue;
        Pathfinder pathfinder;

        Vector2 HoveredPos;
        Vector2 HoveredTile;
        Vector2 DragOriginTile;
        Vector2 PathPreviewTile;

        List<Vector2> PathPreview;
        public Controller( Camera2D camera, Mononoke game, MapHolder maps, ProvinceHolder provinces, Player player, GameEventQueue eventQueue, GraphicsDeviceManager graphics ) 
        {
            Game = game;
            Camera = camera;
            Maps = maps;
            Provinces = provinces;
            Player = player;
            EventQueue = eventQueue;
            pathfinder = new Pathfinder( graphics );
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
                DragOriginTile = HoveredTile;
                dragging = Maps.IsDraggable( DragOriginTile );
                mousedown = true;
            }
            else if ( mousedown && mstate.LeftButton == ButtonState.Released )
            {
               // Debug.WriteLine("Mouse Release");
                if ( DragOriginTile != HoveredTile )
                    DragEnd();
                else
                    Click ( );
                mousedown = false;    
                dragging = false;
            }
            
            if ( dragging && mousedown && DragOriginTile != HoveredTile && HoveredTile != PathPreviewTile )
            {
                PathPreviewTile = HoveredTile;
                SetDragPath();
            }
                
            Camera.Move( sprint /** 100f*/ * camTranslate );
        }
        public void Draw( SpriteBatch spriteBatch)
        {
            if ( PathPreview.Count > 0)
                pathfinder.DrawPathPreview( PathPreview, spriteBatch );
        }
        void DragEnd( )
        {   
            if (PathPreview.Count > 0 && Maps.TryDragAt( DragOriginTile, HoveredTile, Player, PathPreview.GetRange(1, PathPreview.Count -1) ) )
            {
            }
            PathPreview.Clear();
        }
        void Click()
        {
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
        public Vector2 ScreenPosToMapPos( Vector2 mPos)
        {
            Vector2 result = mPos - ( Camera.Position * Mononoke.ScreenScaleV2 );
            result /= ( MapHolder.PIXELS_PER_TILE * Mononoke.ScreenScaleV2 );
            return Vector2.Floor(result);
        }
        void SetDragPath( )
        {
            PathPreview = pathfinder.GetPath( DragOriginTile, PathPreviewTile, Maps );
        }
    }
}
