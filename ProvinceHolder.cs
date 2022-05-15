using System;
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
        Dictionary<Vector2, Province> ProvinceResourcePoints;
        public ProvinceHolder( ActorHolder actorHolder, MapHolder maps )
        {
            ProvinceResourcePoints = new Dictionary<Vector2, Province>();
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
            
            Dictionary<Color, List<Tuple<eProvinceResourceType, Vector2>>> resources = maps.GetProvinceResources();
            foreach ( JsonElement i in itr )
            {
                Province p = Province.FromJson(i, actorHolder );
                if ( resources.ContainsKey(p.Colour))
                {
                    foreach ( Tuple<eProvinceResourceType, Vector2> r in resources[p.Colour] )
                    {
                        p.AddResourceAt( r.Item1, r.Item2 );
                        ProvinceResourcePoints.Add( r.Item2, p );
                    }
                }
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
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Vector2 pos )
        {
            foreach ( Province p in Provinces )
            {
                p.Draw ( spriteBatch );
            }
        }
        public void TryClickAt( Vector2 pos )
        {
            if ( ProvinceResourcePoints.ContainsKey( pos ) )
            {
                   
            }
        }
    }
}
