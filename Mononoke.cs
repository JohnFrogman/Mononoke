﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace Mononoke
{
    public class Mononoke : Game
    {
        //public static Mononoke Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Controller Controller;
        private MapHolder Maps;
        Camera2D Camera;
        public Mononoke()
        {
            //if ( Game != null )
            //{
            //    throw new System.Exception("You cannot have two instances of the game.");
            //}
            //Game = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();  

_graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
_graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window
_graphics.ApplyChanges();

            Maps = new MapHolder( _graphics );
            Camera = new Camera2D( );
            Controller = new Controller( Camera, this );

            //Camera.Zoom = 1f;
            //Actors = new ActorHolder();
            //Actors.New( );
            //Provinces = new ProvinceHolder();
            //EconomyModel.SetProvinceHolder( Provinces );
            //Provinces.New( Maps, EconomyModel );
            //Player.Load("");

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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Vector3 screenScale = GetScreenScale();
            Matrix viewMatrix = Camera.GetTransform();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                           SamplerState.PointClamp, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));

            Maps.Draw( _spriteBatch );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        public Vector3 GetScreenScale()
        {
            float scaleX = 1;//(float)_graphicsDevice.Viewport.Width / (float)_width;
            float scaleY = 1;//(float)_graphicsDevice.Viewport.Height / (float)_height;
            return new Vector3(scaleX, scaleY, 1.0f);
        }        
        public void Quit()
        {
            this.Exit();
        }
        public void ClickAt(Vector2 pos )
        {
            Debug.WriteLine( "click at " + pos );
            Maps.GetTerrainColourAt( pos );
        }
    }
}
