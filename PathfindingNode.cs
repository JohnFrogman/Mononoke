using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
namespace Mononoke
{
    public class PathfindingNode
    {
        public float g;
        public float h;
        public float f;
        public PathfindingNode parent;
        public Vector2 loc;
        public PathfindingNode(Vector2 loc, float g, float h)
        {
            this.loc = loc;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }
        public List<Vector2> GetNeighbours()
        {
            return loc.GetNeighbours();
        }
    }
}
