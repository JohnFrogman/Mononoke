using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke.MapEvents
{
    abstract class ExpandableMapEvent : MapEvent
    {
        protected int Level = 1;
        protected static eTerrainType[] AllowedTerrains;
        protected abstract eTerrainType GetExpansionType();

        public ExpandableMapEvent(Vector2 pos) : base( pos )
        {
        }
        public virtual bool TryExpand( MapHolder mh )
        {
            foreach (Vector2 v in Tiles)
            {
                List<Vector2> neighbours = v.GetNeighbours();
                foreach (Vector2 n in neighbours)
                {
                    eTerrainType tt = mh.GetTerrainAt(n);
                    if (Array.IndexOf(AllowedTerrains, tt) > -1)
                    {
                        Tiles.Add(n);
                        Level++;
                        mh.SetTerrainAt(new List<Vector2>() { n }, GetExpansionType());
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
