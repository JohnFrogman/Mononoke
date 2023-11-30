using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using Myra.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myra.Graphics2D.TextureAtlases;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace Mononoke
{    
    internal class InventoryManager
    {
        public bool Active = false;
        Desktop mDesktop;
        Panel mMainPanel;
        Dictionary<Inventory, InventoryGUI> mInventoryMap = new();
        Inventory mActiveInventory;
        Vector2Int HighlightPos = Vector2Int.Zero;
        InventoryItem mHeldItem = null;
        ItemPanel mHeldItemPanel;
        public InventoryManager(Desktop desktop, Panel mainPanel)
        {
            mDesktop = desktop;
            mMainPanel = mainPanel;
        }
        public void ToggleInventory(Inventory inv, Point pos) // pos where it appears on on the screen, nothing to do with the item slot map
        {
            if (mInventoryMap.ContainsKey(inv))
            {
                mInventoryMap[inv].Delete();
                mInventoryMap.Remove(inv);
                //mDesktop.HideContextMenu();
                SwitchInventory(true);
                Active = mInventoryMap.Count > 0;
                return;
            }
            mInventoryMap.Add(inv, new InventoryGUI(inv, mMainPanel, mDesktop));
            if (mInventoryMap.Count == 1)
            {
                mActiveInventory = inv;
                HighlightPos = Vector2Int.Zero;
                mInventoryMap[inv].Highlight(HighlightPos);
            }
            //mDesktop.ShowContextMenu(mInventoryMap[inv].Container, pos);

            Active = true;
        }
        public void SwitchInventory(bool forward)
        {
            //if (mInventoryMap.Count < 2) return;
            //int index = (forward ? 1 : -1) + mInventoryMap.IndexOf(mActiveInventory);
            //if (index < 0)
            //    index = mInventoryMap.Count - 1;
            //if (index >= mInventoryMap.Count)
            //    index = 0;
            //mActiveInventory = mInventoryMap[index];

        }
        public void HideInventory()
        {
            //inventoryGrid.Enabled = false;
            //inventoryGrid.Visible = false;
            //inventoryGrid.Widgets.Clear();
        }
        public void InventoryMove(Vector2Int v)
        {
            mInventoryMap[mActiveInventory].Unhighlight(HighlightPos);
            HighlightPos += v;
            if (HighlightPos.X < 0)
                HighlightPos.X = mActiveInventory.ItemMap.GetLength(0) - 1;
            if (HighlightPos.X >= mActiveInventory.ItemMap.GetLength(0))
                HighlightPos.X = 0;

            if (HighlightPos.Y < 0)
                HighlightPos.Y = mActiveInventory.ItemMap.GetLength(1) - 1;
            if (HighlightPos.Y >= mActiveInventory.ItemMap.GetLength(1))
                HighlightPos.Y = 0;

            mHeldItemPanel.SetPosition(HighlightPos);
            mInventoryMap[mActiveInventory].Highlight(HighlightPos);
        }
        public void OnInventorySelect()
        {
            InventoryItem highlightedItem = mActiveInventory.ItemMap[HighlightPos.X, HighlightPos.Y];
            if (mHeldItem == null)
            {
                mInventoryMap[mActiveInventory].GrabItem(HighlightPos);
                mHeldItem = highlightedItem;
                ShowHeldItemPanel();                
            }
            if (mHeldItem != null)
            {
                // Need to check if it fits, then if there are any items in the slot.
                // If no items in place then just place it
                // If there's a single item where the held item would be placed then swap with it
                // If more than one it can't be placed
                if (CanPlace(mActiveInventory, mHeldItem, HighlightPos))
                {
                    mActiveInventory.ItemMap[HighlightPos.X, HighlightPos.Y] = mHeldItem;
                    mHeldItem = null;
                }
                //if (CanSwap(mActiveInventory, mHeldItem, HighlightPos))
                //{
                //    InventoryItem i = mHeldItem;

                //}
            }

        }
        bool CanPlace(Inventory inv, InventoryItem item, Vector2Int pos)
        {
            int otherItemCount = 0;
            foreach (Vector2Int v in item.Occupies)
            {
                if (inv.ItemMap[v.X + pos.X, v.Y + pos.Y] != null)
                    otherItemCount++;
                if (otherItemCount > 1)
                    return false;
            }
            return true;
        }
        void ShowHeldItemPanel()
        {
            mHeldItemPanel = new ItemPanel(0, 0, mHeldItem);
        }
    }
}
