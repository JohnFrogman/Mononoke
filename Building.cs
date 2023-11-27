using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    // Building has wall geometry
    class WallGeometry
    { 
        List<Vector2Int> Vertices;
        int Thickness;
        public WallGeometry() { }
        public List<RigidBody> toRigidBodies()
        {
            List<RigidBody> result = new();
            for (int i = 0; i < Vertices.Count; ++i)
            {
                //result.Add(RigidBody.BuildRectangle(Vertices[0], v2, true)
            }
            return result;

        }
    }
    internal class Building
    {
        List<RigidBody> mWalls = new();
        Texture2D mWallTexture;
        Texture2D mRoofTexture;

        public Building(Texture2D tex)
        { 
            
        }
        public void DrawWalls(SpriteBatch spriteBatch)
        {
           // spriteBatch.Draw()
        }
        public void DrawRoof(SpriteBatch spriteBatch)
        {

        }
    }
}
