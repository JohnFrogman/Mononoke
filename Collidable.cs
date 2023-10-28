using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using nkast.Aether.Physics2D.Collision.Shapes;

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
        float mNewRotation;

        float mBounce = 0.5f;
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
                if (!CollisionManager.Collidies(this))
                { 
                    mPosition = newPos;
                    mVelocity += (float)gameTime.ElapsedGameTime.TotalSeconds * mCurrentForce / mMass;
                    mRotation += mNewRotation;
                }
                else
                {
                    //mVelocity = Vector2.Zero;
                }

                //Friction();
                //AirResistance();
                mNewRotation = 0;
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
        public List<Vector2> Vertices()
        {
            List<Vector2> v = new List<Vector2>() {
                new Vector2(-mSize.X * 0.5f, -mSize.Y * 0.5f)
                ,new Vector2(mSize.X * 0.5f, -mSize.Y * 0.5f)
                ,new Vector2(mSize.X * 0.5f, mSize.Y * 0.5f)
                ,new Vector2(-mSize.X * 0.5f, mSize.Y * 0.5f)
            };
            v[0] = v[0].RotateRadians(mRotation) + mPosition;
            v[1] = v[1].RotateRadians(mRotation) + mPosition;
            v[2] = v[2].RotateRadians(mRotation) + mPosition;
            v[3] = v[3].RotateRadians(mRotation) + mPosition;
            return v;
        }
        public bool Intersects(Collidable other)
        {
            List<Vector2> v1 = Vertices();
            List<Vector2> v2 = other.Vertices();
            // AS it's a rectangle there are only 2 axes in each collidable
            List<Vector2> axes = new() {
                (v1[0] - v1[1]).PerpendicularClockwise()
            ,   (v1[1] - v1[2]).PerpendicularClockwise()
            //,   (v1[2] - v1[3]).PerpendicularClockwise()
            //,   (v1[3] - v1[0]).PerpendicularClockwise()

            ,   (v2[0] - v2[1]).PerpendicularClockwise()
            ,   (v2[1] - v2[2]).PerpendicularClockwise()
            //,   (v2[2] - v2[3]).PerpendicularClockwise()
            //,   (v2[3] - v2[0]).PerpendicularClockwise()
            };
            foreach (Vector2 axis in axes)
            {
                float minV1 = Vector2.Dot(axis, v1[0]);
                float maxV1 = minV1;
                float minV2 = Vector2.Dot(axis, v2[0]);
                float maxV2 = minV2;
                for (int i = 0; i < 4; ++i)
                {
                    float p = Vector2.Dot(v1[i], axis);
                    if (p < minV1)
                    {
                        minV1 = p;
                    }
                    else if (p > maxV1)
                    {
                        maxV1 = p;
                    }

                    p = Vector2.Dot(v2[i], axis);
                    if (p < minV2)
                    {
                        minV2 = p;
                    }
                    else if (p > maxV2)
                    {
                        maxV2 = p;
                    }
                }
                if (maxV1 < minV2 || maxV2 < minV1)
                {
                    return false;
                }
            }
            return true;
        }
        public virtual void OnCollide(Collidable other)
        {
            // If it's not static push the collider away/out of the other one
            if (!mStatic)
            {
                Vector2 dir = mPosition - other.mPosition;
                dir.Normalize();
                mPosition += dir;
                mVelocity = -mVelocity * mBounce;
                //AddForce( dir * mMass );
            }
        }
        protected void Rotate(float rotation)
        {
            mNewRotation = rotation;
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
