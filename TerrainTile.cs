using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    class TerrainTile
    {
        Texture2D mTexture;
        public Vector2 mPosition;
        Vector2 mCentre;
        public TerrainTile(Texture2D texture, Vector2 position, Vector2 centre)
        {
            mTexture = texture;
            mPosition = position;
            mCentre = centre;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, null, Color.White, 0f, mCentre, 1f, SpriteEffects.None, 0f);
        }
    }
}
