using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Mononoke.MapEvents;
namespace Mononoke
{
    public class MapHolder
    {
        public const int PIXELS_PER_TILE = 16;
        public const int MAP_TILE_WIDTH = 256;
        public const int MAP_PIXEL_WIDTH = PIXELS_PER_TILE * MAP_TILE_WIDTH;
        public const int MAP_TILE_HEIGHT = 256;
        public const int MAP_PIXEL_HEIGHT = PIXELS_PER_TILE * MAP_TILE_HEIGHT;
        public const int SCREEN_TILE_WIDTH = 2 + Mononoke.RENDER_WIDTH / PIXELS_PER_TILE;
        public const int SCREEN_TILE_HEIGHT = 2 + Mononoke.RENDER_HEIGHT / PIXELS_PER_TILE;

        TerrainPainter TerrainPainter;
        MapPainter ProvincePainter;
        Dictionary< Vector2, MapEvent > MapEvents;
        public MapHolder( GraphicsDeviceManager graphics )        
        {
            TerrainPainter = new TerrainPainter( "data/maps/test_terrain.png", graphics );
            ProvincePainter = new MapPainter("data/maps/test_provinces.png", graphics );
            ProvincePainter = new MapPainter("data/maps/test_provinces.png", graphics);
            SetMapEvents();
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos )
        {
            TerrainPainter.Draw( spriteBatch, graphics, pos ); // Terrain Colours
            foreach (MapEvent r in MapEvents.Values)
                r.Draw(spriteBatch);
        }
        public void SetMapEvents()
        {
            MapEvents = new Dictionary<Vector2, MapEvent>();
            for (int x = 0; x < MAP_TILE_WIDTH; x++)
            {
                for (int y = 0; y < MAP_TILE_HEIGHT; y++)
                {
                    Vector2 pos = new Vector2(x, y);
                    if ( !MapEvents.ContainsKey(pos))
                    {
                        MapEvent ev = null;
                        if (TerrainTypeMap.GetColourTerrainType(TerrainPainter.TileColourMap[pos]) == eTerrainType.Farmland)
                        {
                            ev = new Farm(pos);
                        }
                        else if (TerrainTypeMap.GetColourTerrainType(TerrainPainter.TileColourMap[pos]) == eTerrainType.Urban)
                        {
                            ev = new City(pos);
                        }
                        if ( ev != null )
                        { 
                            List<Vector2> positions = TerrainPainter.GetClumpAt(pos);
                            foreach (Vector2 p in positions)
                            {
                                //ev.AddPosition( p );
                                MapEvents.Add(p, ev);
                            }
                        }
                    }
                }
            }
        }
        // Dictionary< Province, List<Pair( ResourceType, Location ) >>
        //public Dictionary<Color, List<Tuple<Vector2, MapEvent>>> GetProvinceMapEvents()
        //{
        //    Dictionary<Color, List<Tuple<Vector2, MapEvent>>> result = new Dictionary<Color, List<Tuple<Vector2, MapEvent>>>();
        //    for (int x = 0; x < MAP_TILE_WIDTH; x++)
        //    {
        //        for (int y = 0; y < MAP_TILE_HEIGHT; y++)
        //        {
        //            Vector2 pos = new Vector2(x, y);
        //            if (TerrainTypeMap.GetColourTerrainType(TerrainPainter.TileColourMap[pos]) == eTerrainType.Farmland)
        //            {
        //                Color provinceCol = ProvincePainter.TileColourMap[pos];
        //                MapEvent ev = new Farm( pos);
        //                Tuple<Vector2, MapEvent> resourceTuple = new Tuple<Vector2, MapEvent>( pos, ev);
        //                if (!result.ContainsKey(provinceCol))
        //                    result.Add(provinceCol, new List<Tuple<Vector2, MapEvent>>());
        //                result[provinceCol].Add(resourceTuple);
        //            }
        //            else if (TerrainTypeMap.GetColourTerrainType(TerrainPainter.TileColourMap[pos]) == eTerrainType.Urban )
        //            {
        //                Color provinceCol = ProvincePainter.TileColourMap[pos];
        //                MapEvent ev = new City( pos );
        //                Tuple<Vector2, MapEvent> resourceTuple = new Tuple<Vector2, MapEvent>(pos, ev);
        //                if (!result.ContainsKey(provinceCol))
        //                    result.Add(provinceCol, new List<Tuple<Vector2, MapEvent>>());
        //                result[provinceCol].Add(resourceTuple);
        //            }
        //        }
        //    }
        //    return result;
        //}
        public Color GetProvinceColourAt( Vector2 pos )
        {
            return ProvincePainter.GetColourAt(pos);
        }
        public eTerrainType GetTerrainAt(Vector2 pos)
        {
            return TerrainTypeMap.GetColourTerrainType( TerrainPainter.GetColourAt(pos) );
        }
        public void SetTerrainAt(List<Vector2> pos, eTerrainType terrain)
        {
            TerrainPainter.SetColoursAt( pos, TerrainTypeMap.GetTerrainColour( terrain ) );
        }
        public float GetMapCostAt( Vector2 pos )
        {
            eTerrainType terrainType = GetTerrainAt( pos );
            if ( terrainType == eTerrainType.Mountain )
                return 3;
            else 
                return 1;
        }
        public bool Pathable(Vector2 pos)
        {
            
            return TerrainTypeMap.Pathable(GetTerrainAt(pos));// type != eTerrainType.DeepOcean && type != eTerrainType.ShallowOcean;

        }
        public bool TryGetMapEventAt( Vector2 pos, out MapEvent ev )
        {
            ev = null;
            if ( MapEvents.ContainsKey(pos ))
            { 
                ev = MapEvents[pos];
                return true;
            }
            return false;
        }
        public bool TryClickAt(Vector2 pos, Player clicker)
        {
            Debug.WriteLine("Map clicking at " + pos );
            TerrainPainter.LogTextureInfoAt(pos);
            if (MapEvents.ContainsKey(pos))
            {
                Debug.WriteLine("MapEvents contains " + pos);
                MapEvents[pos].OnClick(clicker);
                return true;
            }
            return false;
        }
        public bool TryDragAt(Vector2 origin, Vector2 destination, Player dragQueen, List<Vector2> path )
        {
            if (MapEvents.ContainsKey(origin) && MapEvents.ContainsKey(destination))
            {
                MapEvent ev1 = MapEvents[origin];
                MapEvent ev2 = MapEvents[destination];
                if ( ev1.TryLink(ev2, this, path) )
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsDraggable(Vector2 pos)
        {
            return MapEvents.ContainsKey(pos) && MapEvents[pos] is IDraggable;
        }
        public void Update(GameTime gameTime)
        {
            foreach (MapEvent r in MapEvents.Values)
                r.Update(gameTime);
        }
    }
}
