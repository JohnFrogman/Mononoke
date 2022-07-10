using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;

namespace Mononoke
{
    class Overworld : IGameState
    {
        private OverworldController Controller;
        private MapHolder Maps;
        private ProvinceHolder Provinces;
        private ActorHolder Actors;
        private Player Player;
        private GameEventQueue EventQueue;
        private Camera2D Camera;
        private MapUnitHolder Units;
        private Pathfinder Pathfinder;
        public Overworld(Camera2D camera, GraphicsDeviceManager _graphics, Mononoke game )
        {
            Camera = camera;
            Units = new MapUnitHolder( );
            Maps = new MapHolder(_graphics, Units);
            Pathfinder = new Pathfinder( _graphics, Maps, Units );
            Actors = new ActorHolder();
            Provinces = new ProvinceHolder(Actors, Maps);
            Player = new Player(camera, _graphics);
            EventQueue = new GameEventQueue(Player);
            Controller = new OverworldController(camera, Maps, Provinces, Player, EventQueue, _graphics, game, Units, Pathfinder );
            Units.Initialise( Pathfinder );

            Units.AddUnit( new Vector2(15,15), new MapUnit(null, 1, "Gobloid", new Vector2(15,15), new Actor("Test actor", Color.Magenta)));

        }
        void IGameState.Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
        {
            Vector2 result = Vector2.Floor(-Camera.Position / MapHolder.PIXELS_PER_TILE);
            Maps.Draw(_spriteBatch, _graphics, result);
            Player.Draw(_spriteBatch, _graphics);
            EventQueue.Draw(_spriteBatch);
            Controller.Draw(_spriteBatch);
            Units.Draw(_spriteBatch);
        }

        void IGameState.Update(GameTime gameTime)
        {
            Controller.Update(gameTime);
            EventQueue.Update(gameTime);
            Maps.Update(gameTime);
            Player.Update( gameTime);
            Units.Update(gameTime);
        }
    }
}
