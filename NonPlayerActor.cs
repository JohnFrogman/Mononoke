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
        float secondsToAct = 0f;
        float second = 0f;
        //List<MapEvent> ClickableEvents;
        NonPlayerMapAgent mAgent;
        public NonPlayerActor( string name, Color col, eActorBehavior personality ) 
            : base( name,col )
        { 
            secondsToAct = ActionsPerMinute / 60f;
            switch (personality)
            {
                case eActorBehavior.Raider:
                case eActorBehavior.SavageRaider:
                case eActorBehavior.Creep:
                case eActorBehavior.Expansionist:
                case eActorBehavior.Isolationist:
                    mAgent = new NonPlayerMapAgent();
                    break;
                case eActorBehavior.Player:
                default:
                    throw new Exception("Invalid personality!");
            }
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
        // AI wants to prioritise staying alive, meaning feeding it's cities, different personalities will prioritise defence in different ways.
        void DoAction()
        { 
            mAgent.DoAction();
            //foreach ( )
        }
    }
}