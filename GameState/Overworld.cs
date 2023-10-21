using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using nkast.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Common;
using Microsoft.Xna.Framework.Media;

namespace Mononoke
{
    class Overworld : IGameState
    {
        private string mSaveSlotName = "";
        private OverworldController mController;
        private GUI mGui;
        private Camera2D mCamera;
        private GraphicsDeviceManager mGraphics;
        private Player mPlayer;
        private World mWorld;
        private List<Collidable> mCollidables;
        GUI mGUI;
        public Overworld(Camera2D camera, GraphicsDeviceManager _graphics, Mononoke game, Desktop desktop)
        {
            mSaveSlotName = "Cimmeria";
            mCamera = camera;
            mGraphics = _graphics;
            mController = new OverworldController(this, camera, _graphics, game);
            mGui = new GUI(camera, _graphics);
            BuildGUI(desktop);
            mWorld = new World();
            mWorld.Gravity = Vector2.Zero;
            mPlayer = new Player(new Car(mWorld));
            mCollidables = new List<Collidable>();
            mCollidables.Add(new Collidable(mWorld, new Vector2(500, 400), new Vector2(50, 50)));
        }
        void IGameState.Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            foreach (Collidable collidable in mCollidables)
            {
                collidable.Draw(_spriteBatch);
            }
            mPlayer.Draw(_spriteBatch);
            mGui.Draw(_spriteBatch, _graphics);

            //mController.Draw(_spriteBatch);
            //mController.DrawCursor(_spriteBatch);
        }

        void IGameState.Update(GameTime gameTime)
        {
            mPlayer.Update(gameTime);
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //We update the world
            mWorld.Step(totalSeconds);
        }

        // Need to save out units, maps, Actors, Provinces and the player
        public void Save()
        { 
            if ( mSaveSlotName == "" ) // New save, need to generate a slot name.
            {
                mSaveSlotName = "Test";
                string str = mSaveSlotName;
                int i = 0;
                while ( Directory.Exists( Mononoke.SAVE_PATH + str ) )
                {
                    ++i;
                    str = mSaveSlotName + "_" + i;
                }
                mSaveSlotName = str;
                Directory.CreateDirectory( Mononoke.SAVE_PATH + mSaveSlotName );
            }
            else // Not a new save so clear everything in the folder.
            {
                //DirectoryInfo di = new DirectoryInfo(Mononoke.SAVE_PATH + mSaveSlotName);
                //foreach (FileInfo file in di.GetFiles())
                //{
                //    file.Delete();
                //}
                //foreach (DirectoryInfo dir in di.GetDirectories())
                //{
                //    dir.Delete(true);
                //}
                //foreach (FileInfo file in di.EnumerateFiles())
                //{
                //    file.Delete();
                //}
            }
            //mActors.Save( mSaveSlotName );
            //mPlayer.Save( mSaveSlotName );
            //mProvinces.Save( mSaveSlotName );
        }
        public void Load( )
        { 
        }

        void BuildGUI(Desktop d)
        { 
            //d.Widgets.Clear();
            //Panel mainPanel = new Panel();
            //mainPanel.Background = new SolidBrush(Color.Transparent);
            //d.Root = mainPanel;

            //VerticalStackPanel apprenticeView = new VerticalStackPanel();
            //apprenticeView.Spacing = 10;
            //foreach (Apprentice a in mApprentices)
            //{
            //    Panel aV = new Panel();               
            //    aV.Background = new SolidBrush(Color.Red);
            //    aV.Width = 150;
            //    aV.Height = 50;
            //    Image portrait = new Image();
            //    portrait.Renderable = new TextureRegion(TextureAssetManager.GetIconByName("petrichor"));
            //    portrait.HorizontalAlignment = HorizontalAlignment.Left;
            //    portrait.VerticalAlignment = VerticalAlignment.Center;
            //    portrait.PaddingLeft = 6;

            //    aV.AddChild(portrait);
            //    TextButton button = new TextButton();
            //    //button.Text = a.Name;
            //    button.Width = 150;
            //    button.Height = 50;
            //    button.Background = new SolidBrush(Color.Transparent);
            //    button.PressedBackground = new SolidBrush(Color.Transparent);
            //    button.OverBackground = new SolidBrush(Color.Transparent);
            //    button.Click += (sa, aa) =>
            //    {
            //        ApprenticeClick(a);
            //    };

            //    Label name = new Label();
            //    name.Text = a.Name;
            //    name.HorizontalAlignment = HorizontalAlignment.Right;
            //    name.VerticalAlignment = VerticalAlignment.Center;
                
            //    aV.AddChild(button);
            //    aV.AddChild(name);
            //    //aV.Left = -30;
            //    //aV.Top = -20;
            //    aV.HorizontalAlignment = HorizontalAlignment.Right;
            //    aV.VerticalAlignment = VerticalAlignment.Center;
            //    //button.

            //    apprenticeView.AddChild(aV);
            //}
            //mainPanel.AddChild(apprenticeView);
        }
    }
}
