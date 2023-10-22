using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal class Collidable
    {
        protected Texture2D mSprite;
        protected Body mBody;
        protected Vector2 mSize;
        protected Vector2 mTextureOrigin;
        protected Vector2 mTextureSize;

        public Collidable(World world, Vector2 pos, BodyType type, Texture2D sprite )
        {
            mSprite = sprite;
            mTextureSize = new Vector2(mSprite.Width, mSprite.Height);
            mSize = mTextureSize;
            mTextureOrigin = mTextureSize / 2f;
            Build(world, pos, type);
        }
        public Collidable( World world, Vector2 pos, BodyType type, Texture2D sprite, Vector2 size)
        {
            mSprite = sprite;
            mBody = world.CreateBody(pos, 0, type);
            mTextureSize = new Vector2(mSprite.Width , mSprite.Height);
            mTextureOrigin = mTextureSize /2f;
            mSize = size;
            Build(world, pos, type);
        }
        void Build(World world, Vector2 pos, BodyType type)
        {
            mBody = world.CreateBody(pos, 0, type);
            Fixture pfixture = mBody.CreateRectangle(mSize.X, mSize.Y, 0.1f, Vector2.Zero);
            pfixture.Restitution = 0.3f;
            pfixture.Friction = 0.5f;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, mBody.Position, null, Color.White, mBody.Rotation, mTextureOrigin, mSize / mTextureSize, SpriteEffects.FlipVertically, 0f);
            //spriteBatch.Draw(mSprite, mBody.Position - ( new Vector2(25f,25f) ), null, Color.White, mBody.Rotation, Vector2.Zero, mSize, SpriteEffects.None, 1f);
        }
    }
}   
