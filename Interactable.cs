using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mononoke
{
    delegate void Interaction();
    internal class Interactable
    {
        Texture2D mColliderSprite;
        Vector2 mColliderTextureOrigin;
        Vector2 mColliderTextureSize;
        public Vector2 mSize;
        public Vector2 mPosition;
        List<Vector2> mVertices = new();
        Body mParent;
        bool mActive;
        Interaction mInteraction;
        Vector2 mOrigin;
        public Interactable(Vector2 size, Vector2 pos, Overworld overworld, Interaction interaction, Body parent = null) 
        {         
            mOrigin = pos;
            mPosition = pos;
            mSize = size;

            mColliderSprite = TextureAssetManager.GetSimpleSquare();
            mColliderTextureSize = new Vector2(mColliderSprite.Width, mColliderSprite.Height);
            mColliderTextureOrigin = mColliderTextureSize / 2f;

            List<Vector2> vertices = new List<Vector2> {
                     new Vector2(-mSize.X/2f, -mSize.Y/2f)
                    ,new Vector2(-mSize.X/2f,  mSize.Y/2f)
                    ,new Vector2( mSize.X/2f,  mSize.Y/2f)
                    ,new Vector2( mSize.X/2f, -mSize.Y/2f)
            };

            overworld.RegisterInteractable(this);
            //Activate += activate;
            mInteraction = interaction;
            mParent = parent;
        }
        public void Interact()
        {
            mInteraction();
        }
        public bool Update(GameTime gameTime, Vector2 v)
        {
            if (mParent != null)
            {
                mPosition = mParent.Position + mOrigin;
            }
            return SetActive(v);
        }
        public bool Update(GameTime gameTime, Rectangle rect)
        {
            if (mParent != null)
            {
                mPosition = mParent.Position + mOrigin;
            }
            return SetActive(rect);
        }
        public bool SetActive(Vector2 pos) 
        {
            Rectangle r = new Rectangle(mPosition.ToPoint(), mSize.ToPoint());
            if (r.Contains(pos))
            {
                mActive = true;
            }
            else
            {
                mActive = false;
            }
            return mActive;
        }
        public bool SetActive(Rectangle rect)
        {
            Rectangle r = new Rectangle(mPosition.ToPoint(), mSize.ToPoint());
            //if (mParent.Rotation != 0f)
            //{

            //    for (int i = 0; i < vertices.Count; ++i)
            //    {
            //        //vertices[i] = new Vector2(
            //        //    vertices[i].X * Math.Cos(mParent.Rotation)) - (vertices[i].Y * Math.Sin(mParent.Rotation)
            //        //    vertices[i].Y * Math.Cos(mParent.Rotation)) + (vertices[i].X * Math.Sin(mParent.Rotation)
            //        //);
            //        Vector2.Transform(vertices[i], Matrix.CreateRotationX(mParent.Rotation));
            //    }
            //}
            if ( r.Intersects(rect) )
            { 
                mActive = true;
            }
            else
            {
                mActive = false;
            }
            return mActive;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle r = new Rectangle(mPosition.ToPoint(), mSize.ToPoint());
            //spriteBatch.Draw(mColliderSprite, r, mActive ? Color.Green : Color.Red);
            spriteBatch.Draw(mColliderSprite, mPosition, null, mActive ? Color.Green: Color.Red, mParent == null ? 0f : mParent.Rotation, Vector2.Zero, mSize, SpriteEffects.None, 0f);
        }
    }
}
