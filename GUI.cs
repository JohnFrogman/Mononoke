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
        public InventoryManager mInventoryManager;

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
            mInventoryManager = new (desktop, mMainPanel);
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
            //mInventoryManager.upda(gameTime);
        }
        
    }
}
