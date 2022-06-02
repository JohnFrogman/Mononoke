﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Mononoke
{
    public class Mononoke : Game
    {
        public const int RENDER_WIDTH = 960;
        public const int RENDER_HEIGHT = 540;
        
        public const int VIEWPORT_WIDTH = 1920;
        public const int VIEWPORT_HEIGHT = 1080;

        public const int SCALE_X = VIEWPORT_WIDTH / RENDER_WIDTH;
        public const int SCALE_Y = VIEWPORT_HEIGHT / RENDER_HEIGHT;

        //        public const int DRAW_DISTANCE = 2;

        Vector3 ScreenScale; 
        //public static Mononoke Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Controller Controller;
        private MapHolder Maps;
        private ProvinceHolder Provinces;
        private ActorHolder Actors;
        private Player Player;
        private GameEventQueue EventQueue;
        Camera2D Camera;
        public static SpriteFont Font { get; private set;}
        public static Effect TestEffect;
        public Mononoke()
        {
            ScreenScale = new Vector3( VIEWPORT_WIDTH / RENDER_WIDTH, VIEWPORT_HEIGHT / RENDER_HEIGHT, 1.0f);
            //if ( Game != null )
            //{
            //    throw new System.Exception("You cannot have two instances of the game.");
            //}
            //Game = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //TestEffect = Content.Load<Effect>("water");
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();  

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            Font = Content.Load<SpriteFont>("vampire_wars");
            Camera = new Camera2D( _graphics );

            //Camera.Zoom = 1f;
            Maps = new MapHolder( _graphics );
            Actors = new ActorHolder();
            Provinces = new ProvinceHolder( Actors, Maps );
            Player = new Player( Camera);
            EventQueue = new GameEventQueue( Player );
            Controller = new Controller( Camera, this, Maps, Provinces, Player, EventQueue, _graphics );
            //EventQueue.AddEvent( new GameEvent( "Food demand", 1, EventQueue ) ); 
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here#
            //MapTextTest = Content.Load<Texture2D>("terrain");
        }

        protected override void Update(GameTime gameTime)
        {
            Controller.Update( gameTime );
            Provinces.Update( gameTime );
            EventQueue.Update( gameTime );
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            Matrix viewMatrix = Camera.GetTransform();

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
            //               SamplerState.PointClamp, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                                SamplerState.PointClamp, null, null, null, viewMatrix * Matrix.CreateScale( ScreenScale ));

            //TestEffect.CurrentTechnique.Passes[0].Apply();
       
            Vector2 result = Vector2.Floor( -Camera.Position / MapHolder.PIXELS_PER_TILE );
            Maps.Draw( _spriteBatch, _graphics, result ); 
            Provinces.Draw ( _spriteBatch, _graphics, result );        
            Player.Draw( _spriteBatch, _graphics );
            EventQueue.Draw( _spriteBatch );
            Controller.Draw( _spriteBatch );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        public void Quit()
        {
            this.Exit();
        }
    }
}
