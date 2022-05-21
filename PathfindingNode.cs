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
            List<Vector2> result = new List<Vector2>();
            Vector2 up = loc + new Vector2( 0, -1 );
            if ( up.Y >= 0 ) // Up
            {
                result.Add(up);
            }
            Vector2 down = loc + new Vector2(0, 1);
            if ( down.Y + 1 < MapHolder.MAP_TILE_HEIGHT) // Down
            {
                result.Add(down);
            }
            Vector2 left = loc + new Vector2(-1, 0); // left
            if (left.X >= 0) // left
            {
                result.Add(left);
            }
            Vector2 right = loc + new Vector2(1, 0);
            if (right.X < MapHolder.MAP_TILE_WIDTH) // right
            {
                result.Add(right);
            }
            return result;
        }
    }
}
