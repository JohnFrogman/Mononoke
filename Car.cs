using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
using System;
using System.Collections.Generic;

namespace Mononoke
{
    internal class Car
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
        float mHorizontalDamping = 0.6f;

        public RigidBody mBody;
        RigidBody mDoors;
        RigidBody mBootInteractionArea;
        Texture2D mSprite;

        bool mRadioOn = false;

        public Inventory mBoot;
        public Car(Overworld ow, Vector2 pos, Texture2D sprite, Overworld overworld)
        {
            List<Vector2> vertices = new List<Vector2>()
            {
                 new Vector2(-30, 53)
                ,new Vector2(-18, 64)
                ,new Vector2(18, 64)
                ,new Vector2(30, 53)

                ,new Vector2(30, -53)
                ,new Vector2(18, -64)
                ,new Vector2(-18, -64)
                ,new Vector2(-30, -53)
            };
            mBody = RigidBody.BuildPolygon(pos, true, 700, vertices, new Vector2(sprite.Width, sprite.Height) / 2);
            //mBody = RigidBody.BuildRectangle(pos, true, 700, new Vector2(60,128));
            mDoors = RigidBody.BuildRectangle( Vector2.UnitY * sprite.Height * -0.15f, true, 1, new Vector2(100f,30f),true, new Interaction(() => { overworld.EnterCar(this);}, 0.6f), mBody);
            mBootInteractionArea = RigidBody.BuildRectangle( Vector2.UnitY * sprite.Height * 0.3f, true, 1, new Vector2(80f, sprite.Height * 0.5f), true, new Interaction(() => { overworld.OpenBoot(this); }, 0.6f), mBody);
            mBoot = new Inventory("Boot", 10,10);
            mSprite = sprite;
        }
        public Vector2 ExitPos()
        {
            return mBody.mPosition + new Vector2(50f, mSprite.Height * -0.15f).RotateRadians(mBody.mRotation);
        }
        public void Update(GameTime gameTime)
        {
            // We add force to the forward vector, which is considered the forward of the wheels/car, even though this is unrealistic
            float factor = 1- (float)Math.Pow(Math.E, -0.3*Speed());
            mBody.Rotate(-3f * mSteeringPos * (float)gameTime.ElapsedGameTime.TotalSeconds * factor);
            // Stepping on the gas
            mBody.AddForce(mBody.Forward() * mGas * 10000f);
            // To stop drift we can apply a very strong damping to and horizontal movement, this is kind of realistic,
            // because in real life wheels only rotate on one axis and any movement orthogonal to that axis going to
            // experience very high friction, (which if overcome would be a skid). Can add a ceiling to the damping to 
            // allow drift
            HorizontalDamping();
            ApplyBrakes(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            mDoors.Draw(spriteBatch);
            mBootInteractionArea.Draw(spriteBatch);
            //base.Draw(spriteBatch);
            mBody.Draw(spriteBatch);
            spriteBatch.Draw(mSprite, mBody.mPosition, null, Color.White, mBody.mRotation, mBody.mCentre, 1f, SpriteEffects.None, 0f);
            //mBody.DrawVertices(spriteBatch);
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
            return mBody.mVelocity.Magnitude();
        }
        void HorizontalDamping()
        {
            Vector2 fV = mBody.Forward() * Vector2.Dot(mBody.Forward(), mBody.mVelocity);
            Vector2 rV = mBody.Right() * Vector2.Dot(mBody.Right(), mBody.mVelocity);
            mBody.mVelocity = fV + rV * mHorizontalDamping; 
        }

        public void ToggleRadio()
        { 
            mRadioOn = !mRadioOn;
            SoundAssetManager.SetPaused(mRadioOn);
        }
        void ApplyBrakes(GameTime gameTime )
        {
            Vector2 brakingDeceleration = (float)gameTime.ElapsedGameTime.TotalSeconds * mBody.Back() * mBrake * 10f; // Friction from the brake is roughly Brake Pedal Pos * Mass * Gravity, so just don't include mass term to get deceleration from A = f/m
            Vector2 fV = mBody.Forward() * Vector2.Dot(mBody.Forward(), mBody.mVelocity);
            Vector2 rV = mBody.Right() * Vector2.Dot(mBody.Right(), mBody.mVelocity);
            if (brakingDeceleration.Magnitude() > fV.Magnitude())
                mBody.mVelocity = rV;
            else
                mBody.mVelocity += brakingDeceleration;
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
