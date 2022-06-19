using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Linq;
namespace Mononoke
{
    
    public enum eTerrainType
    {
        Mountain
    
        //,Desert
        //,RockyWasteland
        //,Badlands

        ,Grassland
        //,GrassyHills

        //,Savanna 
        //,SavannaHills

        //,Marsh
        //,Shrubland
        //,ShrublandHills

        //,Swamp
        ,Forest
        //,Jungle

        //,Tundra
        //,Taiga
        //,SnowyHills

        ,Farmland
        ,Urban

        ,DeepOcean
        ,ShallowOcean
        ,Reef

        ,Road

    }
    public static class TerrainTypeMap
    {   
        private static bool initialised = false;
        private static Dictionary<eTerrainType, Color> terrainMap;
        private static Dictionary<Color,eTerrainType > colourMap;
        public static eTerrainType GetColourTerrainType( Color c )
        {
            if ( !initialised )
                Initialise();
            if ( colourMap.ContainsKey( c ) ) 
                return colourMap[c];

            throw new System.Exception( "[TerrainTypeMap] Invalid colour for terrain." + c);
        }
        public static Color GetTerrainColour( eTerrainType t )
        {
            if ( !initialised )
                Initialise();
            if ( terrainMap.ContainsKey( t ) ) 
                return terrainMap[t];
        
            throw new System.Exception( "[TerrainTypeMap] Invalid terrain type for terrain. " + t );
        }
        private static void Initialise()
        {
            terrainMap = new Dictionary<eTerrainType, Color>
            {
                { eTerrainType.Road,                new Color(  87,  54, 52, 255 ) }
            ,   { eTerrainType.Forest,              new Color(  28,  80,  0, 255 ) }
            //,   { eTerrainType.Hills, new Color32(1,1,1) }
            //,   { eTerrainType.Jungle,              new Color(  19,  55,  0, 255 ) }
            ,   { eTerrainType.Mountain,            new Color(  134,134,134, 255 ) }
            ,   { eTerrainType.Grassland,           new Color(  43, 123,  0, 255 ) }
            //,   { eTerrainType.GrassyHills,         new Color(  58, 117,  26, 255 ) }
            //,   { eTerrainType.Taiga,               new Color(  31, 128, 105, 255 ) }
            //,   { eTerrainType.Marsh,               new Color(  50, 128, 109, 255 ) }

            ,   { eTerrainType.Farmland,            new Color( 213, 240, 161, 255 ) }
            ,   { eTerrainType.Urban,               new Color( 166, 166, 166, 255 ) }

            ,   { eTerrainType.DeepOcean,           new Color(   0,   9, 123, 255) }
            ,   { eTerrainType.ShallowOcean,        new Color(   0,  13, 179, 255) }
            ,   { eTerrainType.Reef,                new Color(  88, 154, 224, 255) }
            };
            colourMap = terrainMap.ToDictionary((i) => i.Value, (i) => i.Key);
            initialised = true;
        }
        public static List<string> GetTerrainStringList()
        {
            if ( !initialised )
                Initialise();
            List<string> result = new List<string>();
            foreach ( eTerrainType tt in terrainMap.Keys )
            {
                result.Add(tt.ToString());
            }
            return result;
        }
        public static float GetMapCost(eTerrainType type)
        {
            if (type == eTerrainType.Mountain)
                return 3;
            else if ( type == eTerrainType.Road )
                return 0.4f;
            else
                return 1;
        }
        public static bool Pathable( eTerrainType type)
        {
            return type != eTerrainType.DeepOcean && type != eTerrainType.ShallowOcean;

        }
        public static bool Buildable( eTerrainType type)
        {
            return type != eTerrainType.Road
                && type != eTerrainType.Urban
                && type != eTerrainType.Farmland
                && Pathable( type );
        }
    }
}
