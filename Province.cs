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
        Color Owner;
        List<ProvinceResourceSpawn> ResourceSpawns;
        List<ProvinceResource> Resources
        Province( string name, Color color, Color owner )
        {
            Name = name;
            Colour = color;
            Owner = owner;
        }
        public static Province FromJson( JsonElement json )
        {
            return new Province ( 
                json.GetProperty( NAME_PROPERTY_STR ).GetString()
                ,json.GetProperty( COLOUR_PROPERTY_STR ).GetColor() 
                ,json.GetProperty( OWNER_PROPERTY_STR ).GetColor()
            );
        }
        public void SpawnSupply( eProvinceResourceType type, Vector2 pos )
        {
        }
        public void SpawnDemand()
        {
        }
        public void Update( GameTime gameTime )
        {
            
        }
    }
}
