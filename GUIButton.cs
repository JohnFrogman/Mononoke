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
        public string Text;
        //delegate void onClick();
        //Delegate onClick;
        //delegate void onClick();
        private clickEvent onClick;

        public GUIButton( Rectangle r, Texture2D t, string text, clickEvent callback )
        { 
            rect = r;
            tex = t;
            Text = text;
            onClick = callback;
        }
        public void Draw(SpriteBatch _spriteBatch )
        {
            _spriteBatch.Draw( tex, rect, Color.White);
            _spriteBatch.DrawString( Mononoke.Font, Text, new Vector2( rect.Left, rect.Top), Color.White );
        }
        public bool TryClick( Vector2 pos )
        { 
            if ( rect.Contains(pos ) )
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
