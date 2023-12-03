using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    class HeldItemPanel
    {
        List<ItemPanel> Panels = new();
        Grid mGrid;
        static Vector2Int OFFSET = new Vector2Int(2, 22);
        public HeldItemPanel(Panel mainPanel, InventoryItem item, Vector2Int pos, Vector2Int inventoryPos, Inventory inventory) 
        { 
            mGrid = new Grid();
            ItemPanel p = new ItemPanel(0,0,item);
            mGrid.AddChild(p.mPanel);
            p.mPanel.Background = new SolidBrush(Color.Transparent);
            SetPos(inventoryPos, pos, inventory);
            mainPanel.AddChild(mGrid);
        }
        public void SetPos(Vector2Int inventoryPos, Vector2Int pos, Inventory inventory)
        {
            mGrid.Left = inventoryPos.X + (pos.X * InventoryGUI.ITEM_BOX_SIZE) + OFFSET.X;
            mGrid.Top =  inventoryPos.Y + (pos.Y * InventoryGUI.ITEM_BOX_SIZE) + OFFSET.Y;
        }
        public void Delete()
        {
            mGrid.RemoveFromParent();
        }

    }
    class ItemPanel
    {
        public Panel mPanel;
        public Image mImage;
        public ItemPanel(int i, int j, InventoryItem item)
        {
            mPanel = new Panel();
            mPanel.Background = new SolidBrush(InventoryGUI.DEFAULT_ITEM_COL);
            mPanel.Width = InventoryGUI.ITEM_BOX_SIZE;
            mPanel.Height = InventoryGUI.ITEM_BOX_SIZE;
            mImage = new Image();
            if (item != null)
            {
                Texture2D tex = TextureAssetManager.GetIconByName("petrichor");
                mImage.Renderable = new TextureRegion(tex);
                //portrait.Scale = new Vector2(ITEM_BOX_SIZE / tex.Width, ITEM_BOX_SIZE / tex.Height);
                mImage.HorizontalAlignment = HorizontalAlignment.Center;
                mImage.VerticalAlignment = VerticalAlignment.Center;
            }
            mPanel.AddChild(mImage);
            mPanel.GridRow = j;
            mPanel.GridColumn = i;
        }
    }
    class InventoryGUI
    {
        public const int ITEM_BOX_SIZE = 32;
        public static readonly Color DEFAULT_ITEM_COL = Color.AntiqueWhite;
        public static readonly Color HIGHLIGHTED_ITEM_COL = Color.Red;
        public VerticalStackPanel Container;
        Dictionary<Vector2Int, ItemPanel> mInventoryGridMap = new();
        public Vector2Int ScreenPos;
        public InventoryGUI(Inventory inv, Panel mainPanel, Desktop desktop, Vector2Int pos)
        {
            ScreenPos = pos;
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
                    ItemPanel slotPanel = new ItemPanel(i, j, inv.ItemMap[i, j]);
                    mInventoryGridMap.Add(new Vector2Int(i, j), slotPanel);
                    inventoryGrid.AddChild(slotPanel.mPanel);
                }
            }
            Container = new VerticalStackPanel
            {
                Spacing = 4
            };
            Label title = new Label();
            title.Height = 16;
            title.Text = inv.Name;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            Container.Background = new SolidBrush(Color.Gray);
            Container.AddChild(title);
            Container.AddChild(inventoryGrid);
            Container.Left = pos.X;
            Container.Top = pos.Y;
            Container.Width = inventoryGrid.Width;
            Container.Height = inventoryGrid.Height + title.Height + 3;
            mainPanel.AddChild(Container);

        }
        public void Delete()
        {
            Container.RemoveFromParent();
        }
        public void Highlight(Vector2Int pos)
        {
            mInventoryGridMap[pos].mPanel.Background = new SolidBrush(HIGHLIGHTED_ITEM_COL);
        }
        public void Unhighlight(Vector2Int pos)
        {
            mInventoryGridMap[pos].mPanel.Background = new SolidBrush(DEFAULT_ITEM_COL);
        }
        public void GrabItem(Vector2Int pos)
        {
            //mInventoryGridMap[pos].mImage = new Image();
            mInventoryGridMap[pos].mImage.Renderable = null;
        }
        public void PlaceItem(Vector2Int pos, InventoryItem item)
        {
            //mInventoryGridMap[pos].mImage = new Image();
            Texture2D tex = TextureAssetManager.GetIconByName("petrichor");
            mInventoryGridMap[pos].mImage.Renderable = new TextureRegion(tex); ;
        }
    }
}
