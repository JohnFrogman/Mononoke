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
    internal class Player
    {
        Camera2D mCamera;
        public Interaction mActiveInteraction;
        RigidBody mCameraFocus;
        public RigidBody mBody;
        Animator mAnimator;
        const float mWalkSpeed = 2.5f;
        public bool InCar = false;
        public bool Locked = false; // ignores controls
        float mInteractTimer = 0f;
        public Inventory mInventory;
        public Player(Vector2 pos, Overworld overworld, Camera2D camera, GraphicsDeviceManager graphics) 
        {
            List<Vector2> vertices = new List<Vector2>()
            {
                new Vector2(10, 1)
                ,new Vector2(10, -3)
                ,new Vector2(8, -5)
                ,new Vector2(4, -6)
                ,new Vector2(-4, -6)
                ,new Vector2(-8, -5)
                ,new Vector2(-10, -3)
                ,new Vector2(-10, 1)
                ,new Vector2(-3, 7)
                ,new Vector2(3, 7)
            };
            mBody = RigidBody.BuildPolygon(pos, false, 700, vertices, new Vector2(20, 17) / 2);
            //mBody = RigidBody.BuildRectangle(pos, false, 1, new Vector2(20,17));
            mBody.ActivatesTriggers = true;
            mCameraFocus = mBody;
            mCamera = camera;
            mAnimator = new Animator();
            mAnimator.AddAnimation((int)ePlayerAnimationState.Walking, new Animation(mBody, "data/textures/units/player_walk_cycle.json", graphics));
            mAnimator.AddAnimation((int)ePlayerAnimationState.Idle, new Animation(mBody, "data/textures/units/player_idle.json", graphics));
            mAnimator.AddAnimation((int)ePlayerAnimationState.OpenDoor, new Animation(mBody, "data/textures/units/player_grab.json", graphics, (int)ePlayerAnimationState.Idle));
            mInventory = new Inventory("Backpack",5,5);
            mInventory.ItemMap[2, 3] = new InventoryItem("Test Item", new List<Vector2Int> { Vector2Int.Zero });
            //mAnimator.AddAnimation((int)ePlayerAnimationState.Idle, new Animation(this, "data/textures/units/player_walk_cycle.json", graphics));
        }
        public Rectangle Rectangle()
        {
            return new Rectangle((mBody.mPosition - mBody.mSize /2f).ToPoint(), mBody.mSize.ToPoint());
        }
        public void EnterCar(Car car)
        {
            InCar = true;
            mCameraFocus = car.mBody;
            car.mBody.mStatic = false;
            mBody.Active = false;
            mBody.mDrag = 0f;
        }
        public void ExitCar(Car car)
        {
            InCar = false;
            mCameraFocus = mBody;
            car.mBody.mStatic = true;
            car.mBody.Stop();
            mBody.mPosition = car.ExitPos();
            mBody.Active = true;
        }
        public void Update(GameTime gameTime)
        {
            mAnimator.Update(gameTime);
            mCamera.Position = -mCameraFocus.mPosition + new Vector2(480, 270);
            if (mActiveInteraction != null && Locked && mActiveInteraction.Duration > 0f)
            {
                mInteractTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (mInteractTimer > mActiveInteraction.Duration)
                { 
                    Locked = false;
                    mActiveInteraction.Use(); 
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!InCar)
            {
                mAnimator.Draw(spriteBatch);
                mBody.Draw(spriteBatch);
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
                mBody.mVelocity = dir * mWalkSpeed;
                mBody.mRotation = -mBody.mVelocity.ClockwiseAngleBetween(Vector2.UnitY);
            }
            else
            {
                mAnimator.SetCurrentState((int)ePlayerAnimationState.Idle);
                mBody.mVelocity = Vector2.Zero;
            }
        }
        public void OnCollisionStart(RigidBody other)
        {
            if (other.IsTrigger)
            {
                mActiveInteraction = other.mInteraction;
            }
        }
    }
}
