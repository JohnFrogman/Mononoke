using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System;

namespace Mononoke
{
    public delegate void Interaction();
    internal class Collidable
    {
        protected Texture2D mSprite;
        protected Vector2 mSize;

        Texture2D mColliderSprite;

        public Vector2 mPosition;
        public float mRotation;
        Vector2 mOrigin;
        public bool mStatic;
        Collidable mParent;

        protected float mMass;
        public const int PIXELS_PER_METRE = 26;
        protected Vector2 mVelocity; // units should be metres per second.
        protected float mAngularVelocity; // Radians per second

        protected Vector2 mCurrentForce;
        protected float mNewRotation;

        float mBounce = 0.5f;
        public bool Active = true;
        public bool IsTrigger = false; // Collides but does not cause any physics interactions on collision.
        bool mTriggerActive;
        public float mDrag = 0.99f;

        public Interaction mInteraction;

        public Collidable(Vector2 pos, bool isStatic, Texture2D sprite, float mass, Vector2 size, bool isTrigger = false, Interaction interaction = null, Collidable parent = null)
        {
            if (mass < 0.1f)
                mass = 0.1f;
            IsTrigger = isTrigger;
            mInteraction = interaction;
            mSprite = sprite;
            mStatic = isStatic;
            if (mSprite != null )
            { 
                mSize = new Vector2(mSprite.Width, mSprite.Height);
            }
            else
            { 
                mSize = size;
            }
            mColliderSprite = TextureAssetManager.GetSimpleSquare();
            mMass = mass;
            mPosition = pos;
            mOrigin = pos;
            mParent = parent;
            CollisionManager.RegisterCollidable(this);
        }
        public virtual void Update(GameTime gameTime)
        {
            mTriggerActive = false;
            if (mParent != null)
            { 
                mPosition = mOrigin.RotateRadians(mParent.mRotation) + mParent.mPosition;
                mRotation = mParent.mRotation;
            }

            if (!Active)
                return;
            if (!mStatic)
            { 
                    
                Vector2 newPos = mPosition + PIXELS_PER_METRE * mVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!CollisionManager.Collidies(this))
                { 
                    mPosition = newPos;
                    mVelocity += (float)gameTime.ElapsedGameTime.TotalSeconds * mCurrentForce / mMass;
                    mRotation += mNewRotation;
                    if (mRotation > 2f*Math.PI )
                    {
                        mRotation -= 2f*(float)Math.PI;
                    }
                    if (mRotation <= 2f*(float)Math.PI)
                    {
                        mRotation += 2f * (float)Math.PI;
                    }
                }
                else
                {
                    //mTriggerActive = true;
                    //mVelocity = Vector2.Zero;
                }

                mVelocity *= mDrag;
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
        public Vector2 Back()
        {
            return Vector2.UnitY.RotateRadians(mRotation);
        }
        public Vector2 Left()
        {
            return Vector2.UnitX.RotateRadians(mRotation);
        }
        public Vector2 Right()
        {
            return Vector2.UnitX.RotateRadians(mRotation);
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
            if (!Active)
                return;
            if (Mononoke.SHOW_COLLIDERS)
            { 
                spriteBatch.Draw(mColliderSprite, mPosition, null, mTriggerActive ? Color.AntiqueWhite : Color.Red, mRotation, /*mSize*0.5f*/new Vector2(0.5f,0.5f), mSize, SpriteEffects.None, 0f);
            }
            if (mSprite != null)
            { 
                spriteBatch.Draw(mSprite, mPosition, null, Color.White, mRotation, mSize*0.5f, 1f, SpriteEffects.None, 0f);
            }
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
        // https://developer.mozilla.org/en-US/docs/Games/Techniques/2D_collision_detection
        // https://dyn4j.org/2010/01/sat/
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
            if (IsTrigger && other is Player) 
            {
                mTriggerActive = true;
                return;
            }
            if (other.IsTrigger)
                return;
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
        public void Stop()
        {
            mVelocity = Vector2.Zero;
            mAngularVelocity = 0;
            mCurrentForce = Vector2.Zero;
        }
    }
}
