using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    class ActorHolder
    {
        List<Actor> Actors;
        Dictionary<Color, Actor> ActorsByColour;
        public Actor GetActor( Color col)
        {
            //return new Actor("Test",);
            return ActorsByColour[col];
        }
    }
}
