using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace Mononoke
{
    class City : MapEvent, IDraggable
    {
        int Max = 5;
        int _Current = 0;
        int Current
        {
            set
            {
                if (value > Max)
                    _Current = Max;
                else
                    _Current = value;
            }
            get
            {
                return _Current;
            }
        }
        Player Owner;
        public City( Vector2 pos, Player owner = null)
        {
            Position = pos;
            Owner = owner;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Mononoke.Font, Current.ToString(), Position * MapHolder.PIXELS_PER_TILE, Color.Black);
        }
        public override void OnClick(Player clicker)
        {
            if ( Current + 1 < Max )
            {
                clicker.Food--;
                Current++;
            }
        }
        protected override void OnExpire()
        {
            if ( Current - 1 < 0 )
            {
                Debug.WriteLine( "Starving city event");
                if ( Owner != null )
                    Owner.Stability--;
            }
            Current--;
        }
        public override bool TryLink(MapEvent partner)
        {
            Debug.WriteLine("trying link of city to " + partner);
            return true;
        }
    }
}
