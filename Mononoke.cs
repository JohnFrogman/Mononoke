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
        
        public const int VIEWPORT_WIDTH = 1920;
        public const int VIEWPORT_HEIGHT = 1080;

        //        public const int DRAW_DISTANCE = 2;

        public static Vector3 ScreenScale;
        public static Vector2 ScreenScaleV2;
        //public static Mononoke Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        IGameState CurrentState;

        Camera2D Camera;
        public static SpriteFont Font { get; private set;}
        public static Effect TestEffect;
        public Mononoke()
        {
            ScreenScale = new Vector3( VIEWPORT_WIDTH / RENDER_WIDTH, VIEWPORT_HEIGHT / RENDER_HEIGHT, 1.0f);
            ScreenScaleV2 = new Vector2(ScreenScale.X, ScreenScale.Y);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();  

            _graphics.PreferredBackBufferWidth = VIEWPORT_WIDTH;
            _graphics.PreferredBackBufferHeight = VIEWPORT_HEIGHT;
            _graphics.ApplyChanges();

            Font = Content.Load<SpriteFont>("vampire_wars");
            Camera = new Camera2D( _graphics );

            TextureAssetManager.Initialise( _graphics );
            //CurrentState = new Overworld(Camera, _graphics, this);
            //CurrentState = new MainMenu( _graphics, this);
            NewGame();
            //Camera.Zoom = 1f;

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
            CurrentState.Update( gameTime );
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

            CurrentState.Draw(_spriteBatch, _graphics);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        public void Quit()
        {
            this.Exit();
        }
        public void NewGame()
        {
            CurrentState = new Overworld( Camera, _graphics, this);
        }
    }
}
