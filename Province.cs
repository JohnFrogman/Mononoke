using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json;

namespace Mononoke
{
    class Province
    {
        public const string NAME_PROPERTY_STR = "Name";
        public const string OWNER_PROPERTY_STR = "Owner";
        public const string COLOUR_PROPERTY_STR = "Colour";
        string Name;
        public Color Colour;
        Actor Owner;
        Province( string name, Color color )
        {
            Name = name;
            Colour = color;
        }
        public static Province FromJson( JsonElement json, ActorHolder actorHolder )
        {
            Province p = new Province ( 
                json.GetProperty( NAME_PROPERTY_STR ).GetString()
                ,json.GetProperty( COLOUR_PROPERTY_STR ).GetColor() 
            );
            if ( json.GetProperty( OWNER_PROPERTY_STR ).GetString() != "" )
            {
                p.SetOwner( actorHolder.GetActor ( json.GetProperty( OWNER_PROPERTY_STR ).GetColor() )  );
            }
            return p;
        }
        void SetOwner ( Actor actor )
        {
            Owner = actor;
        }
    }
}
