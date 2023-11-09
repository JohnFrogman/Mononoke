using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    class TerrainTile
    {
        Texture2D mTexture;
        public Vector2 mPosition;
        Vector2 mCentre;
        public TerrainTile(Texture2D texture, Vector2 position, Vector2 centre)
        {
            mTexture = texture;
            mPosition = position;
            mCentre = centre;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, null, Color.White, 0f, mCentre, 1f, SpriteEffects.None, 0f);
        }
    }
    internal class TerrainManager
    {
        Camera2D mCamera;
        TerrainTile[,] mTileGrid;
        //Vector2 CameraDisplacement;
        int TileSizeX;
        int TileSizeY;
        
        int XCoord = 0;
        int YCoord = 0;
        Vector2 Offset;
        public TerrainManager(Camera2D camera) 
        {
            mCamera = camera;
            //mTileGrid = new Texture2D[1,1];
            Texture2D tex = TextureAssetManager.GetTerrainTileByName("sand_tile_big");
            //Texture2D tex = TextureAssetManager.GetTerrainTileByName("SandTile");
            TileSizeX = tex.Width;
            TileSizeY = tex.Height;
            Offset = new Vector2(960, 540);
            mTileGrid = new TerrainTile[3,3];
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    mTileGrid[i, j] = new TerrainTile(tex, CoordsToTerrainPos(i,j), new Vector2(tex.Width/2f, tex.Height/2f));
                }
            }
        }
        Vector2 CoordsToTerrainPos(int x, int y)
        {
            return new Vector2(
                  TileSizeX * (x)
                , TileSizeY * (y)
                );
        }
        public void Update(GameTime gameTime)
        {
            //float div = mCamera.Position + Offset / TileSizeX;
            int newX = (int)Math.Floor((mCamera.Position.X - Offset.X) / TileSizeX);
            int newY = (int)Math.Floor((mCamera.Position.Y - Offset.Y) / TileSizeY);
             //= (mCamera.Position - Offset);
            //if (newX - XCoord != 0 || newY - YCoord != 0)
            //{
                for (int i = 0; i < mTileGrid.GetLength(0); ++i)
                {
                    for (int j = 0; j < mTileGrid.GetLength(1); ++j)
                    {
                        //Vector2 pos = new Vector2(TileSizeX * (i + XCoord), TileSizeY * (j + YCoord));
                        mTileGrid[i, j].mPosition = -CoordsToTerrainPos(newX + i, newY + j);
                        //mTileGrid[i, j].mPosition = TileSizeY * Vector2.UnitY * newY;
                    }
                }
            //}
            XCoord = newX;
            YCoord = newY;
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            for (int i = 0; i < mTileGrid.GetLength(0); ++i)
            {
                for (int j = 0; j < mTileGrid.GetLength(1); ++j)
                {
                    mTileGrid[i, j].Draw(spriteBatch);
                }
            } 
        }
    }
}
