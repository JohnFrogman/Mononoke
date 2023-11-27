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
        public static Player mPlayer;
        private Car mCar;
        private TerrainManager mTerrainManager;
        private InputManager mInputManager;
        private OverworldController mController;
        private List<RigidBody> mBodies = new();
        public Overworld(Camera2D camera, GraphicsDeviceManager graphics, Mononoke game, Desktop desktop)
        {
            mSaveSlotName = "Cimmeria";
            mCamera = camera;
            mCar = new Car(this, new Vector2(0f, 0f), TextureAssetManager.GetCarSpriteByName("car_big"), this);
            mPlayer = new Player(new Vector2(50f,50f), this, mCamera, graphics);
            mGui = new GUI(desktop, mCar);
            mTerrainManager = new TerrainManager(mCamera);
            mInputManager = new InputManager();
            mController = new OverworldController(mInputManager, this, mPlayer, mCar, mGui.mInventoryManager );
            SoundAssetManager.PlaySongsByName(new List<string>{"polygondwanaland", "deserted dunes welcome weary feet"});
            SoundAssetManager.SetPaused(true);
            //mPlayer.EnterCar(mCar);
            //mCollidables.Add(new Collidable(new Vector2(0, 0), true, TextureAssetManager.GetPlayerSprite(), 100, Vector2.Zero));
            mBodies.Add(RigidBody.BuildRectangle(new Vector2(-200,0), false, 300, new Vector2(100,500)));
        }
        void IGameState.Draw(SpriteBatch spriteBatch, GraphicsDeviceManager _graphics)
        {
            mTerrainManager.Draw(spriteBatch);
            foreach (RigidBody b in mBodies)
            {
                b.Draw(spriteBatch);
            }
            mCar.Draw(spriteBatch);
            mPlayer.Draw(spriteBatch);
        }
        public void EnterCar(Car car)
        { 
            mPlayer.EnterCar(car);
        }
        public void OpenBoot(Car car)
        {
            mGui.mInventoryManager.ToggleInventory(car.mBoot, car.mBody.mPosition.ToPoint());
            //mGui.ShowInventory(car.mBoot, car.mPosition);
            mPlayer.mActiveInteraction = new Interaction( CloseBoot, 1f);
        }
        public void ShowPlayerInventory()
        {
            //mGui.ShowInventory(car.mBoot);
            //mPlayer.mActiveInteraction = () => { CloseBoot(); };
            //mPlayer.Enabled = false;
        }
        public void CloseBoot()
        { 
            //mGui.();
        }
        void IGameState.Update(GameTime gameTime)
        {
            SoundAssetManager.Update(gameTime);
            mInputManager.Update(gameTime);
            mController.Update(gameTime);
            mGui.Update(gameTime);
            mTerrainManager.Update(gameTime);
            mPlayer.Update(gameTime);
            mCar.Update(gameTime);
            float ListeningDistance;
            if (mPlayer.InCar)
            {
                ListeningDistance = 1f;
            }
            else
            { 
                ListeningDistance = (mPlayer.mBody.mPosition - mCar.mBody.mPosition).Magnitude() / RigidBody.PIXELS_PER_METRE;
            }
            float attentuation = 3f * (float)Math.Pow(0.5f, ListeningDistance);
            SoundAssetManager.SetMusicVolume(attentuation);
            
            PhysicsManager.DoStep(gameTime);
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
