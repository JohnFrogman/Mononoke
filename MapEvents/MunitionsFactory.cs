using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mononoke
{
    class MunitionsFactory : MapEvent
    { 
        static int MunitionPerAlloy = 10;
        int Munitions = 0;
        static int munitionSpace = 100;
        int pendingProduction; // Amount of resources we have production for
        public MunitionsFactory( Vector2 pos, Actor owner ) : base ( pos, owner)
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
                pendingProduction += MunitionPerAlloy;
                CheckEmpty();
            }

        }

        protected override void OnExpire()
        {
            pendingProduction--;
            Munitions++;
            CheckEmpty();

        }
        void CheckEmpty()
        {
            Paused = pendingProduction > 0 && munitionSpace > Munitions;
        }
        protected override string GetChildJson()
        {
            string result = ",\"Type\" : \"MunitionsFactory\" ";
            result += ",\"Munitions\" : " + Munitions;
            result += ",\"pendingProduction\" : " + pendingProduction;
            return result;
        }
    }
}
