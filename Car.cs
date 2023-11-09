﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
using System;

namespace Mononoke
{
    internal class Car : RigidBody
    {
        //float mFuel;

        //float mEngineRPM;
        //float mWheelRPM;
        //int CurrentGear = 0;
        //float[] mGearCoefficients = new float[6] { 4.0f, 2.2f, 1.4f, 1.2f, 1f, 0.7f };
        //float _SteeringPos = 0f;

        float mSteeringPos = 0f;
        float mGas = 0f;
        float mBrake = 0f;
        int mWheelBase = 45;
        float mHorizontalDamping = 0.6f;

        RigidBody mDoors;
        RigidBody mBootInteractionArea;
        Texture2D mSprite;

        public Inventory mBoot;
        public Car(Overworld ow, Vector2 pos, Texture2D sprite, Overworld overworld)
            : base(pos, false, 700, new Vector2(sprite.Width, sprite.Height))
        {
            mStatic = true;
            mDoors = new RigidBody( Vector2.UnitY * sprite.Height * -0.15f, true, 1, new Vector2(100f,30f),true, () => { overworld.EnterCar(this);}, this);
            mBootInteractionArea = new RigidBody( Vector2.UnitY * sprite.Height * 0.3f, true, 1, new Vector2(80f, sprite.Height * 0.5f), true, () => { overworld.OpenBoot(this); }, this);
            mBoot = new Inventory(10,10);
            mSprite = sprite;
        }
        public Vector2 ExitPos()
        {
            return mPosition + new Vector2(50f, mSprite.Height * -0.15f).RotateRadians(mRotation);
        }
        public override void Update(GameTime gameTime)
        {
            mDoors.Update(gameTime);
            mBootInteractionArea.Update(gameTime);
            
            // We add force to the forward vector, which is considered the forward of the wheels/car, even though this is unrealistic
            float factor = 1- (float)Math.Pow(Math.E, -0.3*Speed());
            Rotate(-3f * mSteeringPos * (float)gameTime.ElapsedGameTime.TotalSeconds * factor);
            // Stepping on the gas
            AddForce(Forward() * mGas * 10000f);
            // To stop drift we can apply a very strong damping to and horizontal movement, this is kind of realistic,
            // because in real life wheels only rotate on one axis and any movement orthogonal to that axis going to
            // experience very high friction, (which if overcome would be a skid). Can add a ceiling to the damping to 
            // allow drift
            HorizontalDamping();

            ApplyBrakes(gameTime);


            //mVelocity += (float)gameTime.ElapsedGameTime.TotalSeconds * mCurrentForce / mMass;
            //https://engineeringdotnet.blogspot.com/2010/04/simple-2d-car-physics-in-games.html
            //Vector2 frontWheel = mPosition + mWheelBase * new Vector2((float)Math.Cos(mRotation), (float)Math.Sin(mRotation));
            //Vector2 backWheel = mPosition - mWheelBase * new Vector2((float)Math.Cos(mRotation), (float)Math.Sin(mRotation));
            //backWheel += mVelocity * PIXELS_PER_METRE * (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2((float)Math.Cos(mRotation), (float)Math.Sin(mRotation));
            //frontWheel += mVelocity * PIXELS_PER_METRE * (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2((float)Math.Cos(mRotation + mSteeringPos), (float)Math.Sin(mRotation + mSteeringPos));
            //mPosition = (frontWheel + backWheel) / 2;
            //mRotation = (float)Math.Atan2(frontWheel.Y - backWheel.Y, frontWheel.X - backWheel.X);
            //CollisionManager.Collidies(this);
            //mVelocity *= mDrag;

            base.Update(gameTime);
            //HandleControls();
        }
        public override void Draw(SpriteBatch spriteBatch) 
        {
            mDoors.Draw(spriteBatch);
            mBootInteractionArea.Draw(spriteBatch);
            //base.Draw(spriteBatch);
            spriteBatch.Draw(mSprite, mPosition, null, Color.White, mRotation, Centre(), 1f, SpriteEffects.None, 0f);
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
            
            mSteeringPos = 0.610865f * pos; // approximation of radians
        }
        public float Speed()
        {
            return mVelocity.Magnitude();
        }
        void HorizontalDamping()
        {
            Vector2 fV = Forward() * Vector2.Dot(Forward(), mVelocity);
            Vector2 rV = Right() * Vector2.Dot(Right(), mVelocity);
            mVelocity = fV + rV * mHorizontalDamping; 
        }
        void ApplyBrakes(GameTime gameTime )
        {
            Vector2 brakingDeceleration = (float)gameTime.ElapsedGameTime.TotalSeconds * Back() * mBrake * 10f; // Friction from the brake is roughly Brake Pedal Pos * Mass * Gravity, so just don't include mass term to get deceleration from A = f/m
            Vector2 fV = Forward() * Vector2.Dot(Forward(), mVelocity);
            Vector2 rV = Right() * Vector2.Dot(Right(), mVelocity);
            if (brakingDeceleration.Magnitude() > fV.Magnitude())
                mVelocity = rV;
            else
                mVelocity += brakingDeceleration;
        }
        //void HandleControls()
        //{
        //    KeyboardState state = Keyboard.GetState();
        //    if (state.IsKeyDown(Keys.D))
        //    {
        //        SetSteer(-1.0f);
        //    }
        //    else if (state.IsKeyDown(Keys.A))
        //    {
        //        SetSteer(1.0f);
        //    }
        //    else
        //    {
        //        SetSteer(0.0f);
        //    }

        //    if (state.IsKeyDown(Keys.W))
        //    {
        //        SetGas(1.0f);
        //    }
        //    else
        //    {
        //        SetGas(0f);
        //    }

        //    if (state.IsKeyDown(Keys.S))
        //    {
        //        SetBrake(1.0f);
        //    }
        //    else
        //    {
        //        SetBrake(0f);
        //    }
        //}

    }
}
