using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D;

namespace Mononoke
{
    struct GUIData { 
        DateTime mDateTime;
        int mHunger;
        int mFuel;
        int mEnergy;
    }
    class GUI
    {
        //uint UnitPerStamina = 1;
        //uint UnitPerHp = 1;
        Label mTimeLabel;
        Car mCar;
        Label mSpeedLabel;
        Vector2 spacing = new Vector2(60, 0);
        Panel mMainPanel;
        public GUI(Desktop desktop, Car car)
        {
            mCar = car;
            desktop.Widgets.Clear();
            mMainPanel = new Panel();
            mMainPanel.Background = new SolidBrush(Color.Transparent);
            desktop.Root = mMainPanel;

            HorizontalStackPanel headerPanel = new HorizontalStackPanel();
            headerPanel.Spacing = 10;
            headerPanel.Background = new SolidBrush(Color.Gray);
            headerPanel.VerticalAlignment = VerticalAlignment.Top;
            headerPanel.Height = 35;
            mTimeLabel = new Label();
            mTimeLabel.Text = "14:32";
            headerPanel.AddChild(mTimeLabel);

            mSpeedLabel = new Label();
            mSpeedLabel.Text = "";
            headerPanel.AddChild(mSpeedLabel);

            mMainPanel.AddChild(headerPanel);
        }
        // 0 = 01/01/1114
        // t = days since 01/01/1114
        // 360 days in a year
        // 30 days in a month

        //public void OnTimeChange()
        //{ 
        //}
        //private string IntegerToYear(int t)
        //{
        //    return ((t / 360) + 1114).ToString() ;
        //}
        //private string IntegerToMonth( int t )
        //{ 
        //    return (( t % 360 ) / 30 ).ToString();
        //}
        //private string IntegerToDay(int t)
        //{
        //    return ((t % 360) % 30).ToString();
        //}
        public void Update(GameTime gameTime)
        {
            mSpeedLabel.Text = mCar.Speed() + " m/s";
        }
        Grid BuildInventoryGrid(Inventory inv)
        {
            Grid inventoryGrid = new Grid
            {
                ShowGridLines = true,
                ColumnSpacing = 150,
                RowSpacing = 150,
            };
            inventoryGrid.Padding = new Thickness(5);
            inventoryGrid.Background = new SolidBrush(Color.Black);
            inventoryGrid.Width = 500;
            inventoryGrid.Height = 500;
            inventoryGrid.Enabled = true;
            inventoryGrid.Visible = true;
            for (int i = 0; i < inv.ItemMap.GetLength(0); i++)
            {
                for (int j = 0; j < inv.ItemMap.GetLength(1); j++)
                {
                    Panel itemSlot = new Panel();
                    itemSlot.Background = new SolidBrush(Color.Gray);
                    itemSlot.Width = 80;
                    itemSlot.Height = 80;
                    //mInventoryGrid.GridRow = j;
                    //mInventoryGrid.GridColumn = i;
                    itemSlot.GridColumn = i;
                    itemSlot.GridRow = j;

                    //Image portrait = new Image();
                    //portrait.Renderable = new TextureRegion(TextureAssetManager.GetIconByName("petrichor"));
                    //portrait.HorizontalAlignment = HorizontalAlignment.Left;
                    //portrait.VerticalAlignment = VerticalAlignment.Center;
                    //portrait.PaddingLeft = 6;
                    inventoryGrid.Widgets.Add(itemSlot);
                }
            }
            return inventoryGrid;
        }
        public void ShowInventory(Inventory inv, Vector2 pos)
        { 
            Grid inventoryGrid = BuildInventoryGrid(inv);
            //inventoryGrid.pos
            inventoryGrid.Enabled = true;
            inventoryGrid.Visible = true;
            for (int i = 0; i < inv.ItemMap.GetLength(0); i++)
            {
                for (int j = 0; j < inv.ItemMap.GetLength(1); j++)
                {
                    Panel itemSlot = new Panel();
                    itemSlot.Background = new SolidBrush(Color.Gray);
                    itemSlot.Width = 80;
                    itemSlot.Height = 80;
                    //mInventoryGrid.GridRow = j;
                    //mInventoryGrid.GridColumn = i;
                    itemSlot.GridColumn = i;
                    itemSlot.GridRow = j;

                    //Image portrait = new Image();
                    //portrait.Renderable = new TextureRegion(TextureAssetManager.GetIconByName("petrichor"));
                    //portrait.HorizontalAlignment = HorizontalAlignment.Left;
                    //portrait.VerticalAlignment = VerticalAlignment.Center;
                    //portrait.PaddingLeft = 6;
                    inventoryGrid.Widgets.Add(itemSlot);
                }
            }
            mMainPanel.AddChild(inventoryGrid);
        }
        public void HideInventory()
        {
            //inventoryGrid.Enabled = false;
            //inventoryGrid.Visible = false;
            //inventoryGrid.Widgets.Clear();
        }
    }
}
