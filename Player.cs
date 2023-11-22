using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualBasic;
using System.Net.Sockets;

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
        Camera2D mCamera;
        public Interaction mActiveInteraction;
        RigidBody mCameraFocus;
        Animator mAnimator;
        const float mWalkSpeed = 2.5f;
        public bool InCar = false;
        public bool Locked = false; // ignores controls
        float mInteractTimer = 0f;
        public Inventory mInventory;
        public Player(Vector2 pos, Overworld overworld, Camera2D camera, GraphicsDeviceManager graphics) 
            : base( pos, false, 1, new Vector2(20, 17) )
        {
            mCameraFocus = this;
            mCamera = camera;
            mAnimator = new Animator();
            mAnimator.AddAnimation((int)ePlayerAnimationState.Walking, new Animation(this, "data/textures/units/player_walk_cycle.json", graphics));
            mAnimator.AddAnimation((int)ePlayerAnimationState.Idle, new Animation(this, "data/textures/units/player_idle.json", graphics));
            mAnimator.AddAnimation((int)ePlayerAnimationState.OpenDoor, new Animation(this, "data/textures/units/player_grab.json", graphics, (int)ePlayerAnimationState.Idle));
            mInventory = new Inventory("Backpack",5,5);
            mInventory.ItemMap[2, 3] = new InventoryItem("Test Item", new List<Vector2Int> { Vector2Int.Zero });
            //mAnimator.AddAnimation((int)ePlayerAnimationState.Idle, new Animation(this, "data/textures/units/player_walk_cycle.json", graphics));
        }
        public Rectangle Rectangle()
        {
            return new Rectangle((mPosition - mSize/2f).ToPoint(), mSize.ToPoint());
        }
        public void EnterCar(Car car)
        {
            InCar = true;
            mCameraFocus = car;
            car.mStatic = false;
            Active = false;
            mDrag = 0f;
        }
        public void ExitCar(Car car)
        {
            InCar = false;
            mCameraFocus = this;
            car.mStatic = true;
            car.Stop();
            mPosition = car.ExitPos();
            Active = true;
        }
        public override void DoStep(GameTime gameTime)
        {
            mAnimator.Update(gameTime);
            mCamera.Position = -mCameraFocus.mPosition + new Vector2(960, 540);
            if (mActiveInteraction != null && Locked && mActiveInteraction.Duration > 0f)
            {
                mInteractTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (mInteractTimer > mActiveInteraction.Duration)
                { 
                    Locked = false;
                    mActiveInteraction.Use(); 
                }
            }
            base.DoStep(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!InCar)
            {
                mAnimator.Draw(spriteBatch);
                base.Draw(spriteBatch);
            }
        }
        public void Interact()
        {
            if (Locked)
                return;
            mAnimator.SetCurrentState((int)ePlayerAnimationState.OpenDoor);
            if (mActiveInteraction != null)
            {
                mInteractTimer = 0f;
                Locked = true;
            }
        }
        public void SetWalkDirection(Vector2 dir)
        {
            if (Locked)
                return;
            if (Vector2.Zero != dir)
            {
                dir.Normalize();
                mAnimator.SetCurrentState((int)ePlayerAnimationState.Walking);
                mVelocity = dir * mWalkSpeed;
                mRotation = -mVelocity.ClockwiseAngleBetween(Vector2.UnitY);
            }
            else
            {
                mAnimator.SetCurrentState((int)ePlayerAnimationState.Idle);
                mVelocity = Vector2.Zero;
            }
        }
        public override void OnCollisionStart(RigidBody other)
        { 
            if (other.IsTrigger)
            {
                mActiveInteraction = other.mInteraction;
            }
            base.OnCollisionStart(other);
        }
    }
}
