using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    struct AnimationFrame 
    { 
        Texture2D Sprite;
        float Time;
    }

    internal class Animation
    {
        float Timer;
        List<AnimationFrame> Frames = new();
        List<Tuple<int, float>> FrameInfo = new();
        int CurrentFrame = 0;
        Collidable Parent;
        Vector2 Origin;
        Texture2D CurrentTexture;
        public Animation(Collidable parent)
        {
            Parent = parent;
        }
        public void Update(GameTime gameTime) 
        { 
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer > FrameInfo[CurrentFrame].Item2) 
            {
                Timer = 0;
                CurrentFrame++;
                if (CurrentFrame > FrameInfo.Count)
                {
                    CurrentFrame = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentTexture, Parent.mPosition, null, Color.White, Parent.mRotation, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
