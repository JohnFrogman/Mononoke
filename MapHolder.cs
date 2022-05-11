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
        public const int PIXELS_PER_TILE = 32;
        MapPainter TerrainPainter;
        //MapPainter ProvincePainter;
        public MapHolder( GraphicsDeviceManager graphics )        
        {
            TerrainPainter = new MapPainter( "data/maps/test_terrain.png", graphics );
            //ProvincePainter = new MapPainter("data/maps/provinces_default.png", graphics ); 
            
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos )
        {
            TerrainPainter.Draw( spriteBatch, graphics, pos );
        }
        public void GetTerrainColourAt(Vector2 pos )
        {
            Debug.WriteLine( TerrainPainter.GetColourAt(pos) );
        }
    }
}
