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
        Texture2D mSprite;
        Body mBody;
        Vector2 mSize;
        public Collidable( World world, Vector2 pos, Vector2 size)
        {
            mSize = size;
            mBody = world.CreateBody(pos, 0, BodyType.Static);
            Fixture pfixture = mBody.CreateRectangle(size.X, size.Y, 1f, Vector2.Zero);
            // Give it some bounce and friction
            pfixture.Restitution = 0.3f;
            pfixture.Friction = 0.5f;
            mSprite = TextureAssetManager.GetSimpleSquare();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, mBody.Position, null, Color.White, mBody.Rotation, Vector2.Zero, mSize, SpriteEffects.None, 1f);
        }
    }
}
