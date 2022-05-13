using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Text.Json;
namespace Mononoke
{
    class ActorHolder
    {
        List<Actor> Actors;
        Dictionary<Color, Actor> ActorsByColour;

        public ActorHolder( )
        {
            ActorsByColour = new Dictionary<Color, Actor>();
            Actors = new List<Actor>();
            New( );
        }
        public void New( )
        {
            Load(  "data/json/actors_default.json" );
        }
        public void Load( string path )
        {
            if ( !File.Exists( path ) )
            {
                throw new Exception("This province file does not exist " + path);
            }
            JsonDocument doc = JsonDocument.Parse( File.ReadAllText( path ) );
            JsonElement e = doc.RootElement;
            JsonElement.ArrayEnumerator itr = e.EnumerateArray();
            
            foreach ( JsonElement i in itr )
            {
                Actor a = Actor.FromJson(i );
                Actors.Add( a );
                ActorsByColour.Add( a.Colour, a );
            }
        }

        public Actor GetActor( Color col)
        {
            //return new Actor("Test",);
            return ActorsByColour[col];
        }
    }
}
