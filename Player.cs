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

namespace Mononoke
{
    internal class Player
    {
        string Name = "Pink";
        Car? mCar;
        Controller mController;
        Texture2D mSprite;
        Vector2 mSize;
        Vector2 mTextureOrigin;
        Vector2 mTextureSize;
        Body mBody;
        public Player(Car car, World world) 
        {
            mSprite = TextureAssetManager.GetSimpleSquare();
            mTextureSize = new Vector2(mSprite.Width, mSprite.Height);
            mTextureOrigin = mTextureSize / 2f;
            mSize = new Vector2(10f,10f);


            mBody = world.CreateBody(new Vector2(450f,450f), 0, BodyType.Dynamic);

            mBody.LinearDamping = 0.1f;
            mBody.AngularDamping = 1.0f;

            Fixture fixture = mBody.CreateRectangle(mSize.X, mSize.Y, 0.01f, Vector2.Zero);
            // Give it some bounce and friction
            fixture.Restitution = 0.3f;
            fixture.Friction = 20f;

            mCar = car;
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            // We make it possible to rotate the player body
            if (mCar  == null)
            {
                HandleWalkInput();
            }
            else
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

                mCar.Update(gameTime);
            }
            //mOldKeyState = state;

            //mController.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        { 
            mCar.Draw(spriteBatch);
            mCar.Draw(spriteBatch);
            spriteBatch.Draw(mSprite, mBody.Position, null, Color.White, mBody.Rotation, mTextureOrigin, mSize / mTextureSize, SpriteEffects.FlipVertically, 0f);

        }
        void HandleWalkInput()
        { 
        }
    }
}
