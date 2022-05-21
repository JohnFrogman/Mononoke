using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mononoke
{
    class GameEvent
    {
        float SecondsToExpire = 10f;
        float CurrentSecond = 0f;
        public int DamageOnFail = 1;
        
        public string Name;
        int ResourcesToFulfil = 1;

        GameEventQueue eventQueue;
        public GameEvent( string name, int resources, GameEventQueue queue )
        {
            eventQueue = queue;
            ResourcesToFulfil = resources;
            Name = name;
        }
        public void Update( GameTime gameTime )
        {   
            CurrentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ( CurrentSecond > SecondsToExpire )
                Expire();
                
        }
        public void Draw(SpriteBatch spriteBatch, int index )
        {
            spriteBatch.DrawString( Mononoke.Font, Name + " " + CurrentSecond.ToString("n0"), new Vector2( 10, 10 * index), Color.White );
        }
        public void Expire()
        {
            eventQueue.OnEventExpire( this );
        }
        public void TryClickAt( Vector2 pos, int index )
        {
            //if ()
        }
    }
}
