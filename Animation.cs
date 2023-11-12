using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mononoke
{
    public class AnimationFrame 
    { 
        public Texture2D Sprite;
        public float Duration;
        public int x; public int y; public int width; public int height;
    }

    internal class Animation
    {
        float Timer;
        List<AnimationFrame> Frames = new();
        AnimationFrame CurrentFrame;
        RigidBody Parent;
        Vector2 Origin;
        Texture2D mSpriteSheet;
        public bool Done { get; private set; }
        public int NextAnim { get; private set; }
        public bool OneShot()
        {
            return NextAnim > -1;
        }
        public Animation(RigidBody parent, string path, GraphicsDeviceManager graphics, int nextAnim = -1)
        {
            NextAnim = nextAnim;
            Done = false;
            Parent = parent;
            Origin = parent.Centre();
            if (!File.Exists(path))
            {
                throw new Exception("This animation manifest file does not exist " + path);
            }
            JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
            /*
            "meta": {
              "app": "https://www.aseprite.org/",
              "version": "1.2.30",
              "image": "player_sheet.png",
              "format": "RGBA8888",
              "size": { "w": 80, "h": 17 },
              "scale": "1",
              "frameTags": [
              ],
             */
            string sheetPath = new FileInfo(path).Directory.FullName + "/" + doc.RootElement.GetProperty("meta").GetProperty("image").ToString();
            if (!File.Exists(sheetPath))
            {
                throw new Exception("This sprite sheet file does not exist " + sheetPath);
            }
            mSpriteSheet = Texture2D.FromFile(graphics.GraphicsDevice, sheetPath);
            /*
            { 
                "frames": 
                {
                    "0": 
                    {
                        "frame": { "x": 0, "y": 0, "w": 20, "h": 17 },
                        "rotated": false,
                        "trimmed": false,
                        "spriteSourceSize": { "x": 0, "y": 0, "w": 20, "h": 17 },
                        "sourceSize": { "w": 20, "h": 17 },
                        "duration": 150
                    },
            ...
            */
            JsonElement e = doc.RootElement.GetProperty("frames");
            int i = 0;
            //JsonElement.ObjectEnumerator itr = e.EnumerateObject();
            JsonElement frame;
            while (e.TryGetProperty(i.ToString(), out frame))
            {
                ++i;
                AnimationFrame f = new AnimationFrame();// = new AnimationFrame{ 
                f.Duration  = frame.GetProperty("duration").GetSingle();
                f.x         = frame.GetProperty("frame").GetProperty("x").GetInt32();
                f.y         = frame.GetProperty("frame").GetProperty("y").GetInt32();
                f.width     = frame.GetProperty("frame").GetProperty("w").GetInt32();
                f.height    = frame.GetProperty("frame").GetProperty("h").GetInt32();
                Frames.Add(f);
            };
            CurrentFrame = Frames[0];
        }
        public void Reset()
        {
            Done = false;
            Timer = 0f;
        }
        public void Update(GameTime gameTime) 
        { 
            if (Frames.Count == 0)
                return;
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Timer > CurrentFrame.Duration) 
            {
                Timer = 0;
                int index = Frames.IndexOf(CurrentFrame) + 1;
                if (index >= Frames.Count)
                {
                    index = 0;
                    Done = true;
                }
                CurrentFrame = Frames[index];
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSpriteSheet, Parent.mPosition, new Rectangle(CurrentFrame.x, CurrentFrame.y, CurrentFrame.width, CurrentFrame.height), Color.White, Parent.mRotation, Origin, 1f, SpriteEffects.None, 0f);
        }
        public float GetTotalLength()
        {
            float total = 0f;
            foreach (AnimationFrame frame in Frames)
            {
                total += frame.Duration;
            }
            return total;
        }
    }
}
