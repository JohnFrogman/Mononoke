using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Mononoke.MapEvents
{
    public abstract class ResourceExtractor : MapEvent, IExpandable
    {
        protected static eTerrainType[] AllowedTerrains;
        int Max = 5;
        int _Current = 0;
        int Increment = 1;
        int Level = 1;
        int Current
        {
            set
            {
                if (value > Max * Level)
                    _Current = Max * Level;
                else
                    _Current = value;
            }
            get
            {
                return _Current;
            }
        }
        public ResourceExtractor(Vector2 pos) : base(pos)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Mononoke.Font, Current.ToString(), Origin * MapHolder.PIXELS_PER_TILE, Color.Black);
        }
        public override void OnClick(Player clicker)
        {
            int harvestAmount = 1;
            if (Current < harvestAmount)
            {
                Harvest ( clicker, Current );
                Current = 0;
            }
            else
            {
                Current -= harvestAmount;
                Harvest( clicker, harvestAmount );
            }
        }
        protected abstract void Harvest( Player clicker, int amount );
        protected override void OnExpire()
        {
            Current += Increment * Level;
        }
        //public abstract bool TryLink(MapEvent partner, MapHolder maps);

        // At some point make is so that potential expansion points are kept up to date automatically so dont need to search every time?
        bool IExpandable.TryExpand(MapHolder mh)
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
                        Debug.WriteLine( "Expanding farm ");
                        mh.SetTerrainAt(new List<Vector2>(){ n }, eTerrainType.Farmland);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
