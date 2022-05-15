using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mononoke
{
    class HUD
    {
        uint UnitPerStamina = 1;
        uint UnitPerHp = 1;
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
        public void Draw( SpriteBatch spriteBatch )
        {
        }
    }
}
