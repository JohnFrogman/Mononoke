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
        //float mFuel;

        //float mEngineRPM;
        //float mWheelRPM;
        //int CurrentGear = 0;
        //float[] mGearCoefficients = new float[6] { 4.0f, 2.2f, 1.4f, 1.2f, 1f, 0.7f };
        //float _SteeringPos = 0f;

        float mSteeringPos = 0f;
        float mGas = 0f;
        float mBrake = 0f;

        Interactable doors;
        public Car(World world, Vector2 pos, Texture2D sprite, Overworld overworld)
            : base(pos, false, sprite, 700)
        {
            //doors = new Interactable(size, offset, overworld, () => { overworld.EnterCar(this);}, mBody);
        }

        public override void Update(GameTime gameTime)
        {
            //Vector2 forwardVelocity = mBody.GetWorldVector(Vector2.UnitY) * ( Vector2.Dot(mBody.GetWorldVector(Vector2.UnitY), mBody.LinearVelocity));
            //Vector2 rightVelocity = mBody.GetWorldVector(Vector2.UnitX) * (Vector2.Dot(mBody.GetWorldVector(Vector2.UnitX), mBody.LinearVelocity));

            ////mBody.LinearVelocity = forwardVelocity + 0.95f * rightVelocity;
            
            ////mBody.LinearVelocity += 300000f * (float)gameTime.ElapsedGameTime.TotalSeconds * mGas * mBody.GetWorldVector(Vector2.UnitY);
            //mBody.ApplyForce(mBody.GetWorldVector(Vector2.UnitY) * 9999999999f * mGas);
            ////float r = Math.Clamp((float)(mBody.LinearVelocity.Magnitude() / 8f), 0f, 1f );
            
            ////mBody.Rotation -= r * mSteeringPos * 2.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //mBody.ApplyTorque(-mSteeringPos * 500f);
            HandleControls();
            mRotation -= 3f * mSteeringPos * (float)gameTime.ElapsedGameTime.TotalSeconds;
            AddForce(Forward() * mGas * 10000f);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch) 
        {
         //   doors.Draw(spriteBatch);
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
