using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mononoke.MapEvents;

namespace Mononoke
{
    class ProvinceHolder
    {
        List<Province> Provinces;
        Dictionary<Color, Province> ProvinceMap;
        Dictionary<Vector2, Province> ProvinceEventPoints;
        public ProvinceHolder( ActorHolder actorHolder, MapHolder maps )
        {
            ProvinceEventPoints = new Dictionary<Vector2, Province>();
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
            
            Dictionary<Color, List<Tuple<Vector2, MapEvent>>> resources = maps.GetProvinceMapEvents();
            foreach ( JsonElement i in itr )
            {
                Province p = Province.FromJson(i, actorHolder );
                if ( resources.ContainsKey(p.Colour))
                {
                    foreach ( Tuple<Vector2, MapEvent> r in resources[p.Colour] )
                    {
                        p.AddEventAt( r.Item1, r.Item2 );
                        ProvinceEventPoints.Add( r.Item1, p );
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
        public bool TryClickAt( Vector2 pos, Player clicker )
        {
            if (ProvinceEventPoints.ContainsKey( pos ) )
            {
                ProvinceEventPoints[pos].TileClick( pos, clicker);
                return true;
            }
            return false;
        }
        public bool TryDragAt(Vector2 origin, Vector2 destination, Player dragQueen )
        {
            if ( ProvinceEventPoints.ContainsKey(origin) && ProvinceEventPoints.ContainsKey( destination ) )
            {
                MapEvent ev1 = ProvinceEventPoints[origin].GetEventAt( origin );
                MapEvent ev2 = ProvinceEventPoints[origin].GetEventAt( destination );
                ev1.TryLink(ev2);
            }
            return false;
        }
        public bool IsDraggable( Vector2 pos )
        {
            return ProvinceEventPoints.ContainsKey( pos ) && ProvinceEventPoints[pos].GetEventAt(pos) is IDraggable;
        }
    }
}
