using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
namespace Mononoke
{
    public static class TextureMap
    {
        //private static bool initialised = false;
        //private static Dictionary<eTerrainType, Texture> terrainMap;
        //private static Texture TestTexture;
        //public static Texture GetTerrainTypeTexture( eTerrainType t )
        //{
            
           // if ( !initialised )
           //     Initialise();
            //return TestTexture;
            //if ( terrainMap.ContainsKey( t ) ) 
            //    return terrainMap[t];

            //throw new System.Exception( "[TextureMap] Invalid type for terrain." + t);
        //}
        private static void Initialise()
        {
            //string path = "data/textures/forest.png";
            //if ( !File.Exists( path ) )
            //{
            //    throw new Exception( "Map does not exist at this path " + path );
            //}
            //TestTexture = Texture2D.FromFile( graphics.GraphicsDevice, path );
            //terrainMap = new Dictionary<eTerrainType, Color>
            //{
            //    { eTerrainType.Forest,              new Color(  28,  80,  0, 255 ) }
            ////,   { eTerrainType.Hills, new Color32(1,1,1) }
            //,   { eTerrainType.Jungle,              new Color(  19,  55,  0, 255 ) }
            //,   { eTerrainType.Mountain,            new Color(  134,134,134, 255 ) }
            //,   { eTerrainType.Grassland,           new Color(  43, 123,  0, 255 ) }
            //,   { eTerrainType.GrassyHills,         new Color(  58, 117,  26, 255 ) }
            //,   { eTerrainType.Taiga,               new Color(  31, 128, 105, 255 ) }
            //,   { eTerrainType.Marsh,               new Color(  50, 128, 109, 255 ) }

            //,   { eTerrainType.Farmland,            new Color( 213, 240, 161, 255 ) }
            //,   { eTerrainType.Urban,               new Color( 166, 166, 166, 255 ) }

            //,   { eTerrainType.DeepOcean,           new Color(   0,   9, 123, 255) }
            //,   { eTerrainType.ShallowOcean,        new Color(   0,  13, 179, 255) }
            //,   { eTerrainType.Reef,                new Color(  88, 154, 224, 255) }
            //};
            //colourMap = terrainMap.ToDictionary((i) => i.Value, (i) => i.Key);
            //initialised = true;
        }
    }
}
