using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    internal class Animator
    {
        Dictionary<int, Animation> AnimationStates= new();
        int CurrentState;
        public Animator() 
        {
            CurrentState = 0;
        }
        public void AddAnimation(int index, Animation animation)
        {
            AnimationStates.Add(index, animation);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            AnimationStates[CurrentState].Draw(spriteBatch);
        }
        public void Update(GameTime gameTime) 
        {
            AnimationStates[CurrentState].Update(gameTime);
            if (AnimationStates[CurrentState].NextAnim > -1 && AnimationStates[CurrentState].Done)
                SetCurrentState(AnimationStates[CurrentState].NextAnim);
        }
        public void SetCurrentState(int index)
        {
            if (AnimationStates.ContainsKey(index))
            {
                if (AnimationStates[index].OneShot())
                    AnimationStates[index].Reset();
                CurrentState = index;
            }
        }
        public int GetCurrentState()
        {
            return CurrentState;
        }
        public float GetAnimationLength(int index)
        {
            return AnimationStates[index].GetTotalLength();
        }
    }
}
