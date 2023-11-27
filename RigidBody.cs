using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata.Ecma335;

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
        protected Texture2D mColliderSprite;

        // Position is in metres not pixels.
        public const int PIXELS_PER_METRE = 26;
        public Vector2 mPosition; 
        private Vector2 mPreviousPosition;
        // Rotation in radians
        public float mRotation;
        private float mPreviousRotation;
        Vector2 mOrigin;
        public RigidBody mParent;
        public Vector2 mSize; 

        protected float mMass;
        public Vector2 mVelocity; // units should be metres per second.
        protected float mAngularVelocity; // Radians per second

        protected Vector2 mCurrentForce;
        protected float mNewRotation;

        float mBounce = 0.5f;
        public bool Active = true; // Inactive, not doing anyhting, doesnt collide, doesnt move, doesnt interact
        public bool mStatic = false; // Static objects collide but will not move themselves.
        public bool IsTrigger = false; // Collides but does not cause any physics interactions on collision.
        bool mTriggerActive;
        public float mDrag = 0.99f;
        public Vector2 mCentre = Vector2.Zero;

        protected List<Vector2> mVertices = new();

        public Interaction mInteraction;
        List<RigidBody> PreviousOthers = new();
        private bool v1;
        private int v2;
        public bool ActivatesTriggers = false;

        public static RigidBody BuildRectangle(Vector2 pos, bool isStatic, float mass, Vector2 size, bool isTrigger = false, Interaction interaction = null, RigidBody parent = null)
        {
            List<Vector2> vertices = new List<Vector2>()
            {
                 new Vector2(-size.X * 0.5f, -size.Y * 0.5f)
                ,new Vector2( size.X * 0.5f, -size.Y * 0.5f)
                ,new Vector2( size.X * 0.5f,  size.Y * 0.5f)
                ,new Vector2(-size.X * 0.5f,  size.Y * 0.5f)
            };
            return new RigidBody(pos, isStatic, mass, vertices, size/2, isTrigger, interaction, parent);
        }
        public static RigidBody BuildPolygon(Vector2 pos, bool isStatic, float mass, List<Vector2> vertices, Vector2 centre, bool isTrigger = false, Interaction interaction = null, RigidBody parent = null)
        {
            return new RigidBody(pos, isStatic, mass, vertices, centre, isTrigger, interaction, parent);
        }
        protected RigidBody(Vector2 pos, bool isStatic, float mass, List<Vector2> vertices, Vector2 centre,bool isTrigger = false, Interaction interaction = null, RigidBody parent = null)
        {
            if (mass < 0.1f)
                mass = 0.1f;
            IsTrigger = isTrigger;
            mInteraction = interaction;
            mStatic = isStatic;
            mVertices = vertices;
            mColliderSprite = TextureAssetManager.GetSimpleSquare();
            mMass = mass;
            mPosition = pos;
            mOrigin = pos;
            mParent = parent;
            mCentre = centre;
            mSize = centre *2;
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
            if (mRotation <= -2f * (float)Math.PI)
            {
                mRotation += 2f * (float)Math.PI;
            }
            mVelocity *= mDrag;
            //Friction();
            //AirResistance();
            mNewRotation = 0;
            mCurrentForce = Vector2.Zero;
        }
        void UndoStep(RigidBody other)
        {
            mPosition = mPreviousPosition;
            mRotation = mPreviousRotation;
            if (Intersects(other))
            { 
                PushOut(other);
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
                spriteBatch.Draw(mColliderSprite, mPosition, null, mTriggerActive ? new Color(new Vector4(255, 0, 255, 125)) : new Color( new Vector4(0, 255, 255, 125)), mRotation, /*mSize*0.5f*/new Vector2(0.5f,0.5f), mSize, SpriteEffects.None, 0f);
            }
            if (Mononoke.SHOW_VERTICES)
            {
                DrawVertices(spriteBatch);   
            }
            //if (mSprite != null)
            //{ 
            //    spriteBatch.Draw(mSprite, mPosition, null, Color.White, mRotation, mSize*0.5f, 1f, SpriteEffects.None, 0f);
            //}
          // spriteBatch.Draw(mSprite, new Rectangle(mPosition.ToPoint(), mSize.ToPoint()), null, Color.White, mRotation, Vector2.Zero, SpriteEffects.None, 0f);
            
            //spriteBatch.Draw(mSprite, mBody.Position - ( new Vector2(25f,25f) ), null, Color.White, mBody.Rotation, Vector2.Zero, mSize, SpriteEffects.None, 1f);
        }
        public void DrawVertices(SpriteBatch spriteBatch)
        {
            List<Vector2> vertices = Vertices();
            int i = 0;
            int step = 265 / vertices.Count;
            foreach (Vector2 v in vertices)
            {
                spriteBatch.Draw(mColliderSprite, v, null, new Color(255, 0, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(mColliderSprite, new Rectangle((int)v.X, (int)v.Y, 3, 3), new Color(i,0,i));
                i += step;
                //spriteBatch.Draw(mColliderSprite, v, null, Color.Red, 0f, new Vector2(1.5f, 1.5f), 1f, SpriteEffects.None, 0f);
            }
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

            List<Vector2> axes = new();

            for (int i = 0; i < v1.Count; ++i)
            {
                Vector2 a = v1[i];
                Vector2 b;
                if (i == v1.Count - 1)
                    b =v1[0];
                else
                    b = v1[i+1];
                axes.Add((a - b).PerpendicularClockwise());
            }
            for (int i = 0; i < v2.Count; ++i)
            {
                Vector2 b;
                Vector2 a = v2[i];
                if (i == v2.Count - 1)
                    b = v2[0];
                else
                    b = v2[i + 1];
                axes.Add((a - b).PerpendicularClockwise());
            }
            // Commented out code: 
            //// AS it's a rectangle there are only 2 axes in each collidable
            //List<Vector2> axes = new() {
            //    (v1[0] - v1[1]).PerpendicularClockwise()
            //,   (v1[1] - v1[2]).PerpendicularClockwise()
            ////,   (v1[2] - v1[3]).PerpendicularClockwise()
            ////,   (v1[3] - v1[0]).PerpendicularClockwise()

            //,   (v2[0] - v2[1]).PerpendicularClockwise()
            //,   (v2[1] - v2[2]).PerpendicularClockwise()
            ////,   (v2[2] - v2[3]).PerpendicularClockwise()
            ////,   (v2[3] - v2[0]).PerpendicularClockwise()
            //};
            foreach (Vector2 axis in axes)
            {
                float minV1 = Vector2.Dot(axis, v1[0]);
                float maxV1 = minV1;
                float minV2 = Vector2.Dot(axis, v2[0]);
                float maxV2 = minV2;
                for (int i = 0; i < v1.Count; ++i)
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
                }
                for (int i = 0; i < v2.Count; ++i)
                {
                    float p = Vector2.Dot(v2[i], axis);
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
        void PushOut(RigidBody other)
        {
            Vector2 dir = mPosition - other.mPosition;
            if (dir == Vector2.Zero)
                dir = Vector2.UnitX; // if we land directly in the middle then we could get stuck
            dir.Normalize();
            mPosition += dir;
            //AddForce( dir * mMass );
        }
        public virtual void OnCollisionStart(RigidBody other)
        {
            if (IsTrigger && other.ActivatesTriggers) 
            {
                mTriggerActive = true;
                Overworld.mPlayer.mActiveInteraction = mInteraction;
                return;
            }      
            if (!IsTrigger && !other.IsTrigger && !mStatic)
            { 
                //PushOut(other);
                //UndoStep();
                Vector2 dir = mPosition - other.mPosition;
                if (dir == Vector2.Zero)
                    dir = Vector2.UnitX; // if we land directly in the middle then we could get stuck
                dir.Normalize();
                mPosition += dir;
                mVelocity = -mVelocity * mBounce;
            }
        }
        public virtual void OnCollision(RigidBody other)
        {
            // If it's not static and we're hitting something that isnt a trigger push the collider away/out of the other one
            if (!mStatic && !IsTrigger && !other.IsTrigger)
            { 
                UndoStep(other);
            }
        }
        public virtual void OnCollisionEnd(RigidBody other)
        {
            if (IsTrigger && other.ActivatesTriggers)
            {
                Overworld.mPlayer.mActiveInteraction = null;
                mTriggerActive = false;
                return;
            }
        }
        public void Rotate(float rotation)
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
