using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.Json;

namespace Mononoke
{
    public abstract class Actor
    {
        public const string NAME_PROPERTY_STR = "Name";
        public const string COLOUR_PROPERTY_STR = "Colour";
        public const string PERSONALITY_PROPERTY_STR = "Personality";
        public Color Colour;
        public string Name;
        public Actor( string name, Color colour )
        {
            Colour = colour;
            Name = name;
        }
        public static Actor FromJson( JsonElement json )
        {

            eActorBehavior b = eActorBehavior.Raider;
            //if (json. Enum.TryParse( json.GetProperty(PERSONALITY_PROPERTY_STR).ToString(), out b ) )
            //{
            //    if ( b == eActorBehavior.Player )
            //    {
            //        throw new System.Exception("Implement me!!!");
            //        //return new Player()
            //        //Player a = new Player( 
            //        //    json.GetProperty( NAME_PROPERTY_STR ).GetString()
            //        //    ,json.GetProperty( COLOUR_PROPERTY_STR ).GetColor() 
            //        //);
            //        //return a;
            //    }
            //    else
            //    {
                    NonPlayerActor a = new NonPlayerActor(
                        json.GetProperty(NAME_PROPERTY_STR).GetString()
                        , json.GetProperty(COLOUR_PROPERTY_STR).GetColor()
                        , b
                    );
                    return a;
            //    }
            //}
            //else
            //{
            //    throw new System.Exception("Not a valid behavior");
            //    //return null;
            //}
        }
    }
}
