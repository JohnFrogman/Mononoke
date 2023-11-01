using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Mononoke
{
    class Overworld : IGameState
    {
        private string mSaveSlotName = "";
        private GUI mGui;
        private Camera2D mCamera;
        private Player mPlayer;
        private Car mCar;
        private TerrainManager mTerrainManager;
        public Overworld(Camera2D camera, GraphicsDeviceManager _graphics, Mononoke game, Desktop desktop)
        {
            mSaveSlotName = "Cimmeria";
            mCamera = camera;
            mCar = new Car(this, new Vector2(150f, 150f), TextureAssetManager.GetCarSpriteByName("car_big"), this);
            mPlayer = new Player(new Vector2(300f,300f), this, mCamera);
            mGui = new GUI(desktop, mCar);
            mTerrainManager = new TerrainManager(mCamera);
            //mCollidables.Add(new Collidable(new Vector2(0, 0), true, TextureAssetManager.GetPlayerSprite(), 100, Vector2.Zero));
        }
        void IGameState.Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            mTerrainManager.Draw(_spriteBatch);
            //foreach (Collidable collidable in mCollidables)
            //{
            //    collidable.Draw(_spriteBatch);
            //}
            mCar.Draw(_spriteBatch);
            mPlayer.Draw(_spriteBatch);
            //mController.Draw(_spriteBatch);
            //mController.DrawCursor(_spriteBatch);
        }
        public void EnterCar(Car car)
        { 
            mPlayer.EnterCar(car);
        }
        public void OpenBoot(Car car)
        {
            mGui.ShowInventory(car.mBoot);
            mPlayer.mActiveInteraction = () => { CloseBoot(); };
            mPlayer.Enabled = false;
        }
        public void CloseBoot()
        { 
            mGui.HideInventory();
            mPlayer.Enabled = true;
        }
        void IGameState.Update(GameTime gameTime)
        {
            MouseState mstate = Mouse.GetState();
            mCar.Update(gameTime);
            mPlayer.Update(gameTime);
            mGui.Update(gameTime);
            mTerrainManager.Update(gameTime);
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //CollisionManager.CheckCollisions();
            //We update the world
            //mWorld.Step(totalSeconds);
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
    }
}
