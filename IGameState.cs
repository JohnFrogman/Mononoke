using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    public interface IGameState
    {
        public void Update( GameTime _gameTime );
        public void Draw( SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics );
    }
}
