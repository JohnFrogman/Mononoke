using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Mononoke
{
    class DoodadTemplate
    {
        public bool Static;
        public List<Vector2> ColliderVertices;
        public Texture2D Sprite;
        public int Mass = 1;
        public bool Trigger;
        public static DoodadTemplate fromJson(JsonElement e)
        {
            DoodadTemplate result = new();
            result.Sprite = TextureAssetManager.GetDoodadByname(e.GetProperty("sprite_name").ToString());
            result.ColliderVertices = JsonSerializer.Deserialize<List<Vector2>>(e.GetProperty("vertices"));
            result.Static = e.GetProperty("static").GetBoolean();
            result.Mass = e.GetProperty("mass").GetInt32();
            result.Trigger = e.GetProperty("trigger").GetBoolean();
            return result;
        }
    }
    internal class Doodad
    {
        Texture2D mSprite;
        RigidBody mBody;
        public Doodad(Vector2 pos, DoodadTemplate template)
        {
            mSprite = template.Sprite;
            mBody = RigidBody.BuildPolygon(pos, template.Static, template.Mass, template.ColliderVertices, Vector2.Zero, template.Trigger );
        }        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSprite, mBody.mPosition, null, Color.White, mBody.mRotation, mBody.mCentre, 1f, SpriteEffects.None, 0f);
        }
    }
}
