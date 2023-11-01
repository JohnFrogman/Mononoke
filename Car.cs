using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
using System;

namespace Mononoke
{
    internal class Car : Collidable
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

        Collidable mDoors;
        Collidable mBootInteractionArea;

        public Inventory mBoot;
        public Car(Overworld ow, Vector2 pos, Texture2D sprite, Overworld overworld)
            : base(pos, false, sprite, 700, Vector2.Zero)
        {
            mStatic = true;
            mDoors = new Collidable( Vector2.UnitY * sprite.Height * -0.15f, true, null, 1, new Vector2(100f,30f),true, () => { overworld.EnterCar(this);}, this);
            mBootInteractionArea = new Collidable( Vector2.UnitY * sprite.Height * 0.3f, true, null, 1, new Vector2(80f, sprite.Height * 0.5f), true, () => { overworld.OpenBoot(this); }, this);
            mBoot = new Inventory(10,10);
        }
        public Vector2 ExitPos()
        {
            return mPosition + new Vector2(50f, mSprite.Height * -0.15f).RotateRadians(mRotation);
        }
        public override void Update(GameTime gameTime)
        {
            mDoors.Update(gameTime);
            mBootInteractionArea.Update(gameTime);

            Vector2 frontWheel = mWheelBase * Forward();
            Vector2 backWheel = -mWheelBase * Forward();
            backWheel += mVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Forward();
            frontWheel += mVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Forward().RotateRadians(mSteeringPos);
            mRotation = (float)Math.Atan2(frontWheel.Y - backWheel.Y, frontWheel.X - backWheel.X);
            backWheel += mPosition;
            frontWheel += mPosition;
            mPosition = (frontWheel + backWheel) / 2f;

            CollisionManager.Collidies(this);
            mVelocity *= mDrag;
            mCurrentForce = Vector2.Zero;

            //Vector2 rear_wheel = mPosition - mPosition.X * mWheelBase;
            //Vector2 front_wheel = position + mPosition.X * mWheelBase;
            //rear_wheel += velocity * delta
            //front_wheel += velocity.rotated(steer_angle) * delta
            //var new_heading = (front_wheel - rear_wheel).normalized()
            //velocity = new_heading * velocity.length()
            //rotation = new_heading.angle()

            //Rotate(-3f * mSteeringPos * (float)gameTime.ElapsedGameTime.TotalSeconds);


            AddForce(Forward() * mGas * 10000f);
            //HandleControls();
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch) 
        {
            mDoors.Draw(spriteBatch);
            mBootInteractionArea.Draw(spriteBatch);
            base.Draw(spriteBatch);
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
        void HandleControls()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.D))
            {
                SetSteer(-1.0f);
            }
            else if (state.IsKeyDown(Keys.A))
            {
                SetSteer(1.0f);
            }
            else
            {
                SetSteer(0.0f);
            }

            if (state.IsKeyDown(Keys.W))
            {
                SetGas(1.0f);
            }
            else
            {
                SetGas(0f);
            }

            if (state.IsKeyDown(Keys.S))
            {
                SetBrake(1.0f);
            }
            else
            {
                SetBrake(0f);
            }
        }
    }
}
