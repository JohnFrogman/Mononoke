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
        public ProvinceHolder( ActorHolder actorHolder )
        {
            ProvinceMap = new Dictionary<Color, Province>();
            Provinces = new List<Province>();
            New( actorHolder );
        }
        public void New( ActorHolder actorHolder )
        {
            Load(  "data/json/provinces_default.json", actorHolder );
        }
        public void Load( string path, ActorHolder actorHolder )
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
        
    }
}
