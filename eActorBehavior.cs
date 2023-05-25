using System;
using System.Collections.Generic;
using System.Text;

namespace Mononoke
{
    public enum eActorBehavior
    {
        Player  // No AI at all, does nothing. For players
        ,Raider // Wont take new territory, just defends, builds to attacks that destroy.
        ,SavageRaider // Attacks, but never does coordinated defense, will garrison and patrol but that's it.
        ,Creep  // Defends, never attacks.
        ,Expansionist
        ,Isolationist // Wont try to expand, but will try to reclaim territory it considers "core" aka what it started with.
    }
}
