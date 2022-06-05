using System;           
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Mononoke
{
    class MapPainter
    {
        Texture2D texture;
        protected Texture2D brokenTexture;
        public Dictionary<Vector2, Color> TileColourMap { get; private set; }
        Texture2D TestTexture;
        public MapPainter( string path, GraphicsDeviceManager graphics )
        {
            string str = "data/textures/forest.png";
            if ( !File.Exists( str ) )
            {
                throw new Exception( "Map does not exist at this path " + str );
            }
            TestTexture = Texture2D.FromFile( graphics.GraphicsDevice, str );
            
            //Noise = new FastNoiseLite();
            //Noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

            if ( !File.Exists( path ) )
            {
                throw new Exception( "Map does not exist at this path " + path );
            }
            texture = Texture2D.FromFile( graphics.GraphicsDevice, path );
            
            TileColourMap = new Dictionary<Vector2, Color>();
            Color[] colors = new Color[ texture.Width * texture.Height];
            texture.GetData(colors);
            int i = 0;
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    TileColourMap.Add( new Vector2( x, y), colors[i] );
                    ++i;
                }
            }
            brokenTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Color[] pixels = new Color[1];
            pixels[0] = Color.Magenta;
            brokenTexture.SetData(pixels);
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 origin )
        {
            spriteBatch.Draw( texture, new Vector2(0,0), null, new Color(255,255,255, 255), 0, new Vector2 (0,0), MapHolder.PIXELS_PER_TILE, SpriteEffects.None, 0f );
            for (int x = 0; x < MapHolder.SCREEN_TILE_WIDTH; x++)
            {
                for (int y = 0; y < MapHolder.SCREEN_TILE_HEIGHT; y++)
                {
                    Vector2 pos = origin + new Vector2(x,y);
                    if (TileColourMap.ContainsKey(pos))
                    {
                        DrawTile( spriteBatch, graphics, pos );
                    }
                    else
                    {
                    }
                }
            }
        }
        public Color GetColourAt( Vector2 pos )
        {
            Color result = new Color();
            if ( TileColourMap.TryGetValue( pos, out result ) )
            {
                return result;
            }
            return Color.Magenta;
        }
        public void SetColourAt( Vector2 pos, Color col )
        {
            TileColourMap[pos ] = col;
        }
        public virtual void DrawTile( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos )
        {
            Texture2D tex;  
            Color col = TileColourMap[pos];
            if (TerrainTypeMap.GetColourTerrainType(col) == eTerrainType.Forest)
            {
                //scale = 1f;
                tex = TestTexture;
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

            //Texture2D tex = new Texture2D(graphics.GraphicsDevice, MapHolder.PIXELS_PER_TILE, MapHolder.PIXELS_PER_TILE);
            //int size = MapHolder.PIXELS_PER_TILE * MapHolder.PIXELS_PER_TILE;
            //Color[] pixels = new Color[size];
            //for (int j = 0; j < size; j++)
            //{
            //    pixels[j] = TileColourMap[pos] * Noise.GetNoise(pos.X, pos.Y);
            //}


            //spriteBatch.Draw(tex, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), scale , SpriteEffects.None, 0f);
        }
    }
}
