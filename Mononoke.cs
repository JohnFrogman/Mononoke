using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;

namespace Mononoke
{
    public class Mononoke : Game
    {
        public const int RENDER_WIDTH = 1920;
        public const int RENDER_HEIGHT = 1080;
        public const string SAVE_PATH = "data/save/";
        //        public const int DRAW_DISTANCE = 2;

        //public static Mononoke Game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private double frameRate = 0;
        private bool ShowFrameCounter = true;

        IGameState CurrentState;

        Camera2D Camera;
        public static SpriteFont Font { get; private set;}
        public static Effect TestEffect;
        public Desktop mDesktop;

        public Mononoke()
        {
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

            mDesktop = new Desktop();

            Font = Content.Load<SpriteFont>("vampire_wars");
            Camera = new Camera2D( _graphics );

            TextureAssetManager.Initialise( _graphics );
            //CurrentState = new MainMenu( _graphics, this);
            CurrentState = new Overworld(Camera, _graphics, this, mDesktop);
            //NewGame();
            //Camera.Zoom = 1f;

            //EventQueue.AddEvent( new GameEvent( "Food demand", 1, EventQueue ) ); 
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MyraEnvironment.Game = this;

            // TODO: use this.Content to load your game content here#
            //MapTextTest = Content.Load<Texture2D>("terrain");
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentState.Update( gameTime );
            frameRate = 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            Matrix viewMatrix = Camera.GetTransform();

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
            //               SamplerState.PointClamp, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                                SamplerState.PointClamp, null, null, null, viewMatrix * Matrix.CreateScale( new Vector3( 1f, 1f, 1f) ));

            //TestEffect.CurrentTechnique.Passes[0].Apply();
            CurrentState.Draw(_spriteBatch, _graphics);
            //if ( ShowFrameCounter )
                //_spriteBatch.DrawString( Font, frameRate.ToString().Substring( 0, 5 ), -Camera.Position, Color.Black );

            _spriteBatch.End();
            mDesktop.Render();
            base.Draw(gameTime);
        }
        public void Quit()
        {
            this.Exit();
        }
        public void NewGame()
        {
            CurrentState = new Overworld( Camera, _graphics, this, mDesktop);
        }
    }
}
