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
    class ItemPanel
    {
        public Panel mPanel;
        public Image mImage;
        public ItemPanel(int i, int j, InventoryItem item)
        {
            Panel panel = new Panel();
            panel.Background = new SolidBrush(InventoryGUI.DEFAULT_ITEM_COL);
            panel.Width = InventoryGUI.ITEM_BOX_SIZE;
            panel.Height = InventoryGUI.ITEM_BOX_SIZE;
            Image image = new Image();
            if (item != null)
            {
                Texture2D tex = TextureAssetManager.GetIconByName("petrichor");
                image.Renderable = new TextureRegion(tex);
                //portrait.Scale = new Vector2(ITEM_BOX_SIZE / tex.Width, ITEM_BOX_SIZE / tex.Height);
                image.HorizontalAlignment = HorizontalAlignment.Center;
                image.VerticalAlignment = VerticalAlignment.Center;
            }
            panel.AddChild(image);
            panel.GridRow = j;
            panel.GridColumn = i;
        }
    }
    class InventoryGUI
    {
        public Texture2D mImage;
        public const int ITEM_BOX_SIZE = 32;
        public static readonly Color DEFAULT_ITEM_COL = Color.AntiqueWhite;
        public static readonly Color HIGHLIGHTED_ITEM_COL = Color.Red;
        public VerticalStackPanel Container;
        Dictionary<Vector2Int, ItemPanel> mInventoryGridMap = new();
        public InventoryGUI(Inventory inv, Panel mainPanel, Desktop desktop)
        {
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
            Container.Left = 200;
            Container.Top = 200;
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
    }
}
