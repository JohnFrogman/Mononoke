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
                if ( Units.ContainsKey(u.Destination() ) ) // Recalculate if next step is occupied
                {
                    u.SetPath( Pathfinder.GetPath( v, u.UltimateDestination(), true ) );
                    if (u.Stationary() ) // Recalculating could lead to the unit being stationary if it's ultimate destination is now occupied or it can't find a path
                        continue; 
                } 

                if ( u.MoveUpdate( gameTime ) )
                {
                    u.CompleteMove();
                    Units[ u.Location ] = u;
                    Units.Remove( v );
                }
            }
            // Foreach unit - if attack is ready, do an attack.
            // When doing an attack: if there's a target already, attack that.
            // if not then aquire a target.
            // If the attack kills the target, queue the target for death.
            foreach ( KeyValuePair<Vector2, MapUnit> kvp in Units )
            { 
                if ( kvp.Value.AttackUpdate( gameTime ) ) // Should return true if attack is ready. 
                {
                    if ( ValidTarget( kvp.Value ) ) // If it's a valid target, or possible to retarget, attack
                    {
                        Debug.WriteLine( "Aqcuired target and attacking" );
                        kvp.Value.Attack.Ready = false;
                        kvp.Value.Target.Health -= kvp.Value.Attack.Damage;
                        if ( kvp.Value.Target.Health < 1 )
                        { 
                            QueueToDie( (kvp.Value.Target.Location, kvp.Value.Target) );
                        }
                    }
                }
            }
            ExecuteDeathQueue();
        }
        bool ValidTarget( MapUnit attacker )
        {
            //if ( )
            // if can find unit by it
            //if ( target != null )
            //return true;
            //List<Vector2> potentialTargets = attacker.Location.GetNeighbours();
            List<Vector2> potentialTargets = attacker.Location.GetTilesInRange( attacker.Attack.Range );
            MapUnit provisionalTarget = null;
            foreach (Vector2 n in potentialTargets) // Right now a range of 1, this should in future get all tiles in a range.
            {
                if ( Units.ContainsKey(n)  )
                { 
                    if ( Units[n] == attacker.Target )
                    { 
                        return true; // Already has a valid target
                    }
                    else
                    { 
                        //if ( ) check unit is hostile to
                        provisionalTarget = Units[n];
                    }
                }
            }
            attacker.Target = provisionalTarget;
            return provisionalTarget != null;
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
                    Debug.WriteLine("unit at " + v.Item1 + " is dead");
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
                u.Value.Draw( spriteBatch );
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
