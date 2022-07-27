using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    class Foundry : MapEvent // Convertes Pterichor/Lint into Alloys
    {
        static int singleProductionRatio = 4; // Just using one ingredient will produce at a ratio of 1 : 4 
        static int dualProductionRatio = 16; // Using 1 of each produces at a ratio of 2 : 16, twice as efficient
        int producing = 0;
        int stored = 0;
        public Foundry( Vector2 pos ) : base ( pos )
        { 
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw( spriteBatch );
            spriteBatch.DrawString(Mononoke.Font, producing.ToString(), Origin * MapHolder.PIXELS_PER_TILE, Color.Black, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0f );
            spriteBatch.DrawString(Mononoke.Font, stored.ToString()   , Origin * MapHolder.PIXELS_PER_TILE + new Vector2(28, 0), Color.Black, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(Mononoke.Font, stored.ToString(), Origin * MapHolder.PIXELS_PER_TILE, Color.Black);
        }

        public override void OnClick(Player clicker)
        {
            if ( clicker.Petrichor > 0 && clicker.Lint > 0 )
            {
                clicker.Petrichor--;
                clicker.Lint--;
                producing += dualProductionRatio;
            }
            else if ( clicker.Petrichor > 0 )
            {
                clicker.Petrichor--;
                producing += singleProductionRatio;
            }
            else if ( clicker.Lint > 0)
            {
                clicker.Lint--;
                producing += singleProductionRatio;
            }
            SetProducing();
        }
        void SetProducing()
        {
            if ( producing < 1 )
            { 
                CurrentSecond = 0f;
                Paused = true;
            }
            else
            {
                Paused = false;
            }
        }
        protected override void OnExpire()
        {
            if ( producing > 0 )
            { 
                producing--;
                stored++;
            }
            SetProducing();
        }
    }
}
