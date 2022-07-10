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
    class Farm : ResourceExtractor
    {
        
        public Farm( Vector2 pos ) : base ( pos )
        {
            AllowedTerrains = new eTerrainType[] { eTerrainType.Forest };
        }
        protected override void Harvest(Player clicker, int amount)
        {
            clicker.Food += amount;
        }
        protected override eTerrainType GetExpansionType()
        {
            return eTerrainType.Farmland;
        }
    }
}
