using System;
using System.Collections.Generic;
using System.Text;

namespace Mononoke.MapEvents
{
    public interface IExpandable
    {
        public bool TryExpand( MapHolder maps );
    }
}
