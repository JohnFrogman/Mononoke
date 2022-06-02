using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;

namespace Mononoke
{
    class TerrainPainter : MapPainter
    {
        Dictionary<Vector2, string> tileTextures; // position and key in texture map, so we can store the textures and later change. move away from string as id eventually.
        Dictionary< string, Texture2D > textureMap;
        string RootPath = "data/textures/forest/";
        //Dictionary< Vector2, eTerrainType > TileTerrainMap;
        public TerrainPainter( string colourMapPath, GraphicsDeviceManager graphics ) : base(colourMapPath, graphics )
        {
            textureMap = new Dictionary<string, Texture2D>();
            //TileTerrainMap = new Dictionary<Vector2, eTerrainType>();
            tileTextures = new Dictionary<Vector2, string>();
            string[] fileNames = Directory.GetFiles( RootPath );  
            foreach ( string str in fileNames )
            {
                
                Texture2D tex = Texture2D.FromFile(graphics.GraphicsDevice, str);
                textureMap.Add( str, tex );
            }
            foreach ( KeyValuePair<Vector2, Color> kvp in TileColourMap)            
            {
                eTerrainType tt = TerrainTypeMap.GetColourTerrainType(kvp.Value);
                if ( tt != eTerrainType.Forest )
                    continue;
                //TileTerrainMap.Add( kvp.Key, tt );
                tileTextures.Add( kvp.Key, GetTileTextPathAt( kvp.Key, kvp.Value ) );
            }
        }
        public override void DrawTile(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos)
        {
            Texture2D tex;
            //Color col = TileColourMap[pos];
            if ( tileTextures.ContainsKey( pos ) )
            {
                tex = textureMap[ tileTextures[pos] ];
            }
            else
            {
                tex = brokenTexture;
                //tex = new Texture2D(graphics.GraphicsDevice, 1, 1);
                //Color[] pixels = new Color[1];
                //pixels[0] = TileColourMap[pos];// * Noise.GetNoise(pos.X, pos.Y);
                //tex.SetData(pixels);
            }
            float scale = MapHolder.PIXELS_PER_TILE / tex.Width;
            spriteBatch.Draw(tex, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }
        private string GetTileTextPathAt( Vector2 pos, Color terrainType )
        {
            string result = "";
            foreach ( eTilePaintFace d in Enum.GetValues(typeof(eTilePaintFace))) // up down left right
            {
                Vector2 neighbour = pos.GetNeighbour(d);
                if (TileColourMap.ContainsKey(neighbour) && TileColourMap[neighbour] == terrainType)
                {
                    result += d.ToString() + "-";
                }
            }
            if (result != "")
            {
                result = RootPath + result.Substring(0, result.Length - 1);
                result += ".png";
            }
            if ( textureMap.ContainsKey( result ))
                return result;
            return RootPath + "default.png";
        }
    }
}
