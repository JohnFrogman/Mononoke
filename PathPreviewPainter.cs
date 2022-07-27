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
    class PathPreviewPainter
    {
        // string is what hte tile connects to, which is sorted.
        Dictionary<string, Texture2D> textureMap;

        public PathPreviewPainter( GraphicsDeviceManager graphics )
        {
            LoadTextures(graphics);
        }

        public void DrawPathPreview(SpriteBatch spriteBatch, List<Vector2> path, Vector2 Origin )
        {
            for (int i = 0; i < path.Count; i++)
            {
                Vector2 pos = path[i];
                string id = "";
                List<string> connectsToList = new List<string>();
                if (i != path.Count - 1)
                { 
                    connectsToList.Add( pos.GetNeighbourDirection( path[i+1] ).ToString() );
                }
                else
                {
                    connectsToList.Add(pos.GetNeighbourDirection( Origin ).ToString() );
                }

                if ( i != 0 )
                {
                    connectsToList.Add(pos.GetNeighbourDirection(path[i - 1]).ToString());
                    
                }
 
                connectsToList = connectsToList.OrderBy(s => s).ToList();
                foreach (string s in connectsToList)
                    id += s;

                spriteBatch.Draw( textureMap[id], pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }
        }
        private void LoadTextures( GraphicsDeviceManager graphics )
        {
            textureMap = new Dictionary<string, Texture2D>();

            string sheetPath = "data/textures/path_preview.png";

            if (!File.Exists(sheetPath))
            {
                throw new Exception("Sheet doesn't exist at this path! " + sheetPath);
            }
            Texture2D spriteSheet = Texture2D.FromFile(graphics.GraphicsDevice, sheetPath);

            LoadTexAt(graphics, spriteSheet, new Vector2(0, 0), "lr");
            LoadTexAt(graphics, spriteSheet, new Vector2(1, 0), "dr");
            LoadTexAt(graphics, spriteSheet, new Vector2(2, 0), "dl");
            LoadTexAt(graphics, spriteSheet, new Vector2(3, 0), "");

            LoadTexAt(graphics, spriteSheet, new Vector2(0, 1), "du");
            LoadTexAt(graphics, spriteSheet, new Vector2(1, 1), "ru");
            LoadTexAt(graphics, spriteSheet, new Vector2(2, 1), "lu");

            LoadTexAt(graphics, spriteSheet, new Vector2(0, 2), "u");
            LoadTexAt(graphics, spriteSheet, new Vector2(1, 2), "r");

            LoadTexAt(graphics, spriteSheet, new Vector2(0, 3), "l");
            LoadTexAt(graphics, spriteSheet, new Vector2(1, 3), "d");
        }
        private void LoadTexAt(GraphicsDeviceManager graphics, Texture2D sheet, Vector2 pos, string id )
        {
            Texture2D tex = new Texture2D(graphics.GraphicsDevice, MapHolder.PIXELS_PER_TILE, MapHolder.PIXELS_PER_TILE);
            pos *= MapHolder.PIXELS_PER_TILE;
            Point p = new Point( (int)pos.X, (int)pos.Y);
            Rectangle r = new Rectangle(p, new Point(MapHolder.PIXELS_PER_TILE, MapHolder.PIXELS_PER_TILE));
            Color[] data = new Color[MapHolder.PIXELS_PER_TILE * MapHolder.PIXELS_PER_TILE];
            sheet.GetData(0, r, data, 0, data.Length);
            tex.SetData(data);

            textureMap[id] = tex;
        }
    }
}
