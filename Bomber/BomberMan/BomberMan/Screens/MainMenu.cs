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
        public const int NUMBER_OF_BUTTONS = 3,
            EASY_BUTTON_INDEX = 0,
            MEDIUM_BUTTON_INDEX = 1,
            HARD_BUTTON_INDEX = 2;

        public Button[] buttons = new Button[NUMBER_OF_BUTTONS];
        bool mpressed, prevMpressed = false;
        KeyboardState KeyboardState, LastKeyboardState;

        public MainMenu(int windowWidth, int windowHeight)
        {
            int x = windowWidth / 2 - windowWidth / 8;
            int y = windowHeight / 2 -
                NUMBER_OF_BUTTONS / 2 * windowHeight / 4 -
                (NUMBER_OF_BUTTONS % 2) * windowHeight / 8;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                buttons[i] = new Button();
                buttons[i].State = BState.UP;
                buttons[i].Color = Color.White;
                buttons[i].Timer = 0.0;
                buttons[i].Rectangle = new Rectangle(x, y, windowWidth / 4, windowHeight / 4);
                y += windowHeight / 4;
            }
        }

        public void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            double frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            MouseState mouse_state = Mouse.GetState();
            int mx = mouse_state.X;
            int my = mouse_state.Y;
            prevMpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;
            int x = windowWidth / 2 - windowWidth / 8;
            int y = windowHeight / 2 -
                NUMBER_OF_BUTTONS / 2 * windowHeight /4 -
                (NUMBER_OF_BUTTONS % 2) * windowHeight / 8;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++ )
            {
                buttons[i].Rectangle = new Rectangle(x, y, windowWidth / 4, windowHeight / 4);
                y += windowHeight / 4;
                buttons[i].Update(mouse_state.X, mouse_state.Y, frame_time, mpressed, prevMpressed);
            }
            HandleKeyboard();
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                buttons[i].Draw(spriteBatch);
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
                        buttons[0].OnClick(0.25);
                        break;
                    case 'm':
                    case 'M':
                        buttons[1].OnClick(0.25);
                        break;
                    case 'h':
                    case 'H':
                        buttons[2].OnClick(0.25);
                        break;
                    default:
                        break;
                }

            }
        }

    }
}
