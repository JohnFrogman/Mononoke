using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    class ProvinceHolder
    {
        List<Province> Provinces;
        Dictionary<Color, Province> ProvinceMap;
        public ProvinceHolder( ActorHolder actorHolder, MapHolder maps )
        {
            ProvinceMap = new Dictionary<Color, Province>();
            Provinces = new List<Province>();
            New( actorHolder, maps );
        }
        public void New( ActorHolder actorHolder, MapHolder maps )
        {
            Load(  "data/json/provinces_default.json", actorHolder, maps );
        }
        public void Load( string path, ActorHolder actorHolder, MapHolder maps )
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
                Province p = Province.FromJson(i, actorHolder );
                Provinces.Add( p );
                ProvinceMap.Add( p.Colour, p);
            }
        }
    }
}
