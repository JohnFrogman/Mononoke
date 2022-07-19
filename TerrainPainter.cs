using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Mononoke
{
    class TerrainPainter : MapPainter
    {
        Dictionary<Vector2, string> tileFeatureTextures; // each tiles texture
        Dictionary<Vector2, eTerrainType> tileBaseTextures; // each tiles base texture, may be different to colour in case of buildings.
        // string is what hte tile connects to, which is sorted.
        Dictionary< eTerrainType, Dictionary<string, Texture2D>> featureTextureMap;
        Dictionary< eTerrainType, Texture2D> baseTextureMap;

        public TerrainPainter( string colourMapPath, GraphicsDeviceManager graphics ) : base(colourMapPath, graphics )
        {
            LoadTerrain(graphics); 
            LoadTiles();
        }
        public override void DrawTile(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos)
        {
            Texture2D tex;
            float scale = MapHolder.PIXELS_PER_TILE;
            //Color col = TileColourMap[pos];
            if ( tileBaseTextures.ContainsKey(pos) )
            { 
                eTerrainType tt = TerrainTypeMap.GetColourTerrainType( TileColourMap[pos] );
                spriteBatch.Draw(baseTextureMap[ tileBaseTextures[pos]], pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                if (tileFeatureTextures.ContainsKey( pos ) )
                {
                    if ( featureTextureMap.ContainsKey( tt ) && featureTextureMap[tt].ContainsKey(tileFeatureTextures[pos]) )
                    { 
                        tex = featureTextureMap[ tt ][ tileFeatureTextures[pos] ];
                        scale = (float)MapHolder.PIXELS_PER_TILE / (float)tex.Width;
                        spriteBatch.Draw(tex, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                    }
                }
            }
            //spriteBatch.DrawString(Mononoke.Font, pos.X + ", " + pos.Y, pos * MapHolder.PIXELS_PER_TILE, Color.White, 0f, new Vector2(0,0), 0.2f, SpriteEffects.None, 0f );
        }
        public void SetColoursAt( List<Vector2> pos, Color col)
        {
            List<Vector2> changedTiles = new List<Vector2>(); 
            foreach ( Vector2 v in pos )
            {
                changedTiles.Add(v);
                SetColourAt( v, col );
                List<Vector2> neighbours = v.GetNeighbours();
                foreach ( Vector2 n in neighbours)
                {
                    if ( !pos.Contains(n))
                        changedTiles.Add(n);
                }
            }
            foreach ( Vector2 v in changedTiles) // Refresh all the tiles that have changed colour and those that neighbour them.
            { 
                tileFeatureTextures[v] = GetTileTextPathAt(v);
                SetBaseTextureAt(v);
            }

        }
        public override void SetColourAt(Vector2 pos, Color col)
        {
            base.SetColourAt(pos, col); // Need to set all colours at once or it's going to set the features incorrectly.
            
        }
        private void LoadTiles()
        {
            tileBaseTextures = new Dictionary<Vector2, eTerrainType>();
            tileFeatureTextures = new Dictionary<Vector2, string>();
            foreach (KeyValuePair<Vector2, Color> kvp in TileColourMap)
            {
                SetBaseTextureAt( kvp.Key );
                tileFeatureTextures.Add(kvp.Key, GetTileTextPathAt(kvp.Key));
            }
        }
        void SetBaseTextureAt(Vector2 pos )
        {
            Color col = TileColourMap[pos];
            eTerrainType tt = TerrainTypeMap.GetColourTerrainType(col);
            if (tt == eTerrainType.Road)
            {
                List<Vector2> neighbours = pos.GetNeighbours();
                Dictionary<eTerrainType, int> ttmap = new Dictionary<eTerrainType, int>();
                foreach (Vector2 v in neighbours)
                {
                    eTerrainType ntt = tileBaseTextures[v];
                    if (TerrainTypeMap.Pathable(ntt) && ntt != eTerrainType.Road)
                    {
                        if (ttmap.ContainsKey(ntt))
                            ttmap[ntt]++;
                        else
                            ttmap.Add(ntt, 1);
                    }
                }
                eTerrainType basett = eTerrainType.Road;
                int max = 0;
                foreach (KeyValuePair<eTerrainType, int> typePair in ttmap)
                {
                    if (typePair.Value > max)
                    {
                        max = typePair.Value;
                        basett = typePair.Key;
                    }
                }
                tileBaseTextures[pos] = basett;
            }
            else
            {
                //if ( tileBaseTextures.ContainsKey(pos))
                    tileBaseTextures[pos] = tt;
                //else
                 //   tileBaseTextures.Add(pos, tt);
            }
        }
        private string GetTileTextPathAt( Vector2 pos )
        {
            string result = "";
            List<string> connectsToList = new List<string>();
            foreach ( eTileFace d in Enum.GetValues(typeof(eTileFace))) // up down left right
            {
                Vector2 neighbour = pos.GetNeighbour(d);
                if (TileColourMap.ContainsKey(neighbour) && TileColourMap[neighbour] == TileColourMap[pos])
                {
                    connectsToList.Add(d.ToString());
                }
            }
            connectsToList = connectsToList.OrderBy(s => s).ToList();
            foreach (string s in connectsToList)
                result += s;
            return result;
        }

        private void LoadTerrain(GraphicsDeviceManager graphics)
        {
            featureTextureMap = new Dictionary<eTerrainType, Dictionary<string, Texture2D>>();
            baseTextureMap = new Dictionary<eTerrainType, Texture2D>();

            // ------------ placeholder we want to set the base texture in a cleverer way  --------------
            eTerrainType[] values = Enum.GetValues(typeof(eTerrainType)).Cast<eTerrainType>().ToArray();
            foreach (eTerrainType tt in values)
            {
                Texture2D t = new Texture2D(graphics.GraphicsDevice, 1, 1);
                Color[] pixel = new Color[] { TerrainTypeMap.GetTerrainColour(tt) };
                t.SetData(pixel);
                baseTextureMap.Add(tt, t);
            }
            // ------------------------------------------------------------------------------------------
            string path = "data/textures/terrain/terrain_manifest.json";
            if (!File.Exists(path))
            {
                throw new Exception("This terrain manifest file does not exist " + path);
            }
            JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
            JsonElement e = doc.RootElement;
            eTerrainType[] terrainTypes = Enum.GetValues(typeof(eTerrainType)).Cast<eTerrainType>().ToArray();
            foreach (eTerrainType tt in terrainTypes)
            {
                JsonElement el;
                if ( e.TryGetProperty(tt.ToString(), out el ) )
                    LoadTerrainSheet( "data/textures/terrain/", el.ToString(), tt, graphics );
                else
                {
                    Debug.WriteLine("No entry in manifest found for terrain type " + tt.ToString() );
                }
            }
        }
        private void LoadTerrainSheet( string rootPath, string filename, eTerrainType type, GraphicsDeviceManager graphics )
        {
            string manifestPath = rootPath + filename;
            if (!File.Exists(manifestPath))
            {
                throw new Exception("Manifest for this terrain type doesn't exist at this path! " + manifestPath);
            }
            featureTextureMap.Add( type, new Dictionary<string, Texture2D>() );
            JsonDocument doc = JsonDocument.Parse(File.ReadAllText(manifestPath));
            JsonElement root = doc.RootElement;
            JsonElement header = root.GetProperty("header");
            int tileSize = header.GetProperty("tilesize").GetInt32();
            string sheetPath = rootPath + header.GetProperty("filepath").ToString();

            if (!File.Exists(sheetPath))
            {
                throw new Exception("Sheet doesn't exist at this path! " + sheetPath);
            }
            Texture2D spriteSheet = Texture2D.FromFile(graphics.GraphicsDevice, sheetPath);

            JsonElement.ArrayEnumerator itr = root.GetProperty("tiles").EnumerateArray();
            foreach (JsonElement i in itr)
            {
                Texture2D tex = new Texture2D( graphics.GraphicsDevice, tileSize, tileSize );
                
                Point pos = new Point( tileSize * i.GetProperty("pos").GetProperty("x").GetInt32(), tileSize * i.GetProperty("pos").GetProperty("y").GetInt32());
                Rectangle r = new Rectangle( pos, new Point(tileSize, tileSize));
                Color[] data = new Color[tileSize * tileSize];
                spriteSheet.GetData(0, r, data, 0, data.Length);
                tex.SetData( data );

                string id = "";
                List<string> connectsToList = new List<string>();
                JsonElement.ArrayEnumerator connectsToItr = i.GetProperty("connectsto").EnumerateArray();
                foreach (JsonElement c in connectsToItr)
                {
                    connectsToList.Add(c.ToString());
                }
                connectsToList = connectsToList.OrderBy(s => s).ToList();
                foreach ( string s in connectsToList )
                    id += s;

                // Right now can only have one texture per connects to id, will change at a later date, with the selected texutre being based on noise
                featureTextureMap[type].Add( id, tex );
            }
        }
        public void LogTextureInfoAt( Vector2 pos)
        {
            Debug.WriteLine( "pos : " + TileColourMap[pos] );
            Debug.WriteLine("terrain type : " + TerrainTypeMap.GetColourTerrainType(TileColourMap[pos]));
            Debug.WriteLine("basetex : " + tileBaseTextures[pos]);
            Debug.WriteLine("featuretex : " + tileFeatureTextures[pos]);
        }
    }
}
