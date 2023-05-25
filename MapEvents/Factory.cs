using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    class Factory : MapEvent
    { 
        static int CashPerAlloy = 50; // One day turn this into a variable price based on the economy?
        int pendingProduction; // Amount of resources the factory has production for
        //int maxSpace;
        public Factory( Vector2 pos, Actor owner ) : base ( pos, owner )
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            throw new NotImplementedException();
        }

        public override void OnClick(Player clicker)
        {
            if ( clicker.Alloys == 0)
                return;
            else
            { 
                clicker.Alloys--;
                pendingProduction += CashPerAlloy;
                CheckEmpty();
            }

        }

        protected override void OnExpire()
        {
        }
        void CheckEmpty()
        {
            Paused = pendingProduction > 0;
        }
        protected override string GetChildJson()
        {
            string result = ",\"Type\" : \"Factory\" ";
            result += ",\"pendingProduction\" : " + pendingProduction;
            return result;
        }
    }
}
