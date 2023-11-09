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
    enum ePlayerAnimationState
    { 
        Idle,
        Walking,
        OpenDoor
    }
    internal class Player : RigidBody
    {
        Overworld mOverworld;
        Car mCar;
        Camera2D mCamera;
        KeyboardState mPreviousKeyboardState;
        public Interaction mActiveInteraction;
        RigidBody mCameraFocus;
        Animator mAnimator;
        const float mWalkSpeed = 2.5f;
        public bool Enabled = true; // disabled means can only do interaction input which will reenable the player
        public Player(Vector2 pos, Overworld overworld, Camera2D camera, GraphicsDeviceManager graphics) 
            : base( pos, false, 1, new Vector2(20, 17) )
        {
            mCameraFocus = this;
            mCamera = camera;
            mOverworld = overworld;
            mAnimator = new Animator();
            mAnimator.AddAnimation((int)ePlayerAnimationState.Walking, new Animation(this, "data/textures/units/player_walk_cycle.json", graphics));
            mAnimator.AddAnimation((int)ePlayerAnimationState.Idle, new Animation(this, "data/textures/units/player_idle.json", graphics));
            mAnimator.AddAnimation((int)ePlayerAnimationState.OpenDoor, new Animation(this, "data/textures/units/player_grab.json", graphics, (int)ePlayerAnimationState.Idle));
            //mAnimator.AddAnimation((int)ePlayerAnimationState.Idle, new Animation(this, "data/textures/units/player_walk_cycle.json", graphics));
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
            mAnimator.Update(gameTime);
            mCamera.Position = -mCameraFocus.mPosition + new Vector2(960, 540);
            KeyboardState state = Keyboard.GetState();
            bool animationSet = false;
            if (mAnimator.GetCurrentState() != (int)ePlayerAnimationState.OpenDoor)
            {
                mAnimator.SetCurrentState((int)ePlayerAnimationState.Idle);
            }
            if (Enabled)
            {
                if (state.IsKeyUp(Keys.E) && mPreviousKeyboardState.IsKeyDown(Keys.E))
                {
                    if (mCar == null)
                    { 
                        mAnimator.SetCurrentState((int)ePlayerAnimationState.OpenDoor);
                        if (mActiveInteraction != null)
                        {
                            mActiveInteraction();
                        }
                    }
                    else
                    {
                        ExitCar(mCar);
                    }
                }
                mActiveInteraction = null;
                if ( mCar == null)
                {
                    HandleWalkInput(state);
                }
                else
                {
                    HandleCarInput(state);
                }
                base.Update(gameTime);
            }
            mPreviousKeyboardState = state;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mCar == null)
            {
                mAnimator.Draw(spriteBatch);
                base.Draw(spriteBatch);
            }
        }
        // -Vector2.UnitY.RotateRadians(mRotation);
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
            if (Vector2.Zero != resultantVelocity)
            {
                resultantVelocity.Normalize();
                mAnimator.SetCurrentState((int)ePlayerAnimationState.Walking);
                mVelocity = resultantVelocity * mWalkSpeed;
                mRotation = -mVelocity.ClockwiseAngleBetween(Vector2.UnitY);
            }
            else
            {
                mVelocity = Vector2.Zero;
            }
                //AddFdorce(resultantVelocity);
                //mVelocity += resultantVelocity * 3f;
        }
        void HandleCarInput(KeyboardState state)
        {
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
        public override void OnCollide(RigidBody other)
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
