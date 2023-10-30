using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualBasic;

namespace Mononoke
{
    internal class Player : Collidable
    {
        Overworld mOverworld;
        Car mCar;
        Camera2D mCamera;
        KeyboardState mPreviousKeyboardState;
        Interaction mActiveInteraction;
        Collidable mCameraFocus;
        public Player(Vector2 pos, Overworld overworld, Camera2D camera) 
            : base( pos, false,TextureAssetManager.GetPlayerSprite(), 1, Vector2.Zero )
        {
            mCameraFocus = this;
            mCamera = camera;
            mOverworld = overworld;
        }
        public Rectangle Rectangle()
        {
            return new Rectangle((mPosition - mSize/2f).ToPoint(), mSize.ToPoint());
        }
        public void EnterCar(Car car)
        {
            mCameraFocus = car;
            car.mStatic = false;
            mCar = car;
            Active = false;
            mActiveInteraction = () => { ExitCar(car); };
            mDrag = 0f;
            //foreach (Fixture f in mBody.FixtureList)
            //  f.CollidesWith = Category.None;
        }
        public void ExitCar(Car car)
        {
            mCameraFocus = this;
            car.mStatic = true;
            car.Stop();
            mPosition = car.ExitPos();
            Active = true;
            mCar = null;
        }
        public override void Update(GameTime gameTime)
        {
            mCamera.Position = -mCameraFocus.mPosition + new Vector2(960, 540);
            KeyboardState state = Keyboard.GetState();
            // We make it possible to rotate the player body
            if ( mCar == null)
            {
                HandleWalkInput(state);
            }
            else
            {
               //mPosition = mCar.Position;
                if (state.IsKeyDown(Keys.E))
                {
                    mCar = null;
                    return;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    mCar.SetSteer(-1.0f);
                }
                else if (state.IsKeyDown(Keys.A))
                {
                    mCar.SetSteer(1.0f);
                }
                else
                {
                    mCar.SetSteer(0.0f);
                }

                if (state.IsKeyDown(Keys.W))
                {
                    mCar.SetGas(1.0f);
                }
                else
                {
                    mCar.SetGas(0f);
                }

                if (state.IsKeyDown(Keys.S))
                {
                    mCar.SetBrake(1.0f);
                }
                else
                {
                    mCar.SetBrake(0f);
                }
            }
            mPreviousKeyboardState = state;
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mCar == null)
                base.Draw(spriteBatch);

        }
        void HandleWalkInput(KeyboardState state)
        {
            Vector2 resultantVelocity = Vector2.Zero;
            if (state.IsKeyDown(Keys.D) )//&& mPreviousKeyboardState.IsKeyDown(Keys.A))
            {
                resultantVelocity += new Vector2(1,0);
            }
            if (state.IsKeyDown(Keys.A) )//&& mPreviousKeyboardState.IsKeyDown(Keys.D))
            {
                resultantVelocity += new Vector2(-1, 0);
            }
            if (state.IsKeyDown(Keys.W) )//&& mPreviousKeyboardState.IsKeyDown(Keys.W))
            {
                resultantVelocity += new Vector2(0, -1);
            }
            if (state.IsKeyDown(Keys.S) )//&& mPreviousKeyboardState.IsKeyDown(Keys.S))
            {
                resultantVelocity += new Vector2(0, 1);
            }
            if (state.IsKeyUp(Keys.E) && mPreviousKeyboardState.IsKeyDown(Keys.E))
            {
                if (mActiveInteraction != null)
                {
                    mActiveInteraction();
                }
            }
            //AddFdorce(resultantVelocity);
            //mVelocity += resultantVelocity * 3f;
            mVelocity = resultantVelocity * 3f ;
        }
        public override void OnCollide(Collidable other)
        { 
            if (other.IsTrigger)
            {
                mActiveInteraction = other.mInteraction;
            }
            base.OnCollide(other);
        }
        void Interact()
        { 
        }
    }
}
