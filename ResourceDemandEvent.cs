using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mononoke
{
    class ResourceDemandEvent : MapEvent, IDraggable
    {
        private int Amount;
        Player Owner;
        public ResourceDemandEvent( int amount, Vector2 pos, Player owner = null )
        {
            Position = pos;
            Owner = owner;
            Amount = amount;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Mononoke.Font, Amount.ToString(), Position * MapHolder.PIXELS_PER_TILE, Color.Red);
        }

        public override void OnClick(Player clicker)
        {
            if ( clicker.Food > Amount )
            {
                clicker.Food -= Amount;
                CurrentSecond -= SecondsToExpire;
            }
                
        }
        protected override void OnExpire()
        {
            if ( Owner != null )
                Owner.Stability -= 1;
        }

    }
}
