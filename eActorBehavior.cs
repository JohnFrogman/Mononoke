using System;
using System.Collections.Generic;
using System.Text;

namespace Mononoke
{
    public enum eActorBehavior
    {
        Player  // No AI at all, does nothing. For players
        ,Raider // Wont take new territory, just defends, builds to attacks that destroy.
    }
}
