using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Mononoke
{
    abstract class MapEvent
    {
        protected float CurrentSecond = 0;
        protected float SecondsToExpire = 5f;
        protected bool Paused = false;
        protected Vector2 Origin;
        protected List<Vector2> Tiles;

        public MapEvent(Vector2 pos )
        {
            Origin = pos;
            Tiles = new List<Vector2>() { pos };
        }
        public void Update(GameTime gameTime)
        {
            if ( !Paused )
            { 
                CurrentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (CurrentSecond > SecondsToExpire)
                {
                    CurrentSecond = 0;
                    OnExpire();
                }
            }
        }
        protected abstract void OnExpire();
        public abstract void OnClick( Player clicker );
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureAssetManager.GetSimpleSquare(), ( Origin * MapHolder.PIXELS_PER_TILE) + new Vector2(0, 28), null, Color.White, 0f, new Vector2(0, 0), new Vector2(  MapHolder.PIXELS_PER_TILE, 4), SpriteEffects.None, 0f);
            spriteBatch.Draw(TextureAssetManager.GetSimpleSquare(), ( Origin * MapHolder.PIXELS_PER_TILE ) + new Vector2(0, 29), null, Color.Black, 0f, new Vector2(0, 0), new Vector2( GetProgress() * MapHolder.PIXELS_PER_TILE, 2f) , SpriteEffects.None, 0f);
        }
        protected float GetProgress()
        {
            return CurrentSecond / SecondsToExpire;
        }
        public virtual bool TryGetRadialMenu( out RadialMenu r )
        {
            r = null;
            return false;
        }
        public virtual bool TryLink( MapEvent partner, MapHolder maps, List<Vector2> path )
        {
            Debug.WriteLine( "Can't link these events ");
            return false;
        }
    }
}
