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
        public static Vector2Int Right = new Vector2Int(1, 0);
        public static Vector2Int Left = new Vector2Int(-1, 0);
        public static Vector2Int Up = new Vector2Int(0, 1);
        public static Vector2Int Down = new Vector2Int(0, -1);
        //public static Vector2Int Zero = new Vector2Int(0, 0);
        //public static Vector2Int Zero = new Vector2Int(0, 0);
        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }
        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return !(a==b);
        }
        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.X == b.X && a.Y == b.Y;
        }        
    }
}
