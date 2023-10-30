using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal class TerrainManager
    {
        Camera2D mCamera;
        Texture2D[,] mTileGrid;
        int TileSize = 128;
        Vector2 Centre;
        int xTiles;
        int yTiles;
        Texture2D GroundTexture;
        //Vector2 CameraDisplacement;
        public TerrainManager(Camera2D camera) 
        {
            mCamera = camera;
            //mTileGrid = new Texture2D[1,1];
            GroundTexture = TextureAssetManager.GetTerrainTileByName("SandTile");
            TileSize = GroundTexture.Width;
            xTiles = 1 + ((1920 / GroundTexture.Width) );
            yTiles = 1 + ((1080 / GroundTexture.Height));
            Centre = Vector2.Zero;
        }
        public void Update(GameTime gameTime)
        { 
            Centre = -new Vector2((float)Math.Floor(mCamera.Position.X / TileSize), (float)Math.Floor(mCamera.Position.Y / TileSize));
        }
        public void Draw(SpriteBatch spriteBatch) 
        { 
            for (int i = 0; i < xTiles; ++i)
            {
                for (int j = 0; j < yTiles; ++j)
                {
                    Vector2 offset = new Vector2(i * TileSize, j * TileSize);
                    spriteBatch.Draw(GroundTexture, Centre + offset, Color.White);
                }
            }
        }
    }
}
