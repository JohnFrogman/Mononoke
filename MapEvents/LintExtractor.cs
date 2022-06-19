using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mononoke.MapEvents
{
    class LintExtractor : ResourceExtractor
    {
        public LintExtractor(Vector2 pos) : base(pos)
        {
            AllowedTerrains = new eTerrainType[] { eTerrainType.Forest };
        }
        protected override void Harvest(Player clicker, int amount)
        {
            clicker.Lint += amount;
        }
    }
}
