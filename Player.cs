using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
namespace Mononoke
{
    class Player
    {
 uint Authority = 0; // Souls
    float BaseSpeed = 40f;
    bool Sprinting;
    
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
    public Color Colour;
    float idleTime;
    List<Province> Provinces;
    private void Start()
    {
        MaxStamina = 300;
        CurrentStamina = 30;
    }

    public void Update( GameTime gameTime )
    {        
        idleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if ( idleTime > 1f )
        {
            CurrentStamina++;
        }
    }
    public float GetSpeed()
    {
        return BaseSpeed * ( Sprinting ? 4f : 1f);
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
