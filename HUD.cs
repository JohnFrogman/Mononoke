using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mononoke
{
    class HUD
    {
        //uint UnitPerStamina = 1;
        //uint UnitPerHp = 1;
        Player Player;
        Camera2D Camera;
        public HUD( Player player, Camera2D camera)
        {
            Player = player;
            Camera = camera;
        }

        public void SetStaminaMax( uint stamina)
        {
            //StaminaBar.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, UnitPerStamina * stamina );
            //StaminaBar.anchoredPosition = new Vector2( -900 + ( StaminaBar.rect.width / 2 ), StaminaBar.anchoredPosition.y );
            //StaminaBarBackground.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, UnitPerStamina * stamina );
            //StaminaBarBackground.anchoredPosition = new Vector2( -900 + ( StaminaBar.rect.width / 2 ), StaminaBar.anchoredPosition.y );
        }
        public void SetStamina( uint stamina )
        {
            //StaminaImage.fillAmount = ( UnitPerStamina * stamina ) / ( StaminaBar.rect.width );
        }
        public void SetHealthMax( uint stamina)
        {
            //HealthBar.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, UnitPerStamina * stamina );
            //HealthBar.anchoredPosition = new Vector2( -900 + ( StaminaBar.rect.width / 2 ), StaminaBar.anchoredPosition.y );
        }
        public void SetHealth( uint stamina )
        {
            //HealthImage.fillAmount = ( UnitPerStamina * stamina ) / ( StaminaBar.rect.width );
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics )
        {
            spriteBatch.DrawString( Mononoke.Font, Player.Food.ToString(), -Camera.Position + new Vector2( 10,10), Color.White );
            spriteBatch.DrawString(Mononoke.Font, Player.Stability.ToString(), -Camera.Position + new Vector2(40, 10), Color.White);
        }
    }
}
