using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    class LintExtractor : ResourceExtractor
    {
        public LintExtractor(Vector2 pos, Actor owner) : base(pos, owner)
        {
            AllowedTerrains = new eTerrainType[] { eTerrainType.Forest };
        }
        protected override void Harvest(Player clicker, int amount)
        {
            clicker.Lint += amount;
        }
        protected override eTerrainType GetExpansionType()
        {
            return eTerrainType.LintExtractor;
        }
        protected override string GetChildJson()
        {
            return ",\"Type\" : \"LintExtractor\" ";
        }
    }
}