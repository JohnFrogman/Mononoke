using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Mononoke
{
    class RadialMenuItem
    {
        int radius;
        Vector2 renderPos; 
        Vector2 centre;
        Texture2D icon;
        Color colour;
        private clickEvent onClick;

        public RadialMenuItem( Vector2 pos, Texture2D t, clickEvent callback )
        {
            radius = 8;
            icon = t;
            renderPos = pos;
            centre = pos + new Vector2( radius, radius );
            onClick = callback;

        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(icon, renderPos, Color.White);
        }
        public bool TryClickAt(Vector2 pos)
        {
            if ( onClick != null && Vector2.Distance( pos, centre ) < radius )
            { 
                onClick();
                return true;
            }
            else return false;
        }
    }
}
