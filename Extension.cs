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
        public static TileFace GetNeighbourDirection( this Vector2 v1, Vector2 v2 )
        {
            Vector2 dir = v1 - v2;
            if ( dir.Length() > 1 )
                throw new System.Exception("These coordinates are not neighbours " + v1 + " " +  v2 + " dir : " + dir );
            if ( dir.Y == 1 )
                return TileFace.Up;
            if ( dir.Y == -1 )
                return TileFace.Down;
            if ( dir.X == 1 )
                return TileFace.Right;
            if ( dir.X == -1 )
                return TileFace.Left;
            throw new System.Exception("Could not establish direction " + v1 + " " + v2 + " dir : " + dir);
        }
        public static List<Vector2> GetNeighbours(this Vector2 v)
        {
            List<Vector2> result = new List<Vector2>();
            Vector2 up = v + new Vector2(0, -1);
            if (up.Y >= 0) // Up
            {
                result.Add(up);
            }
            Vector2 down = v + new Vector2(0, 1);
            if (down.Y + 1 < MapHolder.MAP_TILE_HEIGHT) // Down
            {
                result.Add(down);
            }
            Vector2 left = v + new Vector2(-1, 0); // left
            if (left.X >= 0) // left
            {
                result.Add(left);
            }
            Vector2 right = v + new Vector2(1, 0);
            if (right.X < MapHolder.MAP_TILE_WIDTH) // right
            {
                result.Add(right);
            }
            return result;
        }
        public static Vector2 GetNeighbour( this Vector2 v, TileFace dir )
        {
            if ( dir == TileFace.Up )
            {
                return v + new Vector2(0, -1);
            }
            else if ( dir == TileFace.Down )
            {
                return v + new Vector2(0, 1);
            }
            else if (dir == TileFace.Left)
            {
                return v + new Vector2(-1, 0);
            }
            else //(dir == eTileFace.Right)
            {
                return v + new Vector2(1, 0);
            }
        }
        public static Vector2 GetNeighbour(this Vector2 v, eTilePaintFace dir)
        {
            Vector2 up = new Vector2(0, -1);
            Vector2 right = new Vector2(1, 0);
            switch ( dir )
            {
                case eTilePaintFace.n :
                    return v + up;
                case eTilePaintFace.ne:
                    return v + up + right;
                case eTilePaintFace.e:
                    return v + right;
                case eTilePaintFace.se:
                    return v - up + right;
                case eTilePaintFace.s:
                    return v - up;
                case eTilePaintFace.sw:
                    return v - up - right;
                case eTilePaintFace.w:
                    return v - right;
                case eTilePaintFace.nw:
                    return v + up - right;
                default :
                    throw new System.Exception("Unrecognised direction, should not be possible to get here" );
            }
        }
    }
}
