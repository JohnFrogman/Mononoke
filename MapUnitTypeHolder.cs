using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    public class MapUnitInfo
    {
        public float TimeToMove { get; set; }
        public string SpriteID { get; set; }
        public int Health { get; set; }
        public int ZoneOfControl { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public int AttackPeriod { get; set; }
        public string Name { get; set; }
    }
    class MapUnitTypeHolder
    {
        Dictionary<string, MapUnitInfo> Units;
        public MapUnitTypeHolder( )
        {
            string path = "data/json/unit_manifest.json";
            if (!File.Exists(path))
            {
                throw new Exception("This province file does not exist " + path);
            }
            JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
            JsonElement e = doc.RootElement;
            JsonElement.ObjectEnumerator itr = e.EnumerateObject();
            Units = new Dictionary<string, MapUnitInfo>();
            foreach (JsonProperty i in itr)
            {
                MapUnitInfo inf = JsonSerializer.Deserialize<MapUnitInfo>(i.Value.ToString());
                if ( inf == null )
                    throw new Exception( "This unit type is not valid " + i.Name );
                Units.Add( i.Name, inf ); //UnitInfo.FromJson(i);
            }
        }
        public MapUnit BuildUnitByTypeName(string name, Vector2 pos, Actor owner )
        {
            if (Units.ContainsKey(name))
            {
                //UnitInfo inf = Units[name];
                return new MapUnit( name, Units[name], pos, owner )  ;
            }
            else
            {
                throw new Exception("This is not a valid unit " + name );
            }
        }
    }
}
