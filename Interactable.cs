using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal class Interactable : Fixture
    {
        Texture2D mColliderSprite;
        Vector2 mColliderTextureOrigin;
        Vector2 mColliderTextureSize;
        Vector2 mSize;
        Vector2 mOffset;

        bool Active;
        public Interactable(Shape shape, Vector2 size, Vector2 offset) 
            : base( shape )
        {         
            mOffset = offset;
            mSize = size;

         //   CollisionGroup = 2;
            //CollisionCategories = Category.Cat2;
            mColliderSprite = TextureAssetManager.GetSimpleSquare();
            mColliderTextureSize = new Vector2(mColliderSprite.Width, mColliderSprite.Height);
            mColliderTextureOrigin = mColliderTextureSize / 2f;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mColliderSprite, Body.Position + mOffset, null, Color.Green, Body.Rotation, mColliderTextureOrigin, mSize / mColliderTextureSize, SpriteEffects.None, 0f);
            //spriteBatch.Draw(mColliderSprite, new Vector2(0,1), null, Color.Green, 0f, Vector2.Zero, new Vector2(100f,100f), SpriteEffects.None, 0f);
        }
        //override void OnCollision()
        //    override void OnCollision()
        //public OnInteract() { }
    }
}
