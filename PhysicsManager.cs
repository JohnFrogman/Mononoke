using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal static class PhysicsManager
    {
        static List<RigidBody> ActiveBodies = new();
        //public static List<RigidBody> Collidies(RigidBody c)
        //{
        //    List<RigidBody> result = new();
        //    foreach ( RigidBody collidable in ActiveBodies)
        //    {
        //        if (!collidable.Active || collidable == c.mParent || collidable == c) // Cant collide with inactive, itself or it's parent
        //            continue;
        //        if (collidable.Intersects(c))
        //        {
        //            result.Add(collidable);
        //        }
        //    }
        //    return result;
        //}
        // THIS SHOULD ONLY HAPPEN ONCE PER FRAME!!!
        public static void DoStep(GameTime gameTime)
        {
            Dictionary<RigidBody, List<RigidBody>> collisions = new();
            foreach (RigidBody b in ActiveBodies)
            {
                b.DoStep(gameTime);
            }
            for (int i = 0; i < ActiveBodies.Count; i++)
            {
                RigidBody body1 = ActiveBodies[i];
                if (!body1.Active)
                    continue;
                for (int j = i + 1;  j < ActiveBodies.Count; j++)
                {
                    RigidBody body2 = ActiveBodies[j];
                    if (!body2.Active)
                        continue;
                    if (!body1.Intersects(body2))
                    {
                        continue;
                    }
                    if (!collisions.ContainsKey(body1))
                    {
                        collisions[body1] = new List<RigidBody>();
                    }
                    collisions[body1].Add(body2);

                    if (!collisions.ContainsKey(body2))
                    {
                        collisions[body2] = new List<RigidBody>();
                    }
                    collisions[body2].Add(body1);
                }
            }
            foreach (RigidBody b in ActiveBodies)
            {
                if (!collisions.ContainsKey(b))
                {
                    continue;
                }
                b.Collision(collisions[b]);
            }
        }
        public static void RegisterBody(RigidBody collidable)
        {
            ActiveBodies.Add(collidable); 
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
