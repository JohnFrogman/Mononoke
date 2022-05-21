using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace Mononoke
{
    class ResourceSupplyEvent : MapEvent, IDraggable
    {
        //int Min = 0;
        int Max = 5;
        int _Current = 0;
        int Current{ 
            set {
                if ( value > Max )
                    _Current = Max;
                else
                    _Current = value;
            }
            get
            {
                return _Current;
            }
        }
        //int Increment = 1;
        Vector2 Position;
        eProvinceResourceType Type;
        public ResourceSupplyEvent( eProvinceResourceType type, Vector2 pos )
        {
            Type = type;
            Position = pos;
        }
        public override void Draw( SpriteBatch spriteBatch )
        {
            spriteBatch.DrawString( Mononoke.Font, Current.ToString(), Position * MapHolder.PIXELS_PER_TILE, Color.Black );
        }   
        public override void OnClick( Player clicker )
        {
            int harvestAmount = 1;
            if ( Current < harvestAmount )
            {
                clicker.Food += Current;
                Current = 0;
            }
            else
            {
                Current -= harvestAmount;
                clicker.Food += harvestAmount;
            }
        }
        protected override void OnExpire()
        {
            Current++;
        }
        public override bool TryLink(MapEvent partner)
        {
            Debug.WriteLine("trying link of supply");
            return true;
        }
    }
}
