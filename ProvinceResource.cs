using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    class ProvinceResource
    {
        float CurrentSecond = 0;
        float SecondsToSpawn = 10f;

        //int Min = 0;
        int Max = 5;
        int Current = 0;
        //int Increment = 1;
        Vector2 Position;
        Province Province;
        eProvinceResourceType Type;
        public ProvinceResource( eProvinceResourceType type, Province p, Vector2 pos )
        {
            Type = type;
            Province = p;
            Position = pos;
        }
        public void Update( GameTime gameTime  )
        {
            CurrentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;    
            if ( CurrentSecond > SecondsToSpawn )
            {
                if ( Current < Max )
                    ++Current;
                CurrentSecond = 0;
            }
        }
        public void Draw( SpriteBatch spriteBatch )
        {
            spriteBatch.DrawString( Mononoke.Font, Current.ToString(), Position, Color.White );
        }
    }
}
