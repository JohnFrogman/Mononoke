using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Mononoke
{
    public class Player : Actor
    {
        //uint Authority = 0; // Souls
        
        float BaseSpeed = 40f;
        //bool Sprinting;
    
        public int Food = 0;
        public int Materials = 0;
        public int Wealth = 0;

        private HUD hud;

        // Health
        int _Stability = 10;
        public int Stability { 
            set {
                if ( value <= MaxStability )
                    _Stability = value;
            }
            get {
                return _Stability;
            }
        }
        int MaxStability = 10;

        uint _CurrentStamina;
        public uint CurrentStamina 
        {
            get => _CurrentStamina;
            private set
            {
                if ( value < _CurrentStamina ) // If we spend stamina no longer idle
                    idleTime = 0f;
                _CurrentStamina = value;
                if ( _CurrentStamina > MaxStamina )
                    _CurrentStamina = MaxStamina;
                //hud.SetStamina( _CurrentStamina );
            }
        }
        uint _MaxStamina;
        public uint MaxStamina 
        { 
            get => _MaxStamina; 
            private set   
            {
                _MaxStamina = value;
                //hud.SetStaminaMax( _MaxStamina );
            }
        }
        float idleTime;
        public Player(Camera2D camera) : base( "player", Color.Magenta )
        {
            MaxStamina = 300;
            CurrentStamina = 30;
            hud = new HUD( this, camera );
        }

        public void Update( GameTime gameTime )
        {        
            idleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ( idleTime > 1f )
            {
                CurrentStamina++;
            }
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics )
        {
            hud.Draw( spriteBatch, graphics );
        }
        public float GetSpeed()
        {   
            return 0f;
            //return BaseSpeed * ( Sprinting ? 4f : 1f);
        }
        public void New()
        {
        }
        public void Load( string filepath)
        {
            //filepath =  Application.dataPath + "/save/1/player.json";
            //if ( !File.Exists( filepath ) )
            //{
            //    Debug.LogError("This file does not exist ! " + filepath );
            //}
            //Debug.Log( filepath );
            //JSONObject obj = new JSONObject( File.ReadAllText(filepath) );
            //Colour = obj["Colour"].ToColor32();
        }
        public void Save()
        {
        }
    }
}
