using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Mononoke
{
    public class Mononoke : Game
    {
        public const int RENDER_WIDTH = 960;
        public const int RENDER_HEIGHT = 540;
        public const int DRAW_DISTANCE = 2;
        
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

            _graphics.PreferredBackBufferWidth = RENDER_WIDTH;
            _graphics.PreferredBackBufferHeight = RENDER_HEIGHT;
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

            Vector3 screenScale = GetScreenScale();
            Matrix viewMatrix = Camera.GetTransform();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                           SamplerState.PointClamp, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));

            Vector2 result = Vector2.Floor( -Camera.Position / MapHolder.PIXELS_PER_TILE );
            Maps.Draw( _spriteBatch, _graphics, result );
            Provinces.Draw ( _spriteBatch, _graphics, result );        
            Player.Draw( _spriteBatch, _graphics );
            EventQueue.Draw( _spriteBatch );
            Controller.Draw( _spriteBatch );

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
    }
}
