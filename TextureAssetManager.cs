using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    public static class TextureAssetManager
    {
        private static bool initialised = false;
        private static TextureHolder Icons;
        private static TextureHolder UnitSprites;
        
        public static void Initialise(GraphicsDeviceManager graphics)
        {
            Texture2D brokenTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Color[] pixels = new Color[1];
            pixels[0] = Color.Magenta;
            brokenTexture.SetData(pixels);

            Icons = new TextureHolder( graphics, "data/textures/gui/", brokenTexture );
            UnitSprites = new TextureHolder(graphics, "data/textures/units/", brokenTexture);
            initialised = true;
        }
        public static Texture2D GetIconByName( string str )
        { 
            return Icons.GetTextureByName( str );
        }
        public static Texture2D GetUnitSpriteByName(string str)
        {
            return UnitSprites.GetTextureByName(str);
        }
    }
}
