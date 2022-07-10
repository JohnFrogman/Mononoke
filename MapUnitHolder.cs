using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
namespace Mononoke
{
    class MapUnitHolder
    {
        Dictionary<Vector2, MapUnit> Units; // Tile coordinate, not world pos. Dictionary as only one unit may occupy a tile.
        Pathfinder Pathfinder;
        List<(Vector2, MapUnit)> DeathQueue;
        public MapUnitHolder( )
        {
            Units = new Dictionary<Vector2, MapUnit>();
            DeathQueue = new List<(Vector2, MapUnit)>();
        }
        public void Initialise( Pathfinder pathfinder )
        {
            Pathfinder = pathfinder;
        }
        public void AddUnit( Vector2 pos, MapUnit unit )
        { 
            if ( Units.ContainsKey(pos) )
            {
                throw new Exception("Can't add a unit where a unit already exists!");
            }
            else
            {
                Units.Add( pos, unit );
            }
        }
        public void Update( GameTime gameTime )
        {
            // Need to update each unit. only update if the next step of their journey is free. If it's not then recalculate path.
            List<Vector2 > tiles = new List<Vector2>( Units.Keys );
            foreach ( Vector2 v in tiles )
            {
                MapUnit u = Units[v];
                if ( u.Stationary() )
                    continue;
                if ( Units.ContainsKey(u.Destination() ) )
                {
                    Debug.WriteLine( "Recalculating path, unit blocking destination");
                    u.SetPath( Pathfinder.GetPath( v, u.UltimateDestination(), true ) );
                }
                if ( u.MoveUpdate( gameTime ) )
                {
                    u.CompleteMove();
                    Units[ u.Location() ] = u;
                    Units.Remove( v );
                }
            }
            // Foreach unit - if attack isn't ready, do an update. Otherwise do an attack
            // When doing an attack: if there's a target already, attack that.
            // if not then aquire a target.
            // If the attack kills the target, queue the target for death.
            foreach ( KeyValuePair<Vector2, MapUnit> kvp in Units )
            { 
                if ( kvp.Value.AttackUpdate( gameTime ) ) // Should return true if attack is ready. 
                {
                    if ( ValidTarget(kvp.Key, kvp.Value.Target.Item2 ) ) // If it's a valid target, or possible to retarget, attack
                    {
                        kvp.Value.Target.Item2.Health -= kvp.Value.Attack.Damage;
                        if ( kvp.Value.Target.Item2.Health < 1 )
                        { 
                            QueueToDie( (kvp.Value.Target.Item1, kvp.Value.Target.Item2) );
                        }
                    }
                }
            }
            ExecuteDeathQueue();
        }
        bool ValidTarget( Vector2 pos, MapUnit target )
        {
            // if can find unit by it
            List<Vector2> neighbours = pos.GetNeighbours();
            foreach (Vector2 n in neighbours)
            {
                if ( Units.ContainsKey(n) && Units[n] == target )
                    return true;
            }
            foreach (Vector2 n in neighbours)
            {
                if ( Units.ContainsKey(n) && Units[n].Owner != Units[pos].Owner )
                {
                    Units[pos].Target = ( n, Units[ n ] );
                    return true;
                }
            }
            return false;

        }
        void QueueToDie( (Vector2, MapUnit) u )
        { 
            DeathQueue.Add( u );
        }
        void ExecuteDeathQueue()
        {
            if ( DeathQueue.Count > 0)
            {
                foreach ( (Vector2, MapUnit) v in DeathQueue )
                {
                    //Units[v].Die();
                    Units.Remove( v.Item1 );
                }
                DeathQueue.Clear();
            }

        }
        public void Draw( SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<Vector2, MapUnit> u in Units)
            {
                u.Value.Draw( spriteBatch, u.Key );
                //Pathfinder.DrawPathPreview( u.Value.Path, spriteBatch );
            }
        }
        public bool TryGetUnitAt( Vector2 pos, out MapUnit unit )
        {
            unit = null;
            if ( UnitExistsAt( pos ) )
            {
                unit = Units[pos];
                return true;
            }
            else 
                return false;

        }
        public bool UnitExistsAt( Vector2 pos )
        {
            return Units.ContainsKey(pos);
        }
    }
}
