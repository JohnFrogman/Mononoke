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
            return (int)MathF.Floor( MathF.Abs( v1.X - v2.X ) + MathF.Abs( v1.Y - v2.Y ) );
        }
        public static eTileFace GetNeighbourDirection( this Vector2 v1, Vector2 v2 )
        {
            Vector2 dir = v1 - v2;
            if ( dir.Length() > 1 )
                throw new System.Exception("These coordinates are not neighbours " + v1 + " " +  v2 + " dir : " + dir );
            if ( dir.Y == 1 )
                return eTileFace.u;
            if ( dir.Y == -1 )
                return eTileFace.d;
            if ( dir.X == 1 )
                return eTileFace.r;
            if ( dir.X == -1 )
                return eTileFace.l;
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
        public static Vector2 GetNeighbour( this Vector2 v, eTileFace dir )
        {
            if ( dir == eTileFace.u )
            {
                return v + new Vector2(0, -1);
            }
            else if ( dir == eTileFace.d )
            {
                return v + new Vector2(0, 1);
            }
            else if (dir == eTileFace.l)
            {
                return v + new Vector2(-1, 0);
            }
            else //(dir == eTileFace.r)
            {
                return v + new Vector2(1, 0);
            }
        }
        public static List<Vector2> GetTilesInRange(this Vector2 v, int range)
        {
            // Breadth first search to range. Currently no blockers to range. but 
            List<Vector2> openNodes = v.GetNeighbours();
            List<Vector2> closedNodes = new List<Vector2>(){ v };
            List<Vector2> result = new List<Vector2>();
            while ( openNodes.Count > 0 )
            { 
                int d = openNodes[0].ManhattanDistance(v);
                if ( d <= range )
                { 
                    result.Add(openNodes[0]);
                    List<Vector2> neighbours = openNodes[0].GetNeighbours();
                    foreach ( Vector2 n in neighbours )
                    {
                        if ( !closedNodes.Contains( n ) && !openNodes.Contains( n ) )
                            openNodes.Add( n );
                    }
                }
                closedNodes.Add(openNodes[0]);
                openNodes.RemoveAt(0);
            }
            return result;
        }
        public static string ToJson( this Vector2 v )
        {
            return String.Format( "{{\"X\" : {0}, \"Y\" : {1} )}}", v.X, v.Y );
        }
    }
}
