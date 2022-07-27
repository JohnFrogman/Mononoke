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
        bool dragging = false;

        float PressTime = 0f;

        MapHolder mMaps;
        ProvinceHolder mProvinces;
        Player mPlayer;
        GameEventQueue mEventQueue;
        Pathfinder mPathfinder;
        Overworld mOverworld;
        // Note, for unit movement we want to drag to draw an exact path, tap to select, tap again to move will do proper pathfinding
        MapUnitHolder mUnits;

        List<MapUnit> SelectedUnits;

        Vector2 HoveredPos;
        Vector2 HoveredTile;
        Vector2 ClickedTile;
        Vector2 PathPreviewOriginTile;
        Vector2 PathPreviewTile;

        List<Vector2> PathPreview;

        RadialMenu ActiveRadial;

        bool SavePressed; 
        public OverworldController( Overworld overworld, Camera2D camera, MapHolder maps, ProvinceHolder provinces, Player player, GameEventQueue eventQueue, GraphicsDeviceManager graphics, Mononoke game, MapUnitHolder units, Pathfinder pathfinder ) 
        {
            mOverworld = overworld;
            Game = game;
            Camera = camera;
            mMaps = maps;
            mProvinces = provinces;
            mPlayer = player;
            mEventQueue = eventQueue;
            mPathfinder = pathfinder;
            PathPreview = new List<Vector2>();
            mUnits = units;
            SelectedUnits = new List<MapUnit>();
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
                mOverworld.Load();
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
            Vector2 tile = ScreenPosToMapPos( HoveredPos );
            HoveredTile = tile;
            if ( !mousedown && mstate.LeftButton == ButtonState.Pressed )
            {
                ClickedTile = HoveredTile;
                dragging = true;// Maps.IsDraggable( ClickedTile );
                mousedown = true;
                PathPreviewOriginTile = ClickedTile;
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
                if (ActiveRadial != null)
                {
                    ActiveRadial.TryClickAt( ( HoveredPos - Camera.Position ) );
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
            if ( dragging && mousedown && mMaps.IsDraggable(PathPreviewOriginTile) && ClickedTile != HoveredTile && HoveredTile != PathPreviewTile ) 
            {
                PathPreviewTile = HoveredTile;
                SetDragPath();
            }
            else if (SelectedUnits.Count == 1 && (HoveredTile != PathPreviewTile || PathPreviewOriginTile != SelectedUnits[0].Location))
            {
                SetMovePath();
            }
                
            Camera.Move( sprint /** 100f*/ * camTranslate );
        }
        public void Draw( SpriteBatch spriteBatch )
        {
            if ( PathPreview.Count > 0)
            { 
                mPathfinder.DrawPathPreview( PathPreviewOriginTile, PathPreview, spriteBatch );
            }
            else if ( dragging )
            {
                DrawBoundingBox( spriteBatch );
            }

            if (ActiveRadial != null)
                ActiveRadial.Draw( spriteBatch );
            if ( SelectedUnits.Count > 0 )
            { 
                foreach ( MapUnit u in SelectedUnits )
                { 
                    DrawUnitSelectionBox( spriteBatch, u );
                    if ( !u.Stationary() )
                        DrawDestination( spriteBatch, u );
                }
            }
        }
        void DragEnd( )
        {   
            if ( !mMaps.IsDraggable( ClickedTile ) )
            { 
                int minx = (int)MathF.Min( ClickedTile.X, HoveredTile.X); //Smallest X
                int miny = (int)MathF.Min( ClickedTile.Y, HoveredTile.Y); //Smallest Y
                int maxx = (int)MathF.Max( ClickedTile.X, HoveredTile.X);  //Largest X
                int maxy = (int)MathF.Max( ClickedTile.Y, HoveredTile.Y);  //Largest Y
                
                int width = maxx - minx;
                int height = maxy - miny;

                List<Vector2> tilesInBound = new List<Vector2>();
                for (int x = minx; x < maxx; x++)
                {
                    for (int y = miny; y < maxy; y++)
                    {
                        tilesInBound.Add( new Vector2( x, y ) );
                    }
                }
                mUnits.TryGetUnitsAt( tilesInBound, out SelectedUnits );
            }
            else if (PathPreview.Count > 0 && mMaps.TryDragAt( ClickedTile, HoveredTile, mPlayer, PathPreview.GetRange(1, PathPreview.Count -1) ) )
            {
            }
            PathPreview.Clear();
        }
        void Click()
        {
            PathPreview.Clear();
            if ( SelectedUnits.Count > 0 )
            {
                if ( TryUnitMoveClick( HoveredTile ) )
                {
                    SelectedUnits.Clear();
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
            return mEventQueue.TryClickAt( mousePos );
        }
        bool TryMapClick( Vector2 tile)
        {
            return mMaps.TryClickAt(tile, mPlayer);
        }
        bool TryUnitSelectClick( Vector2 tile)
        {
            Debug.WriteLine( "Trying to select a unit at " + tile );
            MapUnit u;
            if ( mUnits.TryGetUnitAt(tile, out u) )
            { 
                SelectedUnits = new List<MapUnit>() { u };
                return true;
            }
            return false;
        }
        bool TryUnitMoveClick(Vector2 tile)
        {
            Debug.WriteLine("Trying to move a unit to " + tile );
            bool moveOk = false;
            foreach ( MapUnit u in SelectedUnits )
            { 
                List<Vector2> path = mPathfinder.GetPath( u.Location, ClickedTile, true );

                if ( path.Count > 0 )
                {
                    u.SetPath( path );
                    moveOk = true;
                }
            }
            return moveOk;
        }
        public Vector2 ScreenPosToMapPos( Vector2 mPos)
        {
            Vector2 result = mPos - ( Camera.Position );
            result /= ( MapHolder.PIXELS_PER_TILE );
            return Vector2.Floor(result);
        }
        void SetDragPath( )
        {
            PathPreview = mPathfinder.GetPath(PathPreviewOriginTile, PathPreviewTile, false );
        }
        void SetMovePath()
        {
            PathPreviewOriginTile = SelectedUnits[0].Location;
            PathPreviewTile = HoveredTile;
            PathPreview = mPathfinder.GetPath(PathPreviewOriginTile, PathPreviewTile, true);
        }
        bool TryShowRadialAt( Vector2 tilePos, Vector2 renderPos )
        {
            RadialMenu r;
            bool ok = mMaps.TryGetRadialMenuAt( tilePos, out r );
            ActiveRadial = r;
            return ok;
        }
        void DrawUnitSelectionBox( SpriteBatch spriteBatch, MapUnit unit )
        {
            spriteBatch.Draw( TextureAssetManager.GetSelectionBox(), unit.GetSpritePos(), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f );
        }
        void DrawDestination( SpriteBatch spriteBatch, MapUnit unit)
        {
            spriteBatch.Draw(TextureAssetManager.GetIconByName("move"), unit.UltimateDestination() * MapHolder.PIXELS_PER_TILE, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
        public void DrawCursor(SpriteBatch spriteBatch)
        {
            Texture2D tex = TextureAssetManager.GetCursor(eMouseCursorType.Default);
            if ( mUnits.UnitExistsAt( HoveredTile ) )
            { 
                tex = TextureAssetManager.GetCursor(eMouseCursorType.SelectUnit);
            }
            else if (SelectedUnits.Count != 0)
            { 
                if ( /*TerrainTypeMap.Pathable( HoveredTile )*/ true )
                { 
                    tex = TextureAssetManager.GetCursor( eMouseCursorType.Move );
                }
                //else
                //{
                //    tex = TextureAssetManager.GetCursor(eMouseCursorType.IllegalMove);
                //}
            }
            spriteBatch.Draw( tex , HoveredPos - Camera.Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
        private void DrawBoundingBox( SpriteBatch spriteBatch )
        {
            int minx = (int)MathF.Min(ClickedTile.X, HoveredTile.X); //Smallest X
            int miny = (int)MathF.Min(ClickedTile.Y, HoveredTile.Y); //Smallest Y
            int maxx = (int)MathF.Max(ClickedTile.X, HoveredTile.X);  //Largest X
            int maxy = (int)MathF.Max(ClickedTile.Y, HoveredTile.Y);  //Largest Y

            int width = maxx - minx;
            int height = maxy - miny;

            Rectangle r = new Rectangle(minx, miny, width, height);

            Vector2 origin = new Vector2(minx, miny) * MapHolder.PIXELS_PER_TILE;

            spriteBatch.Draw( TextureAssetManager.GetSimpleSquare(), origin, null, Color.CornflowerBlue, 0f, new Vector2( 0, 0 ), MapHolder.PIXELS_PER_TILE * new Vector2( maxx - minx, 0.2f ), SpriteEffects.None, 0f );
            spriteBatch.Draw( TextureAssetManager.GetSimpleSquare(), origin, null, Color.CornflowerBlue, 0f, new Vector2( 0, 0 ), MapHolder.PIXELS_PER_TILE * new Vector2( 0.2f, maxy - miny ), SpriteEffects.None, 0f );

            spriteBatch.Draw( TextureAssetManager.GetSimpleSquare(), MapHolder.PIXELS_PER_TILE * new Vector2( maxx, miny), null, Color.CornflowerBlue, 0f, new Vector2( 0, 0 ), MapHolder.PIXELS_PER_TILE * new Vector2( 0.2f, maxy - miny ), SpriteEffects.None, 0f );

            spriteBatch.Draw( TextureAssetManager.GetSimpleSquare(), MapHolder.PIXELS_PER_TILE * new Vector2( minx, maxy), null, Color.CornflowerBlue, 0f, new Vector2( 0, 0 ), MapHolder.PIXELS_PER_TILE * new Vector2( maxx - minx, 0.2f ), SpriteEffects.None, 0f );
        }
    }
}
