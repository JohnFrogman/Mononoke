using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mononoke
{
    class GameEventQueue
    {
        Player Player;
        List<GameEvent> ActiveEvents; // Events ticking down
        List<GameEvent> PendingEvents; // Events waiting to be made active
        List<GameEvent> EventPool; // Events that can be placed either straight into active or into pending if full.

        int MaxPendingEvents = 5;
        int MaxActiveEvents = 5;

        float CurrentSecond = 0f;
        float SecondsToNextEvent = 10f;
        public GameEventQueue(Player player )
        {
            ActiveEvents = new List<GameEvent>();
            PendingEvents = new List<GameEvent>();
            EventPool = new List<GameEvent>();
            Player = player;
        }
        public void AddEvent( GameEvent ev )
        {
            EventPool.Add( ev );
        }
        public void OnEventExpire( GameEvent ev)
        {
            CycleOutEvent(ev);
            Player.Stability -= ev.DamageOnFail;
        }

        public void OnEventCompleted( GameEvent ev) 
        {
            CycleOutEvent(ev);
        }
        void CycleOutEvent( GameEvent ev)
        {
            ActiveEvents.Remove(ev );
            if ( PendingEvents.Count > 0 )
            {
                ActiveEvents.Add(PendingEvents[0] );
                PendingEvents.RemoveAt( 0 );
            }
            EventPool.Add(ev );
        }
        void AddNextEvent()
        {
            if (EventPool.Count > 0)
            {
                GameEvent ev = EventPool[0];
                if ( !TryAddToActiveEvents( ev ) )
                {
                    if ( !TryAddToPendingEvents( ev ) )
                    {
                        Player.Stability -= ev.DamageOnFail;
                    }
                }
            }
        }
        bool TryAddToPendingEvents( GameEvent ev )
        {
            if ( PendingEvents.Count >= MaxPendingEvents )
                return false;
            else 
            {
                PendingEvents.Add( ev );
                EventPool.Remove(ev);
                return true;
            }
        }
        bool TryAddToActiveEvents(GameEvent ev)
        {
            if (ActiveEvents.Count >= MaxActiveEvents)
                return false;
            else
            {
                ActiveEvents.Add(ev);
                EventPool.Remove(ev);
                return true;
            }
        }
        public void Update( GameTime gameTime )
        {
            for (int i = ActiveEvents.Count - 1; i >= 0; i--)
            {
                ActiveEvents[i].Update( gameTime );
            }
            CurrentSecond += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ( CurrentSecond > SecondsToNextEvent )
            {
                AddNextEvent();
                CurrentSecond = 0f;
            }
        }
        public void Draw(SpriteBatch spriteBatch )
        {
            for (int i = 0; i < ActiveEvents.Count; i++)
            {
                ActiveEvents[i].Draw( spriteBatch, i);
            }
        }
        public bool TryClickAt( Vector2 pos )
        {
            for (int i = 0; i < ActiveEvents.Count; i++)
            {
                
                //if ( ActiveEvents.TryClickAt(pos, i) )
                //    return true;
            }
            return false;
        }
    }
}
