using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
namespace Mononoke
{
    class ProvinceHolder
    {
        List<Province> Provinces;
        Dictionary<Color, Province> ProvinceMap;
        public ProvinceHolder()
        {
            ProvinceMap = new Dictionary<Color, Province>();
            Provinces = new List<Province>();
            New();
        }
        public void New()
        {
            Load(  "/data/maps/provinces_default.json" );
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
                Province p = Province.FromJson(i);
                Provinces.Add( p );
                ProvinceMap.Add( p.Colour, p);
            }
        }
            public void Update( GameTime gameTime )
        {
            foreach ( Province p in Provinces )
            {
                p.Update ( gameTime );
            }
        }
        
    }
}
