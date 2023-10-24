using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using Microsoft.VisualBasic;

namespace Mononoke
{
    internal class Player : Collidable
    {
        Controller mController;
        Overworld mOverworld;
        Car mCar;
        Camera2D mCamera;
        public Player(World world, Vector2 pos, Overworld overworld, Camera2D camera) 
            : base( world, pos, BodyType.Kinematic, TextureAssetManager.GetPlayerSprite() )
        {
            mCamera = camera;
            mOverworld = overworld;
        }
        public Rectangle Rectangle()
        {
            return new Rectangle((mBody.Position - mSize/2f).ToPoint(), mSize.ToPoint());
        }
        public void EnterCar(Car car)
        {
            mCar = car;
            foreach (Fixture f in mBody.FixtureList)
                f.CollidesWith = Category.None;
        }
        public void Update(GameTime gameTime)
        {
            mCamera.Position = -mBody.Position + new Vector2(960, 540);
            KeyboardState state = Keyboard.GetState();
            // We make it possible to rotate the player body
            if ( mCar == null)
            {
                HandleWalkInput(state);
            }
            else
            {
                mBody.Position = mCar.Position();
                if (state.IsKeyDown(Keys.E))
                {
                    mCar = null;
                    foreach (Fixture f in mBody.FixtureList)
                        f.CollidesWith = Category.All;
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
        }
        public override void Draw(SpriteBatch spriteBatch)
        { 
            if ( mCar == null )
                spriteBatch.Draw(mSprite, mBody.Position, null, Color.White, mBody.Rotation, mTextureOrigin, mSize / mTextureSize, SpriteEffects.None, 0f);
            
            //base.Draw(spriteBatch);
        }
        void HandleWalkInput(KeyboardState state)
        {
            Vector2 resultantVelocity = Vector2.Zero;
            if (state.IsKeyDown(Keys.D))
            {
                resultantVelocity += new Vector2(1,0);
            }
            if (state.IsKeyDown(Keys.A))
            {
                resultantVelocity += new Vector2(-1, 0);
            }
            if (state.IsKeyDown(Keys.W))
            {
                resultantVelocity += new Vector2(0, -1);
            }
            if (state.IsKeyDown(Keys.S))
            {
                resultantVelocity += new Vector2(0, 1);
            }
            if (state.IsKeyDown(Keys.E))
            {
                mOverworld.TryInteract();
            }
            mBody.LinearVelocity = resultantVelocity * 100f;
        }
        void Interact()
        { 
        }
    }
}
