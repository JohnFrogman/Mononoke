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
using nkast.Aether.Physics2D.Controllers;

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
        private World mWorld = new();
        private List<Collidable> mCollidables = new();
        private List<Interactable> mInteractables = new();
        private List<Interactable> mActiveInteractables = new();
        private Car mCar;
        VelocityLimitController velocityLimitController;
        public Overworld(Camera2D camera, GraphicsDeviceManager _graphics, Mononoke game, Desktop desktop)
        {
            mSaveSlotName = "Cimmeria";
            mCamera = camera;
            mGraphics = _graphics;
            mController = new OverworldController(this, camera, _graphics, game);
            mWorld.Gravity = Vector2.Zero;
            velocityLimitController = new VelocityLimitController(float.MaxValue, float.MaxValue);
            mWorld.Add(velocityLimitController);
            mCar = new Car(mWorld, new Vector2(150f, 150f), TextureAssetManager.GetCarSpriteByName("car_big"), this);
            //mPlayer = new Player(mWorld, new Vector2(30,30), this, mCamera);
            mGui = new GUI(desktop, mCar);
            //mPlayer.EnterCar(mCar);

            mCollidables.Add(new Collidable(new Vector2(0, 0), true, TextureAssetManager.GetPlayerSprite(), 100));
        }      
        public void RegisterInteractable(Interactable i)
        { 
            mInteractables.Add(i);
        }
        void IGameState.Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            foreach (Collidable collidable in mCollidables)
            {
                collidable.Draw(_spriteBatch);
            }
            mCar.Draw(_spriteBatch);
            //mPlayer.Draw(_spriteBatch);

            //mController.Draw(_spriteBatch);
            //mController.DrawCursor(_spriteBatch);
        }
        public void TryInteract()
        {
            foreach (Interactable i in mActiveInteractables)
            { 
                i.Interact();
            } 
        }
        public void EnterCar(Car car)
        { 
            mPlayer.EnterCar(car);
        }
        void IGameState.Update(GameTime gameTime)
        {
            MouseState mstate = Mouse.GetState();
            foreach (Interactable i in mInteractables)
            {
                //if (i.Update(gameTime, mstate.Position.ToVector2()))
                //    {  }
                if (i.Update(gameTime, mPlayer.Rectangle()))
                {
                    if (!mActiveInteractables.Contains(i))
                    {
                        mActiveInteractables.Add(i);
                    }
                }
                else
                { 
                    mActiveInteractables.Remove(i); 
                }
            }
           // mPlayer.Update(gameTime);
            mCar.Update(gameTime);
            mGui.Update(gameTime);
            mCamera.Position = -mCar.mPosition + new Vector2(960, 540);
            //KeyboardState state = Keyboard.GetState();
            //if (state.IsKeyDown(Keys.D))
            //{
            //    mCar.SetSteer(-1.0f);
            //}
            //else if (state.IsKeyDown(Keys.A))
            //{
            //    mCar.SetSteer(1.0f);
            //}
            //else
            //{
            //    mCar.SetSteer(0.0f);
            //}

            //if (state.IsKeyDown(Keys.W))
            //{
            //    mCar.SetGas(1.0f);
            //}
            //else
            //{
            //    mCar.SetGas(0f);
            //}

            //if (state.IsKeyDown(Keys.S))
            //{
            //    mCar.SetBrake(1.0f);
            //}
            //else
            //{
            //    mCar.SetBrake(0f);
            //}
            //    mCar.Update(gameTime);

            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CollisionManager.CheckCollisions();
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
