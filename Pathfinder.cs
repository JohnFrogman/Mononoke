﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace Mononoke
{
    class Pathfinder
    {
        Texture2D previewTex;
        Texture2D straight;
        Texture2D bend;

        MapHolder Maps;
        MapUnitHolder Units;
        public Pathfinder( GraphicsDeviceManager graphics, MapHolder mapHolder, MapUnitHolder units)
        {
            Maps = mapHolder;
            Units = units;
            Color[] pixels = new Color[]{ Color.Magenta };
            previewTex = new Texture2D( graphics.GraphicsDevice, 1, 1);
            previewTex.SetData(pixels);
        }
        public List<Vector2> GetPath(Vector2 origin, Vector2 destination, bool unitsBlock = false )
        {
            List<PathfindingNode> openNodes = new List<PathfindingNode>();
            List<Vector2> closedLocations = new List<Vector2>();
            float g = 0;                                      // distance from start
            float h = origin.ManhattanDistance( destination); // approximate distance from destination
            float f = g + h;                                  // cost of node
            openNodes.Add(new PathfindingNode(origin, g, h));
            while (openNodes.Count > 0)
            {
                openNodes.Sort((p, q) => p.f.CompareTo(q.f));
                PathfindingNode currentNode = openNodes[0];
                closedLocations.Add(currentNode.loc);
                openNodes.RemoveAt(0);

                if (currentNode.loc == destination)
                {
                    List<Vector2> result = new List<Vector2>();
                    while (currentNode.parent != null)
                    {
                        result.Add(currentNode.loc);
                        currentNode = currentNode.parent;
                    }
                    return result;
                }
                List<Vector2> Neighbours = currentNode.GetNeighbours();
                foreach ( Vector2 n in Neighbours)
                {
                    if (Maps.Pathable( n, Units, unitsBlock ) && !closedLocations.Contains(n))
                    {
                        g = currentNode.g + Maps.GetMapCostAt( n );
                        h = n.ManhattanDistance(destination);
                        f = g + h;
                        PathfindingNode neighbourNode = openNodes.Find(x => x.loc == n);
                        if (neighbourNode == null)
                        {
                            neighbourNode = new PathfindingNode(n, g, h);
                            neighbourNode.parent = currentNode;
                            openNodes.Add(neighbourNode);
                        }
                        else if (neighbourNode.g > currentNode.g)
                        {
                            neighbourNode.parent = currentNode;
                            neighbourNode.g = g;
                            neighbourNode.f = neighbourNode.g + neighbourNode.h;
                        }
                    }
                    else
                    {
                    }
                }
            }
            return new List<Vector2>();
        }

        public void DrawPathPreview(List<Vector2> path, SpriteBatch spriteBatch)
        {
            //for (int i = 0; i < path.Count; i++)
            //{
            //    eTileFace previous = eTileFace.Up;
            //    eTileFace next = eTileFace.Up;
            //    Vector2 pos = path[i];
            //    //Vector2 previous = new Vector2();
            //    //Vector2 next = new Vector2();
            //    if ( i != 0 )
            //    {
            //        previous = pos.GetNeighbourDirection( path[i-1] ); 
            //    }
            //    if ( i != path.Count - 1 )
            //    {
            //        next = pos.GetNeighbourDirection(path[i + 1] );
            //    }

            //    if ( i == 0 )
            //    {
            //        float rotation = ( next == eTileFace.Up || next == eTileFace.Down ) ? 0f : 90f;
            //        spriteBatch.Draw( straight, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, rotation, new Vector2(0, 0), MapHolder.PIXELS_PER_TILE, SpriteEffects.None, 0f);
            //    }
            //    else if ( i == path.Count - 1 )
            //    {
            //        float rotation = ( previous == eTileFace.Up || previous == eTileFace.Down) ? 0f : 90f;
            //        spriteBatch.Draw(straight, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, rotation, new Vector2(0, 0), MapHolder.PIXELS_PER_TILE, SpriteEffects.None, 0f);
            //    }
            //    else
            //    {
    
            //        if ( previous == eTileFace.Right)
            //    }
            //}
            foreach (Vector2 pos in path)
            {
                spriteBatch.Draw(previewTex, pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), MapHolder.PIXELS_PER_TILE, SpriteEffects.None, 0f);
            }
        }
    }
}
