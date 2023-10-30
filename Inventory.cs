using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal class Inventory
    {
        public InventoryItem[,] ItemMap; 
        public Inventory(int width, int height) 
        {
            ItemMap = new InventoryItem[width, height];
        }
    }
}
