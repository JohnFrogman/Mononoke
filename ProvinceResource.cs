using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    class ProvinceResource
    {
        const float SecondsToExpire = 10f;
        public float CurrentSecond = 0f;
        public int Amount;
        public ProvinceResource()
        {
            Amount = 1;
        }
        public void Update( GameTime gameTime )
        {
            CurrentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
