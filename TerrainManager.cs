using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.Text.Json.Nodes;

namespace Mononoke
{
    class DoodadData
    {
        public readonly DoodadTemplate Template;
        public readonly float Threshold;
        public DoodadData(DoodadTemplate t, float theshold)
        {
            Template = t;
            Threshold = theshold;
        }
    }
    class TileTypeData
    {
        public readonly string Name;
        public readonly List<DoodadData> Doodads;
        public readonly Texture2D FloorTexture;
        public readonly Color Colour;
        public TileTypeData(string name, List<DoodadData> doodadTemplates, Texture2D floorTexture, Color colour)
        {
            Name = name;
            Doodads = doodadTemplates; 
            FloorTexture = floorTexture;
            Colour = colour;
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
        Dictionary<Color, TileTypeData> TerrainTypeColourMap = new();
        Dictionary<string, TileTypeData> TerrainTypeNameMap = new();
        //eTerrainTileType[,] TileTypeMap;
        public TerrainManager(Camera2D camera) 
        {
            LoadTerrainTypes();
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
        void LoadTerrainTypes()
        {
            JsonDocument json = JsonDocument.Parse(File.ReadAllText("data/map/terrain_types.json"));
            var types = json.RootElement.EnumerateObject();

            foreach (var e in types)
            {
                string name = e.Name;
                List<DoodadData> doodads = new List<DoodadData>();
                var doodadElements = e.Value.GetProperty("doodads").EnumerateArray();
                foreach (var d in doodadElements)
                {
                    doodads.Add(new DoodadData(DoodadAssetManager.GetDoodadTemplate(d.GetProperty("name").ToString()), (float)d.GetProperty("threshold").GetDouble()));
                }
                TileTypeData tt = new TileTypeData(
                    name,
                    doodads,
                    TextureAssetManager.GetTerrainTileByName(e.Value.GetProperty("floor_texture").GetString()),
                    e.Value.GetProperty("colour").GetColor());
                TerrainTypeColourMap.Add(tt.Colour, tt);
                TerrainTypeNameMap.Add(tt.Name, tt);
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
