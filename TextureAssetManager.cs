using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    //public enum eMouseCursorType
    //{
    //    Default
    //    ,Move
    //    ,IllegalMove
    //    ,SelectUnit
    //}
    public static class TextureAssetManager
    {
        private static TextureHolder Icons;
        private static TextureHolder mCarSprites;
        private static TextureHolder mTerrain;
        private static TextureHolder mDoodads;
        private static Texture2D simpleSpuare;

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

            Icons = new TextureHolder( graphics, "data/textures/icons/", brokenTexture );
            mCarSprites = new TextureHolder(graphics, "data/textures/units/", brokenTexture );
            mTerrain = new TextureHolder(graphics, "data/textures/terrain/", brokenTexture);
            mDoodads = new TextureHolder(graphics, "data/textures/doodads/", brokenTexture);
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
        public static Texture2D GetTerrainTileByName(string str)
        {
            return mTerrain.GetTextureByName(str);
        }
        public static Texture2D GetDoodadByname(string str)
        {
            return mDoodads.GetTextureByName(str);
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
