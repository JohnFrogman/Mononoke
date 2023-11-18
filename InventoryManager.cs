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

namespace Mononoke
{
    class ItemPanel
    {
        public Texture2D mImage;
        public Panel mPanel;
        public ItemPanel(/*Texture2D image,*/ Panel panel)
        {
            //mImage = image;
            mPanel = panel;
        }
    }
    internal class InventoryManager
    {
        public bool Active = false;
        Desktop mDesktop;
        List<Inventory> mActiveInventories = new();
        Dictionary<Inventory, Dictionary<Vector2Int,Panel>> mInventoryGridMap = new();
        Inventory mActiveInventory;
        Vector2Int HighlightPos = Vector2Int.Zero;
        const int ITEM_BOX_SIZE = 32;
        readonly Color DEFAULT_ITEM_COL = Color.AntiqueWhite;
        readonly Color HIGHLIGHTED_ITEM_COL = Color.Pink;
        public InventoryManager(Desktop desktop)
        {
            mDesktop = desktop;
        }
        Grid BuildInventoryGrid(Inventory inv)
        {
            Dictionary<Vector2Int, Panel> map = new();
            Grid inventoryGrid = new Grid
            {
                //ShowGridLines = true,
                ColumnSpacing = 0,
                RowSpacing = 0 
            };
            inventoryGrid.Padding = new Thickness(0);
            inventoryGrid.Margin = new Thickness(0);
            inventoryGrid.Background = new SolidBrush(Color.Transparent);
            inventoryGrid.Width = inv.ItemMap.GetLength(0) * ITEM_BOX_SIZE;
            inventoryGrid.Height = inv.ItemMap.GetLength(1) * ITEM_BOX_SIZE;
            inventoryGrid.Enabled = true;
            inventoryGrid.Visible = true;
            for (int i = 0; i < inv.ItemMap.GetLength(0); i++)
            {
                for (int j = 0; j < inv.ItemMap.GetLength(1); j++)
                {
                    Panel aV = new Panel();
                    aV.Background = new SolidBrush(DEFAULT_ITEM_COL);
                    aV.Width = ITEM_BOX_SIZE;
                    aV.Height = ITEM_BOX_SIZE;
                    Image portrait = new Image();
                    Texture2D tex = TextureAssetManager.GetIconByName("petrichor");
                    portrait.Renderable = new TextureRegion(tex);
                    //portrait.Scale = new Vector2(ITEM_BOX_SIZE / tex.Width, ITEM_BOX_SIZE / tex.Height);
                    portrait.HorizontalAlignment = HorizontalAlignment.Center;
                    portrait.VerticalAlignment = VerticalAlignment.Center;
                    aV.AddChild(portrait);
                    aV.GridRow = j;
                    aV.GridColumn = i;
                    map.Add(new Vector2Int(i, j), aV);
                    inventoryGrid.AddChild(aV);
                }
            }
            mInventoryGridMap.Add(inv, map);
            return inventoryGrid;
        }
        public void ToggleInventory(Inventory inv, Point pos) // pos where it appears on on the screen, nothing to do with the item slot map
        {
            if (mActiveInventories.Contains(inv)) 
            {
                mInventoryGridMap[inv].First().Value.Parent.RemoveFromParent();
                mActiveInventories.Remove(inv);
                mInventoryGridMap.Remove(inv);
                SwitchInventory(true);
                Active = mActiveInventories.Count > 0;
                return;
            }
            mActiveInventories.Add(inv);
            Grid inventoryGrid = BuildInventoryGrid(inv);
            if (mActiveInventories.Count == 1) 
            { 
                mActiveInventory = inv; 
                HighlightPos = Vector2Int.Zero;
                mInventoryGridMap[mActiveInventory][HighlightPos].Background = new SolidBrush(HIGHLIGHTED_ITEM_COL);
            }
            VerticalStackPanel container = new VerticalStackPanel
            {
                Spacing = 4
            };
            container.AddChild(inventoryGrid);            
            mDesktop.ShowContextMenu(container, pos);
            Active = true;
        }
        //void ItemClick(InventoryItem item)
        //{
        //    int i;
        //    i = 0;
        //    i++;
        //}
        public void SwitchInventory(bool forward)
        { 
            if (mActiveInventories.Count < 2) return;
            int index = (forward ? 1 : -1) + mActiveInventories.IndexOf(mActiveInventory);
            if (index < 0)
                index = mActiveInventories.Count - 1;
            if (index >= mActiveInventories.Count)
                index = 0;
            mActiveInventory = mActiveInventories[index];

        }
        public void HideInventory()
        {
            //inventoryGrid.Enabled = false;
            //inventoryGrid.Visible = false;
            //inventoryGrid.Widgets.Clear();
        }
        public void InventoryMove(Vector2Int v)
        {
            mInventoryGridMap[mActiveInventory][HighlightPos].Background = new SolidBrush(DEFAULT_ITEM_COL);
            HighlightPos += v;
            if (HighlightPos.X < 0)
                HighlightPos.X = mActiveInventory.ItemMap.GetLength(0) - 1;
            if (HighlightPos.X >= mActiveInventory.ItemMap.GetLength(0))
                HighlightPos.X = 0;

            if (HighlightPos.Y < 0)
                HighlightPos.Y = mActiveInventory.ItemMap.GetLength(1) - 1;
            if (HighlightPos.Y >= mActiveInventory.ItemMap.GetLength(1))
                HighlightPos.Y = 0;

            mInventoryGridMap[mActiveInventory][HighlightPos].Background = new SolidBrush(HIGHLIGHTED_ITEM_COL);
            //.GridRow(HighlightPos.X, HighlightPos.Y)
        }
        public void OnInventorySelect()
        {
            
        }
    }
}
