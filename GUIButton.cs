using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mononoke
{
    public delegate void clickEvent();
    class GUIButton
    {
        Rectangle rect;
        Texture2D tex;
        public string text;
        //delegate void onClick();
        //Delegate onClick;
        //delegate void onClick();
        private clickEvent onClick;

        public GUIButton( Rectangle r, Texture2D t, string str, clickEvent callback )
        { 
            rect = r;
            tex = t;
            text = str;
            onClick = callback;
        }
        public void Draw(SpriteBatch _spriteBatch )
        {
            _spriteBatch.Draw( tex, rect, Color.White);
        }
        public bool TryClick( Vector2 pos )
        { 
            if ( rect.Contains(pos / Mononoke.ScreenScaleV2) )
            {
                if ( onClick == null)
                    return false;
                onClick();
                return true;
            }
            return false;
        }
    }
}
