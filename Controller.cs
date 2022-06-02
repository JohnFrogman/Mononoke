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
                Debug.WriteLine("Mouse Down");
                dragging = Provinces.IsDraggable( DragOriginTile );
                Debug.WriteLine("Dragging is " + dragging);
                DragOriginTile = HoveredTile;
                mousedown = true;
            }
            else if ( mousedown && mstate.LeftButton == ButtonState.Released )
            {
                Debug.WriteLine("Mouse Release");
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
            PathPreview.Clear();
            if ( Provinces.TryDragAt( DragOriginTile, HoveredTile, Player ) )
            {
            }
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
            Provinces.TryClickAt(tile, Player);
            return true;
        }
        public Vector2 ScreenPosToMapPos( Vector2 mPos)
        {
            Vector2 result = mPos - Camera.Position;
            result /= ( MapHolder.PIXELS_PER_TILE * new Vector2( Mononoke.SCALE_X, Mononoke.SCALE_Y ) ) ;
            return Vector2.Floor(result);
        }
        void SetDragPath( )
        {
            PathPreview = pathfinder.GetPath( DragOriginTile, PathPreviewTile, Maps );
        }
    }
}
