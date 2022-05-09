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
        public const int PIXELS_PER_TILE = 1;
        MapPainter TerrainPainter;
        MapPainter ProvincePainter;
        public MapHolder( GraphicsDeviceManager graphics )        
        {
            TerrainPainter = new MapPainter( "data/maps/test.png", graphics );
            //ProvincePainter = new MapPainter("data/maps/provinces_default.png", graphics ); 
            
        }
        public void Draw( SpriteBatch spriteBatch)
        {
            TerrainPainter.Draw( spriteBatch );
        }
        public void GetTerrainColourAt(Vector2 pos )
        {
            Debug.WriteLine( TerrainPainter.GetColourAt(pos) );
        }
    }
}
