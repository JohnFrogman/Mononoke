using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mononoke
{
    public class Camera2D
    {
        public Camera2D()
        {
            //Zoom = 1;
            Position = Vector2.Zero;
            Rotation = 0;
            Origin = Vector2.Zero;
            Position = Vector2.Zero;
        }

        //public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }

        public void Move(Vector2 direction)
        {
            Position += direction;
        }

        public Matrix GetTransform()
        {
            Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));
            Matrix rotationMatrix = Matrix.CreateRotationZ(Rotation);
            //Matrix scaleMatrix = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)); // No zoom for now
            Matrix originMatrix = Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));

            return translationMatrix * rotationMatrix /** scaleMatrix*/ * originMatrix;
        }
    }
}
