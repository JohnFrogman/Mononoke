using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;

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
            return (int)MathF.Floor( MathF.Abs( v1.X - v2.X ) + MathF.Abs( v1.Y - v2.Y ) );
        }
        public static string ToJson( this Vector2 v )
        {
            return String.Format( "{{\"X\" : {0}, \"Y\" : {1} )}}", v.X, v.Y );
        }
        public static float Magnitude(this Vector2 v)
        {
            return (float)Math.Sqrt( v.X * v.X + v.Y * v.Y);
        }
        public static Vector2 RotateRadians(this Vector2 v, float angle)
        {
            Matrix m = Matrix.CreateRotationZ(angle);
            return Vector2.Transform(v, m);
        }
        public static Vector2 RotateDegrees(this Vector2 v, float angle)
        {
            Matrix m = Matrix.CreateRotationZ((float)(Math.PI / 180f) * angle);
            return Vector2.Transform(v, m);
        }
        public static Vector2 PerpendicularClockwise(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }
        public static Vector2 PerpendicularAnticlockwise(this Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }
        public static float AngleBetween(this Vector2 a, Vector2 b)
        {
            // a · b = | a || b | cosθ
            return (float)Math.Acos(Vector2.Dot(a,b) / (a.Magnitude() * b.Magnitude()));
        }
        public static float ClockwiseAngleBetween(this Vector2 a, Vector2 b)
        {
            float dot = Vector2.Dot(a,b); //Dot product between [x1, y1] and [x2, y2]
            float det = a.X * b.Y - a.Y * b.X;      // Determinant
            return (float)Math.Atan2(det, dot);  //# atan2(y, x) or atan2(sin, cos)
        }
    }
}
