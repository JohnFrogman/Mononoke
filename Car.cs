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

        //float mRadius = 2.5f;
        float mWheelBase;
        float mFrontWheelAngle;

        Interactable doors;
        const float EngineConstant = 3000f;
        public Car(World world, Vector2 pos, Texture2D sprite, Overworld overworld)
            : base(world, pos, BodyType.Dynamic, sprite )
        {
            mWheelBase = mSize.Y / 2f;
          
            mBody.LinearDamping = 5f;
            mBody.AngularDamping = 50f;

            mFrontWheelAngle = mBody.Rotation;

            Vector2 size = new Vector2(90f, 30f);
            Vector2 offset = new Vector2(-45f, -15f);//new Vector2(20f, );
            doors = new Interactable(size, offset, overworld, () => { overworld.EnterCar(this);}, mBody);
        }

        //void 
        void Throttle()
        {
            //mBody.Add()
        }
        public void Update(GameTime gameTime)
        {
            // apply force to wheels
            // Calculate position of body, average of the two wheel positions
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
