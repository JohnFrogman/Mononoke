using info.lundin.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D;
using nkast.Aether.Physics2D.Dynamics;

namespace Mononoke
{
    internal class Car 
    {
        float mThrottle; 
        float mFuel;

        float mEngineRPM;
        float mDriveRPM;
        float[] mGearCoefficients = new float[6] { 4.0f, 2.2f, 1.4f, 1.2f, 1f, 0.7f };
        float _SteeringPos = 0f;
        Texture2D mSprite;

        float mSteeringPos = 0f;
        float mGas = 0f;
        float mBrake = 0f;

        Body mBody;
        public Car(World world)
        {
            mBody = world.CreateBody(new Vector2(460f, 400f), 0, BodyType.Dynamic);
            Fixture pfixture = mBody.CreateRectangle(5f, 10f, 1f, Vector2.Zero);
            // Give it some bounce and friction
            pfixture.Restitution = 0.3f;
            pfixture.Friction = 20f;
            mSprite = TextureAssetManager.GetSimpleSquare();
        }

        //void 
        void Throttle()
        {
            //mBody.Add()
        }
        public void Update(GameTime gameTime)
        {
            Vector2 resultantForce = new Vector2(0,0);
           // F = T⋅GR / R⋅2π force applied by the wheels on the car
            mBody.ApplyForce(200f * mBody.GetWorldVector(Vector2.UnitY) * mGas);
            mBody.Rotation += mSteeringPos * 0.01f;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Mononoke.Font, "Car", mBody.Position, Color.Black);
            spriteBatch.Draw(mSprite, mBody.Position, null, Color.White, mBody.Rotation, Vector2.Zero, new Vector2(5f, 10f), SpriteEffects.None, 1f);
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
                mSteeringPos = -1f;
            else if (pos > 1f)
                mSteeringPos = 1f;
            else
                mSteeringPos = pos;
        }
    }
}
