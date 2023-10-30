using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal static class CollisionManager
    {
        static List<Collidable> collidableList = new();
        public static bool Collidies(Collidable c)
        {
            bool ret = false;
            foreach ( Collidable collidable in collidableList)
            {
                if (!collidable.Active)
                    continue;
                if (collidable != c && collidable.Intersects(c))
                {
                    c.OnCollide(collidable);
                    collidable.OnCollide(c);
                    if (!collidable.IsTrigger)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }
        public static void CheckCollisions()
        {
            //for ( int i = 0; i < collidableList.Count; i++ )
            //{
            //    for (int j = i + 1; j < collidableList.Count; j++)
            //    {
            //        if (collidableList[i].Intersects(collidableList[j]))
            //        {
            //            collidableList[i].OnCollide(collidableList[j]);
            //            collidableList[j].OnCollide(collidableList[i]);
            //        }
            //    }
            //} 
        }
        public static void RegisterCollidable(Collidable collidable)
        {
            collidableList.Add(collidable); 
        }

        static void ProjectPolygon(Vector2 axis, List<Vector2> vertices,
                           ref float min, ref float max)
        {
            // To project a point on an axis use the dot product
            float dotProduct = Vector2.Dot(axis, vertices[0]);
            min = dotProduct;
            max = dotProduct;
            for (int i = 0; i < vertices.Count; i++)
            {
                dotProduct = Vector2.Dot(vertices[i], axis);
                if (dotProduct < min)
                {
                    min = dotProduct;
                }
                else
                {
                    if (dotProduct > max)
                    {
                        max = dotProduct;
                    }
                }
            }
        }
        static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }
    }
}
