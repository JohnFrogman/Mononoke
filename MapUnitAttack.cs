using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
namespace Mononoke
{
    class MapUnitAttack
    {
        float currentSecond = 0f;
        float timeToAttack = 1f;
        public int Damage = 5;
        public bool Update(GameTime gameTime)
        {
            //Debug.WriteLine("Updating attack");
            currentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentSecond > timeToAttack)
            {
                currentSecond = 0f;
                Debug.WriteLine("Ready to attack");
                return true;
            }
            return false;
        }
    }
}
