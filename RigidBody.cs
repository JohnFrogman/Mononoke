using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;

namespace Mononoke
{
    public delegate void OnUse();
    public class Interaction
    {
        public Interaction(OnUse use, float duration) 
        { 
            Duration = duration;
            Use = use;
        }
        public float Duration = -1.0f;
        public OnUse Use;
    }
    public class RigidBody
    {
        Texture2D mColliderSprite;

        public Vector2 mPosition;
        private Vector2 mPreviousPosition;
        public float mRotation;
        private float mPreviousRotation;
        Vector2 mOrigin;
        public RigidBody mParent;
        protected Vector2 mSize; 

        protected float mMass;
        public const int PIXELS_PER_METRE = 26;
        protected Vector2 mVelocity; // units should be metres per second.
        protected float mAngularVelocity; // Radians per second

        protected Vector2 mCurrentForce;
        protected float mNewRotation;

        float mBounce = 0.5f;
        public bool Active = true; // Inactive, not doing anyhting, doesnt collide, doesnt move, doesnt interact
        public bool mStatic = false; // Static objects collide but will not move themselves.
        public bool IsTrigger = false; // Collides but does not cause any physics interactions on collision.
        bool mTriggerActive;
        public float mDrag = 0.99f;

        List<Vector2> mVertices = new();

        public Interaction mInteraction;
        List<RigidBody> PreviousOthers = new();

        public RigidBody(Vector2 pos, bool isStatic, float mass, Vector2 size, bool isTrigger = false, Interaction interaction = null, RigidBody parent = null)
        {
            if (mass < 0.1f)
                mass = 0.1f;
            IsTrigger = isTrigger;
            mInteraction = interaction;            
            mStatic = isStatic;
            mSize = size;
            mVertices = new List<Vector2>() {
                 new Vector2(-size.X * 0.5f, -size.Y * 0.5f)
                ,new Vector2( size.X * 0.5f, -size.Y * 0.5f)
                ,new Vector2( size.X * 0.5f,  size.Y * 0.5f)
                ,new Vector2(-size.X * 0.5f,  size.Y * 0.5f)
            };
            mColliderSprite = TextureAssetManager.GetSimpleSquare();
            mMass = mass;
            mPosition = pos;
            mOrigin = pos;
            mParent = parent;
            PhysicsManager.RegisterBody(this);
        }
        public virtual void Collision(List<RigidBody> others)
        {
            foreach (RigidBody previousOther in PreviousOthers)
            {
                // If previous others has a member not in the new others then collision has ended
                if (!others.Contains(previousOther))
                {
                    OnCollisionEnd(previousOther);
                }
            }
            foreach (RigidBody other in others)
            {
                // If there was a previous rigid body and we're now hitting a different one
                if (!PreviousOthers.Contains(other))
                {
                    OnCollisionStart(other);
                }
                OnCollision(other); // Fires every frame we're hitting
            }
            PreviousOthers = others;
        }
        // Only call from child or Physics manager, once per frame!!!
        public virtual void DoStep(GameTime gameTime)
        {
            if (this is Player)
            {
                Debug.WriteLine("Position: " + mPosition.ToString());
                Debug.WriteLine("Previous Position: " + mPreviousPosition.ToString());
                Debug.WriteLine("------");
            }
            mPreviousPosition = mPosition;
            mPreviousRotation = mRotation;
            if (mParent != null)
            {
                mPosition = mOrigin.RotateRadians(mParent.mRotation) + mParent.mPosition;
                mRotation = mParent.mRotation;
            }
            Vector2 newPos = mPosition + PIXELS_PER_METRE * mVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mPosition = newPos;
            mVelocity += (float)gameTime.ElapsedGameTime.TotalSeconds * mCurrentForce / mMass;
            mRotation += mNewRotation;
            if (mRotation > 2f * Math.PI)
            {
                mRotation -= 2f * (float)Math.PI;
            }
            if (mRotation <= 2f * (float)Math.PI)
            {
                mRotation += 2f * (float)Math.PI;
            }
            mVelocity *= mDrag;
            //Friction();
            //AirResistance();
            mNewRotation = 0;
            mCurrentForce = Vector2.Zero;
        }
        void UndoStep()
        {
            mPosition = mPreviousPosition;
            mRotation = mPreviousRotation;
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
                spriteBatch.Draw(mColliderSprite, mPosition, null, mTriggerActive ? new Color(new Vector4(255, 0, 255, 125)) : new Color( new Vector4(0, 255, 255, 125)), mRotation, /*mSize*0.5f*/new Vector2(0.5f,0.5f), mSize, SpriteEffects.None, 0f);
            }
            //if (mSprite != null)
            //{ 
            //    spriteBatch.Draw(mSprite, mPosition, null, Color.White, mRotation, mSize*0.5f, 1f, SpriteEffects.None, 0f);
            //}
          // spriteBatch.Draw(mSprite, new Rectangle(mPosition.ToPoint(), mSize.ToPoint()), null, Color.White, mRotation, Vector2.Zero, SpriteEffects.None, 0f);
            
            //spriteBatch.Draw(mSprite, mBody.Position - ( new Vector2(25f,25f) ), null, Color.White, mBody.Rotation, Vector2.Zero, mSize, SpriteEffects.None, 1f);
        }
        public Vector2 Centre()
        {
            return mSize * 0.5f;
            Vector2 result = Vector2.Zero;
            foreach (Vector2 v in mVertices)
            {
                result += v;
            }
            return result / mVertices.Count;
        }
        public List<Vector2> Vertices()
        {
            List<Vector2> result = new();
            foreach (Vector2 v in mVertices) 
            { 
                result.Add(v.RotateRadians(mRotation) + mPosition);
            }
            return result;
        }
        // https://developer.mozilla.org/en-US/docs/Games/Techniques/2D_collision_detection
        // https://dyn4j.org/2010/01/sat/
        public bool Intersects(RigidBody other)
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
        
        public virtual void OnCollisionStart(RigidBody other)
        {
            if (IsTrigger && other is Player) 
            {
                mTriggerActive = true;
                return;
            }      
            if (!IsTrigger && !other.IsTrigger)
            { 
                UndoStep();
            }
        }
        void PushOut(RigidBody other)
        {
            Vector2 dir = mPosition - other.mPosition;
            if (dir == Vector2.Zero)
                dir = Vector2.UnitX; // if we land directly in the middle then we could get stuck
            dir.Normalize();
            mPosition += dir;
            mVelocity = -mVelocity * mBounce;
            //AddForce( dir * mMass );
        }
        public virtual void OnCollision(RigidBody other)
        {
            // If it's not static and we're hitting something that isnt a trigger push the collider away/out of the other one
            if (!mStatic && !other.IsTrigger)
            { 
                PushOut(other);
            }
        }
        public virtual void OnCollisionEnd(RigidBody other)
        {
            if (IsTrigger && other is Player)
            {
                mTriggerActive = false;
                return;
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
