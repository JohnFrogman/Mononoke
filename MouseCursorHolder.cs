using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    class MouseCursorHolder
    {
        //Dictionary<string, Texture2D> Textures;
        Texture2D BrokenTexture;
        public Texture2D MoveCursor { private set; get; }
        public Texture2D DefaultCursor { private set; get; }
        public Texture2D SelectCursor { private set; get; }
        public Texture2D IllegalMoveCursor { private set; get; }
        public MouseCursorHolder(GraphicsDeviceManager graphics, Texture2D broken)
        {
            string path = "data/textures/cursor_sheet.png";
            BrokenTexture = broken; 
            int tileSize = 32;
            
            Texture2D spriteSheet = Texture2D.FromFile(graphics.GraphicsDevice, path);

            DefaultCursor = new Texture2D( graphics.GraphicsDevice, tileSize, tileSize );
            Point pos = new Point(0, 0);
            Rectangle r = new Rectangle(pos, new Point(tileSize, tileSize));
            Color[] data = new Color[tileSize * tileSize];
            spriteSheet.GetData(0, r, data, 0, data.Length);
            DefaultCursor.SetData(data);

            MoveCursor = new Texture2D(graphics.GraphicsDevice, tileSize, tileSize);
            pos = new Point(tileSize, 0);
            r = new Rectangle(pos, new Point(tileSize, tileSize));
            data = new Color[tileSize * tileSize];
            spriteSheet.GetData(0, r, data, 0, data.Length);
            MoveCursor.SetData(data);

            SelectCursor = new Texture2D(graphics.GraphicsDevice, tileSize, tileSize);
            pos = new Point(0, tileSize);
            r = new Rectangle(pos, new Point(tileSize, tileSize));
            data = new Color[tileSize * tileSize];
            spriteSheet.GetData(0, r, data, 0, data.Length);
            SelectCursor.SetData(data);

            IllegalMoveCursor = new Texture2D(graphics.GraphicsDevice, tileSize, tileSize);
            pos = new Point(tileSize, tileSize);
            r = new Rectangle(pos, new Point(tileSize, tileSize));
            data = new Color[tileSize * tileSize];
            spriteSheet.GetData(0, r, data, 0, data.Length);
            IllegalMoveCursor.SetData(data);
        }
    }
}
