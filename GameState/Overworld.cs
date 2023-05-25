using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mononoke
{
    class Overworld : IGameState
    {
        private string mSaveSlotName = "";
        private OverworldController mController;
        private MapHolder mMaps;
        private ProvinceHolder mProvinces;
        private ActorHolder mActors;
        private Player mPlayer;
        private GameEventQueue mEventQueue;
        private Camera2D mCamera;
        private MapUnitHolder mUnits;
        private Pathfinder mPathfinder;
        private GraphicsDeviceManager mGraphics;
        private MapUnitTypeHolder mUnitTypeHolder;
        public Overworld(Camera2D camera, GraphicsDeviceManager _graphics, Mononoke game )
        {
            mSaveSlotName = "Cimmeria";
            mCamera = camera;
            mGraphics = _graphics;
            mUnits = new MapUnitHolder( );
            mUnitTypeHolder = new MapUnitTypeHolder( );
            mMaps = new MapHolder( _graphics, mUnits, mUnitTypeHolder );
            mPathfinder = new Pathfinder( _graphics, mMaps, mUnits );
            mActors = new ActorHolder();
            mProvinces = new ProvinceHolder(mActors, mMaps);
            mPlayer = new Player(camera, _graphics);
            mEventQueue = new GameEventQueue(mPlayer);
            mController = new OverworldController( this, camera, mMaps, mProvinces, mPlayer, mEventQueue, _graphics, game, mUnits, mPathfinder );
            mUnits.Initialise( mPathfinder );

            Actor TA = new NonPlayerActor("Test actor", Color.Magenta, eActorBehavior.Raider);
            Vector2 pos = new Vector2(15, 15);
            mUnits.AddUnit( mUnitTypeHolder.BuildUnitByTypeName( "infantry", pos, TA ) );
            mUnits.AddUnit( mUnitTypeHolder.BuildUnitByTypeName("infantry", new Vector2( 10, 10 ), mPlayer));
        }
        void IGameState.Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            Vector2 result = Vector2.Floor(-mCamera.Position / MapHolder.PIXELS_PER_TILE);
            mEventQueue.Draw(_spriteBatch);
            mMaps.Draw(_spriteBatch, _graphics, result);
            mUnits.Draw(_spriteBatch);
            mPlayer.Draw(_spriteBatch, _graphics);

            mController.Draw(_spriteBatch);
            mController.DrawCursor(_spriteBatch);
        }

        void IGameState.Update(GameTime gameTime)
        {
            mEventQueue.Update(gameTime);
            mMaps.Update(gameTime);
            mPlayer.Update( gameTime);
            mUnits.Update(gameTime, mMaps );
            mController.Update(gameTime);
        }

        // Need to save out units, maps, Actors, Provinces and the player
        public void Save()
        { 
            if ( mSaveSlotName == "" ) // New save, need to generate a slot name.
            {
                mSaveSlotName = mPlayer.GetSaveName();
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
            mMaps.Save( Mononoke.SAVE_PATH + mSaveSlotName, mGraphics.GraphicsDevice );
            mUnits.Save( Mononoke.SAVE_PATH + mSaveSlotName );
            //mActors.Save( mSaveSlotName );
            //mPlayer.Save( mSaveSlotName );
            //mProvinces.Save( mSaveSlotName );
        }
        public void Load( )
        { 
            mMaps.Load( Mononoke.SAVE_PATH + mSaveSlotName, mUnits, mUnitTypeHolder, mGraphics );
            mUnits.Load( Mononoke.SAVE_PATH + mSaveSlotName);
        }

    }
}
