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
    public class Settings : Screen
    {
        public const int NUMBER_OF_BUTTONS = 5;

        public Button[] buttons = new Button[NUMBER_OF_BUTTONS];
        bool mpressed, prevMpressed = false;
        KeyboardState KeyboardState, LastKeyboardState;

        public Settings(int windowWidth, int windowHeight)
        {
            int x = windowWidth / 2 - windowWidth / (2 * NUMBER_OF_BUTTONS);
            int y = windowHeight / 2 -
                NUMBER_OF_BUTTONS / 2 * windowHeight / NUMBER_OF_BUTTONS -
                (NUMBER_OF_BUTTONS % 2) * windowHeight / (2 * NUMBER_OF_BUTTONS);
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                buttons[i] = new Button();
                buttons[i].State = BState.UP;
                buttons[i].Color = Color.White;
                buttons[i].Timer = 0.0;
                //buttons[i].Rectangle = new Rectangle(x, y, windowWidth / NUMBER_OF_BUTTONS, windowHeight / NUMBER_OF_BUTTONS);
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
            int x = windowWidth / 2 - windowWidth / (2 * NUMBER_OF_BUTTONS);
            int y = windowHeight / 2 -
                NUMBER_OF_BUTTONS / 2 * windowHeight / NUMBER_OF_BUTTONS -
                (NUMBER_OF_BUTTONS % 2) * windowHeight / (2 * NUMBER_OF_BUTTONS);
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++ )
            {
                //buttons[i].Rectangle = new Rectangle(x, y, windowWidth / NUMBER_OF_BUTTONS, windowHeight / NUMBER_OF_BUTTONS);
                y += windowHeight / NUMBER_OF_BUTTONS;
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
