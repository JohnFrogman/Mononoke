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
        public GUI(Desktop desktop, Car car)
        {
            mCar = car;
            desktop.Widgets.Clear();
            Panel mainPanel = new Panel();
            mainPanel.Background = new SolidBrush(Color.Transparent);
            desktop.Root = mainPanel;

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

            mainPanel.AddChild(headerPanel);





                //Image icon = new Image();
                //icon.Renderable = new TextureRegion(TextureAssetManager.GetIconByName("petrichor"));
                //icon.HorizontalAlignment = HorizontalAlignment.Left;
                //icon.VerticalAlignment = VerticalAlignment.Top;
                //Myra.Graphics2D.Thickness t = icon.Padding;
                //t.Left = 6;
                //icon.Padding = t;

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
    }
}
