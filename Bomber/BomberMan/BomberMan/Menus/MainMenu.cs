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
        public StateComponent Title { get; set; }

        public MainMenu()
        {
            Title = new StateComponent();
            Title.Color = Color.White;
            for (int i = 0; i < OPTIONS; i++)
            {
                optionButtons[i] = new Button();
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
            int width = windowWidth / (OPTIONS - 1);
            int height = windowHeight / (OPTIONS + (OPTIONS - 3)/2);
            int x = (windowWidth - width)/2;
            int y = height /2;
            Title.Rectangle = new Rectangle (windowWidth / (3), y, windowWidth / (3), (int)(2 * height));
            y += height;
            for (int i = 0; i < OPTIONS; i++ )
            {
                y += height / 2 + GAP;
                optionButtons[i].Rectangle = new Rectangle(x, y, width, height);
                optionButtons[i].Update(mouse_state.X, mouse_state.Y, frame_time, mousePressed, prevMousePressed);
            }
            HandleKeyboard();
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Title.Draw(spriteBatch);
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
