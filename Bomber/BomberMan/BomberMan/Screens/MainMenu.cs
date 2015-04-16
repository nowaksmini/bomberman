using BomberMan.Common;
using BomberMan.Common.Components;
using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Screens
{
    public class MainMenu : Screen
    {
        public const int OPTIONS = 5;
        public Button[] optionButtons = new Button[OPTIONS];
        private bool mousePressed, prevMousePressed = false;
        private KeyboardState KeyboardState, LastKeyboardState;
        private const int GAP = 10;
        private const int SHIFT = 100;
        //public Component Title { get; set; }

        public MainMenu()
        {
            //Title = new Component();
            //Title.Color = Color.White;
            for (int i = 0; i < OPTIONS; i++)
            {
                optionButtons[i] = new Button();
                optionButtons[i].Angle = 0;
                optionButtons[i].Color = Color.Transparent;
            }
        }

        public void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            double frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            MouseState mouse_state = Mouse.GetState();
            
            int mx = mouse_state.X;
            int my = mouse_state.Y;
            prevMousePressed = mousePressed;
            mousePressed = mouse_state.LeftButton == ButtonState.Pressed;
            int width = windowWidth/ 4;
            int height = (windowHeight - 2 * SHIFT - (OPTIONS)*GAP )/ OPTIONS;
            int x = (windowWidth)/2;
            int y = SHIFT + height/2;
            //Vector2 scale = new Vector2((float)width / (float)texture.Width, (float)height / (float)texture.Height);
           // Block block = new Block(texture, Color.Transparent, new Vector2(x, y), scale, 0, blockType);
            //Title.Rectangle = new Rectangle (windowWidth / (3), y, windowWidth / (3), (int)(2 * height));
            for (int i = 0; i < OPTIONS; i++ )
            {
                optionButtons[i].Position = new Vector2(x,y);
                optionButtons[i].Scale = new Vector2((float)width / (float)optionButtons[i].Texture.Width,
                    (float)height / (float)optionButtons[i].Texture.Height);
                optionButtons[i].Update(mouse_state.X, mouse_state.Y, frame_time, mousePressed, prevMousePressed);
                y += height + GAP;
            }
            HandleKeyboard();
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //Title.Draw(spriteBatch);
            for (int i = 0; i < OPTIONS; i++)
                optionButtons[i].Draw(spriteBatch);
            spriteBatch.End();
        }

        public void HandleKeyboard()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            Keys[] keymap = (Keys[])KeyboardState.GetPressedKeys();
            foreach (Keys k in keymap)
            {

                char key = k.ToString()[0];
                switch (key)
                {
                    case 'e':
                    case 'E':
                        optionButtons[0].OnClick(0.25);
                        break;
                    case 'm':
                    case 'M':
                        optionButtons[1].OnClick(0.25);
                        break;
                    case 'h':
                    case 'H':
                        optionButtons[2].OnClick(0.25);
                        break;
                    default:
                        break;
                }

            }
        }
    }

    public enum MainMenuOptions
    {
        NewGame,
        LoadGame,
        HighScores,
        Settings,
        LogOut
    }
}
