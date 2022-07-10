using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Mononoke
{
    static class IconHolder
    {
        static Dictionary<string, Texture2D> Icons;
        static Texture2D BrokenTexture;
        static bool Initialised = false;
        public static void Initialise( GraphicsDeviceManager graphics )
        {
            //string str = "data/textures/gui/mainmenu/background.jpg";
            //if (!File.Exists(str))
            //{
            //    throw new Exception("Background does not exist at this path " + str);
            //}
            //BrokenTexture = Texture2D.FromFile(graphics.GraphicsDevice, str);

            Icons = new Dictionary<string, Texture2D>();
            string str = "data/textures/gui/icon.png";
            if (!File.Exists(str))
            {
                throw new Exception("Background does not exist at this path " + str);
            }
            BrokenTexture = Texture2D.FromFile(graphics.GraphicsDevice, str);

            //BrokenTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            //Color[] pixels = new Color[1];
            //pixels[0] = Color.Magenta;
            //BrokenTexture.SetData(pixels);
            Initialised = true;
        }
        public static Texture2D GetIconByName( string name )
        {
            if ( !Initialised )
                throw new Exception("Icon holder hasn't been initialised!");
            if ( Icons.ContainsKey( name ) )
                return Icons[name];
            else return BrokenTexture;
        }
    }
}
