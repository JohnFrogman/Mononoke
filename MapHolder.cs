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
        public const int PIXELS_PER_TILE = 16;
        public const int MAP_TILE_WIDTH = 256;
        public const int MAP_PIXEL_WIDTH = PIXELS_PER_TILE * MAP_TILE_WIDTH;
        public const int MAP_TILE_HEIGHT = 256;
        public const int MAP_PIXEL_HEIGHT = PIXELS_PER_TILE * MAP_TILE_HEIGHT;
        public const int SCREEN_TILE_WIDTH = 2 + Mononoke.RENDER_WIDTH / PIXELS_PER_TILE;
        public const int SCREEN_TILE_HEIGHT = 2 + Mononoke.RENDER_HEIGHT / PIXELS_PER_TILE; 

       MapPainter TerrainPainter;
        MapPainter ProvincePainter;
        public MapHolder( GraphicsDeviceManager graphics )        
        {
            TerrainPainter = new MapPainter( "data/maps/test_terrain.png", graphics );
            ProvincePainter = new MapPainter("data/maps/test_provinces.png", graphics ); 
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos )
        {
            TerrainPainter.Draw( spriteBatch, graphics, pos ); // Terrain Colours
        }
        // Dictionary< Province, List<Pair( ResourceType, Location ) >>
        public Dictionary<Color, List<Tuple<Vector2, MapEvent>>> GetProvinceMapEvents()
        {
            Dictionary<Color, List<Tuple<Vector2, MapEvent>>> result = new Dictionary<Color, List<Tuple<Vector2, MapEvent>>>();
            for (int x = 0; x < MAP_TILE_WIDTH; x++)
            {
                for (int y = 0; y < MAP_TILE_HEIGHT; y++)
                {
                    Vector2 pos = new Vector2(x, y);
                    if (TerrainTypeMap.GetColourTerrainType(TerrainPainter.TileColourMap[pos]) == eTerrainType.Farmland)
                    {
                        Color provinceCol = ProvincePainter.TileColourMap[pos];
                        MapEvent ev = new Farm( pos);
                        Tuple<Vector2, MapEvent> resourceTuple = new Tuple<Vector2, MapEvent>( pos, ev);
                        if (!result.ContainsKey(provinceCol))
                            result.Add(provinceCol, new List<Tuple<Vector2, MapEvent>>());
                        result[provinceCol].Add(resourceTuple);
                    }
                    else if (TerrainTypeMap.GetColourTerrainType(TerrainPainter.TileColourMap[pos]) == eTerrainType.Urban )
                    {
                        Color provinceCol = ProvincePainter.TileColourMap[pos];
                        MapEvent ev = new City( pos );
                        Tuple<Vector2, MapEvent> resourceTuple = new Tuple<Vector2, MapEvent>(pos, ev);
                        if (!result.ContainsKey(provinceCol))
                            result.Add(provinceCol, new List<Tuple<Vector2, MapEvent>>());
                        result[provinceCol].Add(resourceTuple);
                    }
                }
            }
            return result;
        }
        public Color GetProvinceColourAt( Vector2 pos )
        {
            return ProvincePainter.GetColourAt(pos);
        }
        public eTerrainType GetTerrainAt(Vector2 pos)
        {
            return TerrainTypeMap.GetColourTerrainType( TerrainPainter.GetColourAt(pos) );
        }
        public float GetMapCostAt( Vector2 pos )
        {
            eTerrainType terrainType = GetTerrainAt( pos );
            if ( terrainType == eTerrainType.Mountain )
                return 3;
            else 
                return 1;
        }
    }
}
