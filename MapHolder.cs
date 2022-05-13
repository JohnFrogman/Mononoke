using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
namespace Mononoke
{
    class MapHolder
    {
        public const int PIXELS_PER_TILE = 8;
        public const int MAP_TILE_WIDTH = 256;
        public const int MAP_PIXEL_WIDTH = PIXELS_PER_TILE * MAP_TILE_WIDTH;
        public const int MAP_TILE_HEIGHT = 256;
        public const int MAP_PIXEL_HEIGHT = PIXELS_PER_TILE * MAP_TILE_HEIGHT;

        MapPainter TerrainPainter;
        MapPainter ProvincePainter;
        public MapHolder( GraphicsDeviceManager graphics )        
        {
            TerrainPainter = new MapPainter( "data/maps/test_terrain.png", graphics );
            ProvincePainter = new MapPainter("data/maps/test_provinces.png", graphics ); 
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos )
        {
            TerrainPainter.Draw( spriteBatch, graphics, pos );
        }
        public void GetTerrainColourAt(Vector2 pos )
        {
            Debug.WriteLine( TerrainPainter.GetColourAt(pos) );
        }
        // Dictionary< Province, List<Pair( ResourceType, Location ) >>
        public Dictionary<Color, List<Tuple<eProvinceResourceType, Vector2>>> GetProvinceResources()
        {
            Dictionary<Color, List<Tuple<eProvinceResourceType, Vector2>>> result = new Dictionary<Color, List<Tuple<eProvinceResourceType, Vector2>>>();
            for (int x = 0; x < MAP_TILE_WIDTH; x++)
            {
                for (int y = 0; y < MAP_TILE_HEIGHT; y++)
                {
                    Vector2 pos = new Vector2(x, y);
                    if (TerrainTypeMap.GetColourTerrainType(TerrainPainter.TileColourMap[pos]) == eTerrainType.Farmland)
                    {
                        Color provinceCol = ProvincePainter.TileColourMap[pos];
                        Tuple<eProvinceResourceType, Vector2> resourceTuple = new Tuple<eProvinceResourceType, Vector2>(eProvinceResourceType.Food, pos);
                        if (!result.ContainsKey(provinceCol))
                            result.Add(provinceCol, new List<Tuple<eProvinceResourceType, Vector2>>());
                        result[provinceCol].Add(resourceTuple);
                    }
                }
            }
            return result;
        }
    
    }
}
