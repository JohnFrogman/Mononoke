using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal class Inventory
    {
        public string Name;
        public InventoryItem[,] ItemMap; 
        public Inventory(string name, int width, int height) 
        {
            Name = name;  
            ItemMap = new InventoryItem[width, height];
        }
        public void AddItem(InventoryItem item, Vector2Int pos)
        {
            foreach (Vector2Int v in item.Occupies)
            {
                Vector2Int slot = v + pos;
                ItemMap[slot.X, slot.Y] = item;
            }
        }
        public InventoryItem TakeItem(Vector2Int pos) 
        {
            InventoryItem item = ItemMap[pos.X, pos.Y];
            if (item != null)
            {
                foreach ( Vector2Int v in item.Occupies )
                {
                    Vector2Int p = v + pos;
                    ItemMap[p.X, p.Y] = null;
                }
            }
            return item;
        }
        public bool CanPlace(InventoryItem item, Vector2Int pos)
        {
            foreach (Vector2Int v in item.Occupies)
            {
                Vector2Int p = v + pos;
                if (p.X >= ItemMap.GetLength(0) )
                    return false;
                if ( p.X < 0 || p.Y < 0 ) 
                    return false;
                if ( p.Y >=  ItemMap.GetLength(1) )
                    return false;
                if (ItemMap[p.X, p.Y] != null)
                    return false;
            }
            return true;
        }
    }
}
