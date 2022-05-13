using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mononoke
{
    public class Camera2D
    {
        GraphicsDeviceManager Graphics;
        public Camera2D( GraphicsDeviceManager graphics )
        {
            //Zoom = 1;
            Position = Vector2.Zero;
            Rotation = 0;
            Origin = Vector2.Zero;
            Position = Vector2.Zero;
            Graphics = graphics;
        }

        //public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }

        public void Move(Vector2 direction)
        {
            Vector2 newPos = ( Position + direction );
            Position += direction;
            if ( Position.X >= 0 )
                Position = new Vector2( 0f, Position.Y );
            else if ( Position.X <= -MapHolder.MAP_PIXEL_WIDTH + Graphics.PreferredBackBufferWidth )
                Position = new Vector2( -MapHolder.MAP_PIXEL_WIDTH + Graphics.PreferredBackBufferWidth, Position.Y );
            
            if ( Position.Y >= 0 )   
                Position = new Vector2( Position.X, 0f );
            else if ( Position.Y <= -MapHolder.MAP_PIXEL_HEIGHT + Graphics.PreferredBackBufferHeight )   
                Position = new Vector2( Position.X, -MapHolder.MAP_PIXEL_HEIGHT + Graphics.PreferredBackBufferHeight );

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
