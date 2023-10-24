using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    public enum eMouseCursorType
    {
        Default
        ,Move
        ,IllegalMove
        ,SelectUnit
    }
    public static class TextureAssetManager
    {
        private static TextureHolder Icons;
        private static TextureHolder mCarSprites;
        private static Texture2D simpleSpuare;
        private static MouseCursorHolder Cursors;
        public static Texture2D GetCursor( eMouseCursorType type )
        {
            switch (type)
            {
                case eMouseCursorType.Move :
                {
                    return Cursors.MoveCursor;
                }
                case eMouseCursorType.IllegalMove:
                {
                    return Cursors.IllegalMoveCursor;
                }
                case eMouseCursorType.SelectUnit:
                {
                    return Cursors.SelectCursor;
                }
                default :
                {
                    return Cursors.DefaultCursor;
                }
            }
        }
        
        public static void Initialise( GraphicsDeviceManager graphics )
        {
            Texture2D brokenTexture = new Texture2D(graphics.GraphicsDevice, 1,1);
            Color[] pixels = new Color[1];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.Magenta;
            }
            brokenTexture.SetData(pixels);
            SetSimpleSquare( graphics );

            Cursors = new MouseCursorHolder( graphics, brokenTexture );

            Icons = new TextureHolder( graphics, "data/textures/icons/", brokenTexture );
            mCarSprites = new TextureHolder(graphics, "data/textures/units/", brokenTexture );
        }
        public static Texture2D GetIconByName( string str )
        { 
            return Icons.GetTextureByName( str );
        }
        public static Texture2D GetCarSpriteByName(string str)
        {
            return mCarSprites.GetTextureByName(str);
        }
        public static Texture2D GetSimpleSquare()
        { 
            return simpleSpuare;
        }
        public static Texture2D GetPlayerSprite()
        { 
            return mCarSprites.GetTextureByName("soldier");
        }
        static void SetSimpleSquare( GraphicsDeviceManager graphics )
        {
            simpleSpuare = new Texture2D( graphics.GraphicsDevice, 1, 1 );
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            simpleSpuare.SetData(pixels);
        }
    }
}
