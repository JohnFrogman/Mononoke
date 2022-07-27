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
        public Overworld(Camera2D camera, GraphicsDeviceManager _graphics, Mononoke game )
        {
            mCamera = camera;
            mUnits = new MapUnitHolder( );
            mMaps = new MapHolder(_graphics, mUnits);
            mPathfinder = new Pathfinder( _graphics, mMaps, mUnits );
            mActors = new ActorHolder();
            mProvinces = new ProvinceHolder(mActors, mMaps);
            mPlayer = new Player(camera, _graphics);
            mEventQueue = new GameEventQueue(mPlayer);
            mController = new OverworldController( this, camera, mMaps, mProvinces, mPlayer, mEventQueue, _graphics, game, mUnits, mPathfinder );
            mUnits.Initialise( mPathfinder );

            mUnits.AddUnit( new Vector2(15,15), new MapUnit( TextureAssetManager.GetUnitSpriteByName("soldier"), 1, "Gobloid", new Vector2(15,15), new NonPlayerActor("Test actor", Color.Magenta, eActorBehavior.Raider), mMaps));

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
            mUnits.Update(gameTime);
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
                while ( Directory.Exists( "data/save/" + str )
                {
                    ++i;
                    str = mSaveSlotName + "_" + i;
                }
                mSaveSlotName = str;
            }
            mMaps.Save( mSaveSlotName );
            mUnits.Save( mSaveSlotName );
            mActors.Save( mSaveSlotName );
            mPlayer.Save( mSaveSlotName );
            //mProvinces.Save( mSaveSlotName );
        }
        public void Load()
        { 
        }

    }
}
