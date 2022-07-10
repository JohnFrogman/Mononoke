using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.Json;

namespace Mononoke
{
    public class Actor
    {
        public const string NAME_PROPERTY_STR = "Name";
        public const string COLOUR_PROPERTY_STR = "Colour";
        public Color Colour;
        public string Name;
        public Actor( string name, Color colour )
        {
            Colour = colour;
            Name = name;
        }
        public static Actor FromJson( JsonElement json )
        {
            Actor a = new Actor ( 
                json.GetProperty( NAME_PROPERTY_STR ).GetString()
                ,json.GetProperty( COLOUR_PROPERTY_STR ).GetColor() 
            );
            return a;
        }
    }
}
