using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
namespace Mononoke
{
    enum eOverworldControlState
    {
        Default
        ,SelectedUnit
        ,DraggingMapDraggable
        ,DragSelect
    }
    class OverworldController
    {
        Player mPlayer;
        Car mCar;
        InputManager mInputManager;
        //bool SavePressed; 
        Overworld mOverworld;
        InventoryManager mInventoryManager;
        //Overworld overworld, Camera2D camera, GraphicsDeviceManager graphics, Mononoke game
        public OverworldController(InputManager inputManager, Overworld overworld, Player player, Car car, InventoryManager inventoryManager) 
        {
            mPlayer = player;
            mCar = car;
            mInputManager = inputManager;
            mOverworld = overworld;
            mInventoryManager = inventoryManager;
        }
        public void Update( GameTime gameTime)
        {
            // If in a car
            if (mPlayer.InCar)
            {
                CarInput();

            }
            // Else do player input
            else
            { 
                WalkInput();
            }
            InventoryInput();
        }
        void WalkInput()
        {
            Vector2 walkInput = Vector2.Zero;
            if (mInputManager.ButtonReleased(eInputType.Interact))
            {
                mPlayer.Interact();
                return;
            }
            if (mInputManager.ButtonDown(eInputType.Up))
            {
                walkInput -= Vector2.UnitY;
            }
            if (mInputManager.ButtonDown(eInputType.Down))
            {
                walkInput += Vector2.UnitY;
            }
            if (mInputManager.ButtonDown(eInputType.Right))
            {
                walkInput += Vector2.UnitX;
            }
            if (mInputManager.ButtonDown(eInputType.Left))
            {
                walkInput -= Vector2.UnitX;
            }
            mPlayer.SetWalkDirection(walkInput);
        } 
        void CarInput()
        {
            // Leave car
            if (mInputManager.ButtonReleased(eInputType.Interact))
            {
                mPlayer.ExitCar(mCar);
                //mOverworld.
            }
            // Accelerate
            if (mInputManager.ButtonDown(eInputType.Accelerate))
            {
                mCar.SetGas(1);
            }
            else
            {
                mCar.SetGas(0);
            }

            // Steer
            if (mInputManager.ButtonDown(eInputType.TurnLeft))
            {
                mCar.SetSteer(1);
            }
            else if (mInputManager.ButtonDown(eInputType.TurnRight))
            {
                mCar.SetSteer(-1);
            }
            else
            {
                mCar.SetSteer(0);
            }
            // Brake
            if (mInputManager.ButtonDown(eInputType.Brake))
            {
                mCar.SetBrake(1);
            }
            else
            {
                mCar.SetBrake(0);
            }

            if (mInputManager.ButtonReleased(eInputType.Radio))
            {
                mCar.ToggleRadio();
            }
        }

        //SwitchInventoryForward, SwitchInventoryBack, InventoryLeft, InventoryRight, InventoryUp, InventoryDown, OpenPlayerInventory
        void InventoryInput()
        {
            if (mInputManager.ButtonReleased(eInputType.OpenPlayerInventory))
            {
                mInventoryManager.ToggleInventory(mPlayer.mInventory, new Vector2Int(Mononoke.RENDER_WIDTH/2, Mononoke.RENDER_HEIGHT/2));
            }

            if (!mInventoryManager.Active) return;
            if (mInputManager.ButtonReleased(eInputType.SwitchInventoryForward))
            {
                mInventoryManager.SwitchInventory(true);
            }
            else if (mInputManager.ButtonReleased(eInputType.SwitchInventoryBack))
            {
                mInventoryManager.SwitchInventory(false);
            }

            Vector2Int inventoryMove = Vector2Int.Zero;
            if (mInputManager.ButtonReleased(eInputType.InventoryUp))
            {
                inventoryMove += Vector2Int.Up;
            }
            else if (mInputManager.ButtonReleased(eInputType.InventoryDown))
            {
                inventoryMove += Vector2Int.Down;
            }

            if (mInputManager.ButtonReleased(eInputType.InventoryLeft))
            {
                inventoryMove += Vector2Int.Left;
            }
            else if (mInputManager.ButtonReleased(eInputType.InventoryRight))
            {
                inventoryMove += Vector2Int.Right;
            }
            if (inventoryMove!= Vector2Int.Zero)
            { 
                mInventoryManager.InventoryMove(inventoryMove);
            }

            if (mInputManager.ButtonReleased(eInputType.InventorySelect))
            {
                mInventoryManager.OnInventorySelect();
            }
        }
    }
}
