using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Mononoke
{
    public static class Extension
    {
        //public static List<Vector2Int> GetNeighbours( this Vector2Int coord)
        //{   
        //    List<Vector2Int> result = new List<Vector2Int>();
        //    result.Add ( coord + Vector2Int.left );
        //    result.Add ( coord + Vector2Int.up );
        //    result.Add ( coord + Vector2Int.down );
        //    result.Add ( coord + Vector2Int.right );
        //    return result;
        //}
        public static Color GetColor( this JsonElement e)
        {   
            string str = e.ToString();
            //Color col = new Color();
            byte r = byte.Parse( str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber );
            byte g = byte.Parse( str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber );
            byte b = byte.Parse( str.Substring(4, 2), System.Globalization.NumberStyles.HexNumber );
            //col.a = 255;
            return new Color( r, g, b );
        }
        public static void Shuffle<T>(this Stack<T> stack)
        {
            System.Random rnd = new System.Random();
            var values = stack.ToArray();
            stack.Clear();
            foreach (var value in values.OrderBy(x => rnd.Next() ) )
                stack.Push(value);
        }
        public static int ManhattanDistance( this Vector2 v1, Vector2 v2 )
        {
            return (int)MathF.Floor(v1.X -v2.X + v1.Y - v2.Y);
        }
    }
}
