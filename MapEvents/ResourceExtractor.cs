using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Mononoke
{
    abstract class ResourceExtractor : ExpandableMapEvent
    {
        int Max = 5;
        int _Current = 0;
        int Increment = 1;
        int Current
        {
            set
            {
                if (value > Max * Level)
                    _Current = Max * Level;
                else
                    _Current = value;
            }
            get
            {
                return _Current;
            }
        }
        public ResourceExtractor(Vector2 pos) : base(pos)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.DrawString(Mononoke.Font, Current.ToString(), Origin * MapHolder.PIXELS_PER_TILE, Color.Black, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Mononoke.Font, Current.ToString(), Origin * MapHolder.PIXELS_PER_TILE + new Vector2(8, 0), Color.Black, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(Mononoke.Font, Current.ToString(), Origin * MapHolder.PIXELS_PER_TILE, Color.Black);
        }
        public override void OnClick(Player clicker)
        {
            int harvestAmount = 1;
            if (Current < harvestAmount)
            {
                Harvest ( clicker, Current );
                Current = 0;
            }
            else
            {
                Current -= harvestAmount;
                Harvest( clicker, harvestAmount );
            }
        }
        protected abstract void Harvest( Player clicker, int amount );
        protected override void OnExpire()
        {
            Current += Increment * Level;
        }
        //public abstract bool TryLink(MapEvent partner, MapHolder maps);

    }
}
