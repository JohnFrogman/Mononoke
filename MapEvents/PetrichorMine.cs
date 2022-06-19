using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mononoke.MapEvents
{
    class PetrichorMine : ResourceExtractor
    {
        public PetrichorMine( Vector2 pos ) : base ( pos )
        { 
            AllowedTerrains = new eTerrainType[] { eTerrainType.Forest };
        }
        protected override void Harvest( Player clicker, int amount)
        {
            clicker.Eider += amount;
        }
    }
}
