using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace Mononoke
{
    internal class CollidableDoodad
    {
        RigidBody mBody;
        Texture2D mSprite;
        public CollidableDoodad() 
        { 
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, mBody.mPosition, null, Color.White, mBody.mRotation, mBody.mCentre, 1f, SpriteEffects.None, 0f);
        }
        public static CollidableDoodad fromJSON(JsonElement json)
        {
            return new CollidableDoodad();
            //int i = 0;
            //JsonElement vertex;
            //JsonElement e = json.get("vertices");
            //List<Vector2Int> vertices = new List<Vector2Int>();

            //while (e, out vertex))
            //{
            //    ++i;
            //    Vector2Int v = new Vector2Int(.GetProperty("frame").GetProperty("X").GetInt32(), .GetProperty("frame").GetProperty("Y").GetInt32());
            //    AnimationFrame f = new AnimationFrame();// = new AnimationFrame{ 
            //    f.Duration = frame.GetProperty("duration").GetSingle();
            //    f.x = frame.GetProperty("frame").GetProperty("x").GetInt32();
            //    f.y = frame.GetProperty("frame").GetProperty("y").GetInt32();
            //    f.width = frame.GetProperty("frame").GetProperty("w").GetInt32();
            //    f.height = frame.GetProperty("frame").GetProperty("h").GetInt32();
            //    Frames.Add(f);
            //};
        }
    }
}
