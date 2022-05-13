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
        List<ProvinceResource> Resources;
        Province( string name, Color color )
        {
            Name = name;
            Colour = color;
            Resources = new List<ProvinceResource>();
            //Resources.Add( new ProvinceResource(eProvinceResourceType.Food, this, new Vector2(0,0)));
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
        public void Update( GameTime gameTime )
        {
            foreach ( ProvinceResource r in Resources )
                r.Update( gameTime );
        }
        public void Draw( SpriteBatch spriteBatch )
        {
            foreach ( ProvinceResource r in Resources )
                r.Draw( spriteBatch );
        }
        public void AddResourceAt( eProvinceResourceType res, Vector2 pos )
        {
            Resources.Add( new ProvinceResource( res, this, pos ) );
        }
    }
}
