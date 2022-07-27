using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.Json;


namespace Mononoke
{
    class NonPlayerActor : Actor
    {
        int ActionsPerMinute = 50;
        float secondsToAct;
        float second;
        List<MapEvent> ClickableEvents;
        List<MapUnit> Units;
        public NonPlayerActor( string name, Color col, eActorBehavior personality ) 
            : base( name,col )
        { 
        }
        public void Update( GameTime gameTime)
        {
            second += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ( second > secondsToAct )
            {
                second = 0f;
                DoAction();
            }
        }
        void DoAction()
        { 
        }
    }
}
