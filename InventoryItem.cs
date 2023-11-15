using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace Mononoke
{
    internal class InventoryItem
    {
        public List<Vector2Int> Occupies;
        public string Name;
        public InventoryItem(string name, List<Vector2Int> occupies)
        {
            Name = name;
            Occupies = occupies;
            if (!Occupies.Contains(Vector2Int.Zero) || Occupies.Count == 0) { Occupies.Add(Vector2Int.Zero); }
        }
    }
}
