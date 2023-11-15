using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal struct Vector2Int
    {
        public Vector2Int(int x, int y)   
        {
            X = x;
            Y = y;
        } 
        public int X;
        public int Y;
        public static Vector2Int Zero = new Vector2Int(0, 0);
        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }
    }
}
