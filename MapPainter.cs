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
        Dictionary<Vector2, Color> TileColourMap;
        public MapPainter( string path, GraphicsDeviceManager graphics )
        {
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
        }
        public void Draw( SpriteBatch spriteBatch)
        {
            spriteBatch.Draw( texture, new Vector2(0,0), null, Color.White, 0, new Vector2 (0,0), MapHolder.PIXELS_PER_TILE, SpriteEffects.None, 0f );
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
    }
}
