using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Mononoke.MapEvents
{
    public abstract class MapEvent
    {
        protected float CurrentSecond = 0;
        protected float SecondsToExpire = 5f;
        protected Vector2 Origin;
        protected List<Vector2> Tiles;

        public MapEvent(Vector2 pos )
        {
            Origin = pos;
            Tiles = new List<Vector2>() { pos };
        }
        public void Update(GameTime gameTime)
        {
            CurrentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (CurrentSecond > SecondsToExpire)
            {
                CurrentSecond = 0;
                OnExpire();
            }
        }
        protected abstract void OnExpire();
        public abstract void OnClick( Player clicker );
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual bool TryLink( MapEvent partner, MapHolder maps )
        {
            Debug.WriteLine( "Can't link these events ");
            return false;
        }
    }
}
