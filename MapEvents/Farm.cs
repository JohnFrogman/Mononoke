using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Mononoke.MapEvents;
using System.Linq;

namespace Mononoke.MapEvents
{
    public class Farm : MapEvent, IDraggable, IExpandable
    {
        private static eTerrainType[] AllowedTerrains = new eTerrainType[] { eTerrainType.Forest };
        int Max = 5;
        int _Current = 0;
        int Increment = 1;
        int Level = 1;
        int Current{ 
            set {
                if ( value > Max * Level )
                    _Current = Max * Level ;
                else
                    _Current = value;
            }
            get
            {
                return _Current;
            }
        }
        public Farm( Vector2 pos ) : base ( pos )
        {
        }
        public override void Draw( SpriteBatch spriteBatch )
        {
            spriteBatch.DrawString( Mononoke.Font, Current.ToString(), Origin * MapHolder.PIXELS_PER_TILE, Color.Black );
        }   
        public override void OnClick( Player clicker )
        {
            int harvestAmount = 1;
            if ( Current < harvestAmount )
            {
                clicker.Food += Current;
                Current = 0;
            }
            else
            {
                Current -= harvestAmount;
                clicker.Food += harvestAmount;
            }
        }
        protected override void OnExpire()
        {
            Current += Increment * Level;
        }
        public override bool TryLink(MapEvent partner)
        {
            Debug.WriteLine("trying link of farm to " + partner );
            return true;
        }
        // At some point make is so that potential expansion points are kept up to date automatically so dont need to search every time?
        bool TryExpand( MapHolder mh )
        {
            foreach ( Vector2 v in Tiles )
            {
                List<Vector2> neighbours = v.GetNeighbours();
                foreach ( Vector2 n in neighbours )
                {
                    eTerrainType tt = mh.GetTerrainAt( n );
                    if ( Array.IndexOf(AllowedTerrains, tt) > -1 )
                    { 
                        Tiles.Add( n );
                        Level++;
                        mh.SetTerrainAt( n, eTerrainType.Farmland );
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
