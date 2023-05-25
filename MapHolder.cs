using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Mononoke
{
    class MapHolder
    {
        public const int PIXELS_PER_TILE = 32;
        public const int MAP_TILE_WIDTH = 256;
        public const int MAP_PIXEL_WIDTH = PIXELS_PER_TILE * MAP_TILE_WIDTH;
        public const int MAP_TILE_HEIGHT = 256;
        public const int MAP_PIXEL_HEIGHT = PIXELS_PER_TILE * MAP_TILE_HEIGHT;
        public const int SCREEN_TILE_WIDTH = 2 + Mononoke.RENDER_WIDTH / PIXELS_PER_TILE;
        public const int SCREEN_TILE_HEIGHT = 2 + Mononoke.RENDER_HEIGHT / PIXELS_PER_TILE;

        TerrainPainter mTerrainPainter;
        MapPainter mProvincePainter;
        Dictionary< Vector2, MapEvent > MapEvents;
        public MapHolder( GraphicsDeviceManager graphics, MapUnitHolder units, MapUnitTypeHolder unitTypes )        
        {
            mTerrainPainter = new TerrainPainter( "data/maps/test_terrain.png", graphics );
            mProvincePainter = new MapPainter("data/maps/test_provinces.png", graphics );
            mProvincePainter = new MapPainter("data/maps/test_provinces.png", graphics);
            SetMapEvents( units, unitTypes );
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos )
        {
            mTerrainPainter.Draw( spriteBatch, graphics, pos ); // Terrain Colours
            foreach (MapEvent r in MapEvents.Values)
                r.Draw(spriteBatch);
        }
        public void SetMapEvents( MapUnitHolder units, MapUnitTypeHolder unitTypes )
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
                        if (TerrainTypeMap.GetColourTerrainType(mTerrainPainter.TileColourMap[pos]) == eTerrainType.Farmland)
                        {
                            ev = new Farm(pos, null );
                        }
                        else if (TerrainTypeMap.GetColourTerrainType(mTerrainPainter.TileColourMap[pos]) == eTerrainType.Urban)
                        {
                            ev = new City(pos, this, units, unitTypes, null  );
                        }
                        else if (TerrainTypeMap.GetColourTerrainType(mTerrainPainter.TileColourMap[pos]) == eTerrainType.PetrichorMine)
                        {
                            ev = new PetrichorMine(pos, null);
                        }
                        else if (TerrainTypeMap.GetColourTerrainType(mTerrainPainter.TileColourMap[pos]) == eTerrainType.LintExtractor)
                        {
                            ev = new LintExtractor(pos, null);
                        }
                        if ( ev != null )
                        { 
                            List<Vector2> positions = mTerrainPainter.GetClumpAt(pos);
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

        public Color GetProvinceColourAt( Vector2 pos )
        {
            return mProvincePainter.GetColourAt(pos);
        }
        public eTerrainType GetTerrainAt(Vector2 pos)
        {
            return TerrainTypeMap.GetColourTerrainType( mTerrainPainter.GetColourAt(pos) );
        }
        public void SetTerrainAt( Vector2 pos, eTerrainType terrain )
        {
            mTerrainPainter.SetColourAt(pos, TerrainTypeMap.GetTerrainColour(terrain));
        }
        public bool TrySetMapEventAt( Vector2 pos, MapEvent ev )
        { 
            if ( MapEvents.ContainsKey( pos ) )
                return false;
            else
            {
                MapEvents.Add( pos, ev );
                return true;
            }
        }
        public void SetTerrainAt(List<Vector2> pos, eTerrainType terrain)
        {
            mTerrainPainter.SetColoursAt( pos, TerrainTypeMap.GetTerrainColour( terrain ) );
        }
        public float GetMapCostAt( Vector2 pos )
        {
            eTerrainType terrainType = GetTerrainAt( pos );
            if ( terrainType == eTerrainType.Mountain )
                return 3;
            else if ( terrainType == eTerrainType.Road )
                return 0.25f;
            else 
                return 1;
        }
        public bool Pathable(Vector2 pos, MapUnitHolder units, bool unitsBlock)
        {
            if ( unitsBlock && units.UnitExistsAt(pos) )
            {
                return false;
            }
            return TerrainTypeMap.Pathable(GetTerrainAt(pos));

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
            //Debug.WriteLine("Map clicking at " + pos );
            //TerrainPainter.LogTextureInfoAt(pos);
            if (MapEvents.ContainsKey(pos))
            {
                //Debug.WriteLine("MapEvents contains " + pos);
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
        public bool TryGetRadialMenuAt( Vector2 pos, out RadialMenu m)
        {
            m = null;
            if (MapEvents.ContainsKey(pos))
            {
                if ( MapEvents[pos].TryGetRadialMenu( out m ) )
                { 
                    return true;
                }
            }
            return false;
        }
        public void Save( string slot, GraphicsDevice graphics )
        {
            mTerrainPainter.Save( slot, graphics );
            SaveMapEvents( slot );
        }
        public void Load( string slot, MapUnitHolder units, MapUnitTypeHolder unitTypes, GraphicsDeviceManager graphics )
        {
            mTerrainPainter = new TerrainPainter(slot + "/terrain.png", graphics);
            LoadMapEvents(units, unitTypes, slot);
            //LoadMapEvents( slot );
        }
        
        void LoadMapEvents( MapUnitHolder units, MapUnitTypeHolder unitTypes, string slot )
        {

            //string path = slot + "map_events.json";
            //if (!File.Exists(path))
            //{
            //    throw new Exception("The map events file does not exist " + path);
            //}
            //JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
            //JsonElement e = doc.RootElement;
            //JsonElement.ObjectEnumerator itr = e.EnumerateObject();

            //MapEvents = new Dictionary<Vector2, MapEvent>();

            //foreach (JsonProperty i in itr)
            //{
            //    JsonElement e = i.Value.ToString
             
            //}

        }
        void SaveMapEvents(string slot)
        {
            string json = "{";
            foreach ( KeyValuePair<Vector2, MapEvent> kvp in MapEvents )
            {
                json += kvp.Key.ToJson();
                json += ",";
            }
            json = json.Substring( json.Length - 1 );
            json += "}";
            File.WriteAllText(slot + "/map_events.json", json);
        }
        public void UnitEntered( Vector2 location, MapUnit unit)
        {
            if ( MapEvents.ContainsKey(location) )
            {
                MapEvents[location].UnitEntered( unit );  
            }
        }
    }
}
