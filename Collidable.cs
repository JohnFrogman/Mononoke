using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;

namespace Mononoke
{
    internal class Collidable
    {
        protected Texture2D mSprite;
        protected Vector2 mSize;

        Texture2D mColliderSprite;

        public Vector2 mPosition;
        protected float mRotation;
        protected bool mStatic;
        Collidable Parent;

        protected float mMass;
        public const int PIXELS_PER_METRE = 26;
        protected Vector2 mVelocity; // units should be metres per second.

        Vector2 mCurrentForce;
        List<Vector2> mVertices;
        public Collidable(Vector2 pos, bool isStatic, Texture2D sprite, float mass)
        {
            mSprite = sprite;
            mStatic = isStatic;
            mSize = new Vector2(mSprite.Width, mSprite.Height);
            mColliderSprite = TextureAssetManager.GetSimpleSquare();
            mMass = mass;
            mPosition = pos;
            CollisionManager.RegisterCollidable(this);
        }
        public virtual void Update(GameTime gameTime)
        {
            if (!mStatic)
            { 
                mVelocity *= 0.99f;
                Vector2 newPos = mPosition + PIXELS_PER_METRE * mVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (CollisionManager.CollidesWith(this, newPos) == null)
                { 
                    mPosition = newPos;
                    mVelocity += (float)gameTime.ElapsedGameTime.TotalSeconds * mCurrentForce / mMass;
                }
                else
                {
                    mVelocity = Vector2.Zero;
                }

                //Friction();
                //AirResistance();
                mCurrentForce = Vector2.Zero;
            }
        }
        public Vector2 Forward()
        {
            return -Vector2.UnitY.RotateRadians(mRotation);
        }
        public void AddForce(Vector2 force)
        {
            mCurrentForce += force;
            // f/m = a;
        }
        void AirResistance()
        { 
            // F = Coefficient * v * v * A
            AddForce(-mVelocity * mVelocity * 0.3f);// not including area for now.
        }

        void Friction()
        { 
            // F = coefficient * weight
            //if ( mVelocity.Magnitude() > 0 )
            //{
                AddForce( -Forward() * mMass * 8f );
            //}
           
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mColliderSprite, mPosition, null, Color.AntiqueWhite, mRotation, /*mSize*0.5f*/new Vector2(0.5f,0.5f), mSize, SpriteEffects.None, 0f);
            spriteBatch.Draw(mSprite, mPosition, null, Color.White, mRotation, mSize*0.5f, 1f, SpriteEffects.None, 0f);
          // spriteBatch.Draw(mSprite, new Rectangle(mPosition.ToPoint(), mSize.ToPoint()), null, Color.White, mRotation, Vector2.Zero, SpriteEffects.None, 0f);
            
            //spriteBatch.Draw(mSprite, mBody.Position - ( new Vector2(25f,25f) ), null, Color.White, mBody.Rotation, Vector2.Zero, mSize, SpriteEffects.None, 1f);
        }
        public bool Intersects(Collidable other)
        {
            Rectangle r1 = new Rectangle((mPosition - (mSize * 0.5f) ).ToPoint(), mSize.ToPoint());
            Rectangle r2 = new Rectangle((other.mPosition - (other.mSize * 0.5f)).ToPoint(), other.mSize.ToPoint());
            return r1.Intersects(r2);
        }
        public virtual void OnCollide(Collidable other)
        {
            if (!mStatic)
            {
                Vector2 dir = mPosition - other.mPosition;
                dir.Normalize();
                //AddForce( dir * mMass );
            }
            //mVelocity = -mVelocity;
            
            int i;
            i = 0;
            ++ i;
        }
        //public virtual void Update(GameTime gameTime)  
        //{
        //    if (mBody.BodyType != BodyType.Static)
        //    {
        //        Vector2 frictiveForce = -mBody.LinearVelocity * mBody.Mass * 0.8f; // 8 is gravity * coefficient of frictoin, 10*0.8, in future can split this depending on the sruface.
        //        Vector2 airResistance = -mBody.LinearVelocity * mBody.LinearVelocity * 0.03f * mSize.Y;
        //        //mBody.ApplyForce(frictiveForce + airResistance);
        //    }
        //}
    }
}
