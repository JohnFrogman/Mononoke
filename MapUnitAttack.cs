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
        public bool Ready;
        public int Damage = 5;
        public int Range = 2;
        public void Update(GameTime gameTime)
        {
            //Debug.WriteLine("Updating attack");
            if ( !Ready )
            { 
                currentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentSecond > timeToAttack)
                {
                    Ready = true;
                    currentSecond = 0f;
                }
            }
        }
        public float Progress()
        {
            if ( Ready )
                return 1f;
            else
                return currentSecond / timeToAttack;
        }
    }
}
