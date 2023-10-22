using info.lundin.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
using nkast.Aether.Physics2D;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Text.Unicode;

namespace Mononoke
{
    internal class Car : Collidable
    {
        float mThrottle; 
        float mFuel;

        float mEngineRPM;
        float mWheelRPM;
        int CurrentGear = 0;
        float[] mGearCoefficients = new float[6] { 4.0f, 2.2f, 1.4f, 1.2f, 1f, 0.7f };
        float _SteeringPos = 0f;

        float mSteeringPos = 0f;
        float mGas = 0f;
        float mBrake = 0f;

        float mRadius = 2.5f;
        float mWheelBase;
        float mFrontWheelAngle;

        Interactable doors;
        Camera2D mCamera;
        const float EngineConstant = 3000f;
        public Car(World world, Camera2D camera, Vector2 pos, Texture2D sprite)
            : base(world, pos, BodyType.Dynamic, sprite )
        {
            mCamera = camera;
            mSprite = TextureAssetManager.GetCarSpriteByName("car");

            mWheelBase = mSize.Y / 2f;
          
            mBody.LinearDamping = 0.1f;
            mBody.AngularDamping = 1.0f;

            mFrontWheelAngle = mBody.Rotation;

            Vector2 size = new Vector2(60f, 20f);
            Vector2 offset = new Vector2(0, 0);
            Vertices vertices = PolygonTools.CreateRectangle( size.X/2f, size.Y/2f);
            vertices.Translate(offset);
            PolygonShape shape = new PolygonShape(vertices, 0.1f);
            doors = new Interactable(shape,  size, offset);
            mBody.Add(doors);
        }

        //void 
        void Throttle()
        {
            //mBody.Add()
        }
        public void Update(GameTime gameTime)
        {
           // mCamera.Position = -mBody.Position / 2f;
            Vector2 heading = mBody.GetWorldVector(Vector2.UnitY);
            // https://engineeringdotnet.blogspot.com/2010/04/simple-2d-car-physics-in-games.html
            //if ( mSteeringPos != 0 )
            //{
            //    //Vector2 frontWheel = mBody.Position + mWheelBase / 2f * new Vector2((float)Math.Cos(mBody.Rotation), (float)Math.Sin(mBody.Rotation));
            //    //Vector2 backWheel = mBody.Position - mWheelBase / 2f * new Vector2((float)Math.Cos(mBody.Rotation), (float)Math.Sin(mBody.Rotation));

            //    Vector2 frontWheel = mBody.Position + ((mWheelBase/2f ) * mBody.GetWorldVector(Vector2.UnitY));
            //    Vector2 backWheel = mBody.Position - ((mWheelBase/2f )* mBody.GetWorldVector(Vector2.UnitY));

            //    frontWheel = new Vector2(
            //          (float)Math.Cos(mSteeringPos * frontWheel.X) - (float)Math.Sin(mSteeringPos * frontWheel.Y)
            //        , (float)Math.Sin(mSteeringPos * frontWheel.X) + (float)Math.Cos(mSteeringPos * frontWheel.Y));

            //    //backWheel += mBody.LinearVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2((float)Math.Cos(mBody.Rotation), (float)Math.Sin(mBody.Rotation));
            //    //frontWheel += mBody.LinearVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2((float)Math.Cos(mBody.Rotation + mSteeringPos), (float)Math.Sin(mBody.Rotation + mSteeringPos));

            //    //frontWheel += mBody.LinearVelocity;

            //    //carLocation = (frontWheel + backWheel) / 2f;
            //    mBody.Rotation += (float)Math.Atan2(frontWheel.Y - backWheel.Y, frontWheel.X - backWheel.X);
            //}
            // Vector2 wheelForward;
            // wheelForward.X = (float)Math.Cos(bodyForward.X * mSteeringPos) - (float)Math.Sin(mSteeringPos * bodyForward.Y);
            // wheelForward.Y = (float)Math.Sin(bodyForward.X * mSteeringPos) + (float)Math.Cos(mSteeringPos * bodyForward.Y);
            // Vector2 resultantForce = new Vector2(0,0);
            ///
            SetEngineRPM();
            SetWheelRPM();
            //// F = T⋅GR / R⋅2π force applied by the wheels on the car
            //float Force = 
            //mBody.ApplyTorque(mSteeringPos * 1000f);


            //Vector2 frontWheel = mBody.Position + ((mWheelBase / 2f) * mBody.GetWorldVector(Vector2.UnitY));
            //Vector2 backWheel = mBody.Position - ((mWheelBase / 2f) * mBody.GetWorldVector(Vector2.UnitY));

            //Vector2 f2 = mBody.GetWorldVector(Vector2.UnitY); //Vector2.Normalize(frontWheel);
            //if (mSteeringPos != 0)
            //{
            //    f2 = new Vector2(
            //      f2.X * (float)Math.Cos(mSteeringPos) - f2.Y * (float)Math.Sin(mSteeringPos)
            //    , f2.X * (float)Math.Sin(mSteeringPos) + f2.Y * (float)Math.Cos(mSteeringPos));
            //    //mBody.Rotation = mBody.Rotation + mSteeringPos;
            //}

            //frontWheel = frontWheel + f2;

            //mWheelAngle += 
            if (mGas > 0)
            {
                int i;
                i = 0;
                ++i;
            }
            mFrontWheelAngle = mSteeringPos + mBody.Rotation;
            if (mBody.LinearVelocity.Magnitude() < 0.5)
            {
                mBody.LinearVelocity = Vector2.Zero;
                mBody.AngularVelocity = 0;
            }
            if ( mBody.LinearVelocity.Magnitude() > 0 )
            { 
                if ( mFrontWheelAngle < mBody.Rotation )
                {
                    mBody.Rotation += 0.05f;
                }
                else if (mFrontWheelAngle > mBody.Rotation)
                {
                    mBody.Rotation -= 0.05f;
                }
            }
            mBody.ApplyForce(10000f * mBody.GetWorldVector(Vector2.UnitY) * mGas);
        }
        
        void SetEngineRPM()
        {

        }
        void SetWheelRPM()
        { 
        }
        public override void Draw(SpriteBatch spriteBatch) 
        {
            doors.Draw(spriteBatch);
            base.Draw(spriteBatch);
        //    spriteBatch.Draw(mSprite, mBody.Position, null, Color.White, mBody.Rotation, mTextureOrigin, mSize / mTextureSize, SpriteEffects.FlipVertically, 0f);

        //    Vector2 frontWheel = mBody.Position + ((mWheelBase / 2f) * mBody.GetWorldVector(Vector2.UnitY));
        //    Vector2 backWheel = mBody.Position - ((mWheelBase / 2f) * mBody.GetWorldVector(Vector2.UnitY));

        //    Vector2 f2 = Vector2.Normalize(frontWheel);
        //    if ( mSteeringPos != 0 )
        //    { 
        //        f2 = new Vector2(
        //          f2.X * (float)Math.Cos(mSteeringPos) - f2.Y * (float)Math.Sin(mSteeringPos)
        //        , f2.X * (float)Math.Sin(mSteeringPos) + f2.Y * (float)Math.Cos(mSteeringPos));
        //    }
        //    frontWheel = frontWheel + f2;
        //    spriteBatch.Draw(TextureAssetManager.GetSimpleSquare(), frontWheel, null, Color.White, mBody.Rotation, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.FlipVertically, 0f);
        //    spriteBatch.Draw(TextureAssetManager.GetSimpleSquare(), backWheel, null, Color.White, mBody.Rotation, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.FlipVertically, 0f);
        //    //spriteBatch.Draw(mColliderSprite, mBody.Position, null, Color.Green, mBody.Rotation, mColliderTextureOrigin, mSize / mColliderTextureSize, SpriteEffects.FlipVertically, 0f);
        }
        public void SetGas(float gas)
        {
            if (gas < 0f)
                mGas = 0f;
            else if (gas > 1f)
                mGas = 1f;
            else
                mGas = gas;
        }
        public void SetBrake(float brake)
        {
            if (brake < 0f)
                mBrake = 0f;
            else if (brake > 1f)
                mBrake = 1f;
            else
                mBrake = brake;
        }
        public void SetSteer(float pos )
        {
            if (pos < -1f)
                pos = -1f;
            else if (pos > 1f)
                pos = 1f;
            
            // Ranges between -35 and 35 degrees
            //mSteeringPos = 35f * pos;
            mSteeringPos = 0.610865f * pos; // approximation of radians
        }
    }
}
