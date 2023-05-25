using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    class PetrichorMine : ResourceExtractor
    {
        public PetrichorMine( Vector2 pos, Actor owner ) : base ( pos, owner )
        { 
            AllowedTerrains = new eTerrainType[] { eTerrainType.Forest };
        }
        protected override void Harvest( Player clicker, int amount)
        {
            clicker.Petrichor += amount;
        }
        protected override eTerrainType GetExpansionType()
        {
            return eTerrainType.PetrichorMine;
        }
        protected override string GetChildJson()
        {
            return ",\"Type\" : \"PetrichorMine\" ";
        }
    }
}
