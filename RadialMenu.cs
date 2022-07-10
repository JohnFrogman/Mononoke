using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    class RadialMenu
    {
        List<RadialMenuItem> Items;
        public RadialMenu( List<RadialMenuItem> items )
        { 
            Items = items;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach ( RadialMenuItem i in Items )
                i.Draw( spriteBatch );
        }
        public bool TryClickAt( Vector2 pos )
        {
            foreach ( RadialMenuItem i in Items )
            {
                if ( i.TryClickAt(pos) )
                {
                    return true;
                }
            }
            return false;
        }
    }
}
