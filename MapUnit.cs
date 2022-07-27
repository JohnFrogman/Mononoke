using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Mononoke
{
    class MapUnit
    {
        string Name;
        public int Speed;
        //public Vector2 Pos; // Tile position
        Texture2D Sprite;
        public Vector2 UltimateDestination() { return Path[0]; }
        public Vector2 Destination() { return Path [Path.Count-1]; }
        public Vector2 Location;
        public List<Vector2> Path { get; private set; }

        public MapUnit Target;
        public MapUnitAttack Attack { get; private set;}
        MapHolder Maps;

        public int ZoneOfControl = 2;

        int MaxHealth = 100;
        int _Health = 100;
        public int Health { get { return _Health; } 
            set 
            { 
                if ( value < Health )
                    DamageAnimation();
                _Health = value;
            }
        }
        public Actor Owner;

        float currentSecond = 0f;
        float baseSpeed = 0.2f;
        float secondsToMove () { return baseSpeed * Maps.GetMapCostAt(Destination()); } // modified by terrain cost/

        public bool Selected = false;
        public MapUnit( Texture2D tex, int speed, string name, Vector2 pos, Actor owner, MapHolder maps )
        { 
            Location = pos;
            Speed = speed;
            Name = name;
            Sprite = tex;
            Maps = maps;
            Attack = new MapUnitAttack();

            Path = new List<Vector2>(){ };
        }
        public bool MoveUpdate( GameTime gameTime ) // returns true if move complete
        {
            currentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ( currentSecond >  secondsToMove() )
            {
                currentSecond = 0f;
                return true;
            }
            return false;
        }
        public void CompleteMove()
        {
            Location = Destination();
            if ( Path.Count > 0 )
            { 
                SetPath( Path.GetRange( 0, Path.Count - 1 ) );
            }
        }
        public bool AttackUpdate( GameTime gameTime )
        {
            if ( Stationary() )
            { 
                Attack.Update( gameTime );
                return Attack.Ready;
            }
            return false;
        }
        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        public void Draw( SpriteBatch spriteBatch ) // pos is tile pos
        {
            //spriteBatch.Draw( Sprite, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            Vector2 drawPos = GetSpritePos();

            spriteBatch.Draw(Sprite, drawPos, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            float healthPortion = (float)_Health / MaxHealth;
            float test = MapHolder.PIXELS_PER_TILE * healthPortion;

            //spriteBatch.Draw(TextureAssetManager.GetSimpleSquare(), drawPos + new Vector2(0,0), null, Color.Black, 0f, new Vector2(0, 0), new Vector2(MapHolder.PIXELS_PER_TILE, 4), SpriteEffects.None, 0f);
            Vector2 hpScale = new Vector2(MapHolder.PIXELS_PER_TILE * healthPortion, 2);
            Vector2 attackScale = new Vector2(MapHolder.PIXELS_PER_TILE * Attack.Progress(), 2);
            spriteBatch.Draw( TextureAssetManager.GetSimpleSquare(), drawPos + new Vector2(0, 26), null, new Color( 0, 255, 0, 255 ), 0f, new Vector2(0 ,0), hpScale, SpriteEffects.None, 0f );
            spriteBatch.Draw( TextureAssetManager.GetSimpleSquare(), drawPos + new Vector2(0, 29), null, Color.White, 0f, new Vector2(0, 0), attackScale, SpriteEffects.None, 0f );
        }
        public void SetPath( List<Vector2> path )
        {
            Path = path;
            currentSecond = 0f;
        }
        public bool Stationary()
        {
            return Path.Count == 0 ;
        }
        void DamageAnimation()
        {
        }
        public Vector2 GetSpritePos()
        {
            if (Path.Count > 0)
            {
                float progress = currentSecond / secondsToMove();
                Vector2 lerpedPos = new Vector2(
                                    Lerp( Location.X * MapHolder.PIXELS_PER_TILE, Destination().X * MapHolder.PIXELS_PER_TILE, progress)
                                    , Lerp(Location.Y * MapHolder.PIXELS_PER_TILE, Destination().Y * MapHolder.PIXELS_PER_TILE, progress)
                                    );
                return lerpedPos;
            }
            else
            {
                return Location * MapHolder.PIXELS_PER_TILE;
            }
        }
    }
}
