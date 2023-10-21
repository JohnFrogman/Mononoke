using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    internal class Player
    {
        string Name = "Pink";
        Car? mCar;
        Controller mController;
        public Player(Car car) 
        { 
            mCar = car;
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            // We make it possible to rotate the player body
            if (mCar  == null)
            {

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
        }

    }
}
