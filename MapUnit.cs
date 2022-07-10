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
        public Vector2 Destination() { return Path [Path.Count-2]; }
        public Vector2 Location() { return Path[Path.Count-1]; }
        public List<Vector2> Path { get; private set; }

        public (Vector2, MapUnit) Target;
        public MapUnitAttack Attack { get; private set;}

        int _Health = 100;
        public int Health { get { return _Health; } 
            set 
            { 
                _Health = value;
            }
        }
        public Actor Owner;

        float currentSecond = 0f;
        float secondsToMove = 1f; // modified by terrain cost/

        public bool Selected = false;
        public MapUnit( Texture2D tex, int speed, string name, Vector2 pos, Actor owner )
        { 
            Speed = speed;
            Name = name;
            Sprite = tex;

            Attack = new MapUnitAttack();

            Path = new List<Vector2>(){ pos };
        }
        public bool MoveUpdate( GameTime gameTime ) // returns true if move complete
        {
            currentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ( currentSecond > secondsToMove )
            {
                currentSecond = 0f;
                return true;
            }
            return false;
        }
        public void CompleteMove()
        {
            if ( Path.Count > 1 )
            { 
                SetPath( Path.GetRange( 0, Path.Count - 1 ) );
            }
        }
        public bool AttackUpdate( GameTime gameTime )
        {
            return Attack.Update( gameTime );
        }
        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        public void Draw( SpriteBatch spriteBatch, Vector2 pos ) // pos is tile pos
        {
            //spriteBatch.Draw( Sprite, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            if ( Path.Count > 1 )
            {
                float progress = currentSecond / secondsToMove;
                Vector2 lerpedPos = new Vector2( 
                                    Lerp ( pos.X * MapHolder.PIXELS_PER_TILE, Destination().X * MapHolder.PIXELS_PER_TILE,  progress)
                                    ,Lerp ( pos.Y * MapHolder.PIXELS_PER_TILE, Destination().Y * MapHolder.PIXELS_PER_TILE, progress ) 
                                    );
                spriteBatch.Draw(Sprite, lerpedPos, Color.White);
                //spriteBatch.DrawString(Mononoke.Font, Name, lerpedPos ,Color.White);
            }
            else
            { 
                spriteBatch.Draw(Sprite, pos * MapHolder.PIXELS_PER_TILE, Color.White);
                //spriteBatch.DrawString( Mononoke.Font, Name, pos * MapHolder.PIXELS_PER_TILE, Color.White);
            }
        }
        public void SetPath( List<Vector2> path )
        {
            Path = path;
            currentSecond = 0f;
        }
        public bool Stationary()
        {
            return Path.Count == 1 ;
        }

    }
}
