using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    class ProvinceResourceSpawn
    {
        float CurrentSecond = 0;
        float SecondsToSpawn = 10f;
        public Dictionary<Vector2, bool> SpawnPoints;
        Province Province;
        bool Supply;
        eProvinceResourceType Type;
        public ProvinceResourceSpawn( eProvinceResourceType type, bool supply, Province p )
        {
            Supply = supply;
            Type = type;
            SpawnPoints = new Dictionary<Vector2, bool>();
            Province = p;
        }
        public void Update( GameTime gameTime  )
        {
            CurrentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;    
            if ( CurrentSecond > SecondsToSpawn )
            {
                Vector2 pos;
                if ( TryGetSpawnPosition ( out pos ) )
                {
                    if ( Supply )
                        Province.SpawnSupply( Type, pos );
                    //else
                        //Province.SpawnDemand( Type, pos );
                }
                CurrentSecond = 0;
            }
        }
        bool TryGetSpawnPosition( out Vector2 pos)
        {
            //List<Vector2> SpawnPoints = SpawnPoints.Keys.Shuffle();
            foreach ( Vector2 p in SpawnPoints.Keys )
            {
                if ( !SpawnPoints[p] )
                {
                    pos = p;
                    return true;
                }
            }
            pos = new Vector2();
            return false;
        }

        public void AddSpawnPos( Vector2 pos )
        {
            if ( SpawnPoints.ContainsKey( pos ))    
            {
                throw new Exception("Trying to add a duplicate spawn position " + pos + " " + Type );
            }
            else
            {
                SpawnPoints.Add( pos, false ) ;
            }
        }
    }
}
