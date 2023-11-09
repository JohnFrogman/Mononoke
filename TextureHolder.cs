using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;

namespace Mononoke
{
    class TextureHolder
    {
        Dictionary<string, Texture2D> Textures = new();
        Texture2D BrokenTexture;
        public TextureHolder( GraphicsDeviceManager graphics, string directory, Texture2D broken )
        {
            BrokenTexture = broken;
            if ( !Directory.Exists( directory ) )
            {
                throw new Exception("Directory does not exist! " + directory);
            }
            string[] files = Directory.GetFiles( directory );
            foreach (string f in files)
            {
                if ( ".png" == Path.GetExtension(f) || ".jpg" == Path.GetExtension(f))
                { 
                    Textures.Add( f.Substring( directory.Length , f.Length - directory.Length - 4 ) , Texture2D.FromFile(graphics.GraphicsDevice, f) );
                }
            }
        }
        public Texture2D GetTextureByName( string name )
        {
            if (Textures.ContainsKey( name ) )
            { 
                return Textures[name];
            }
            else
            {
                return BrokenTexture;
            }
        }
    }
}
