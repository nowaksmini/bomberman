using System.Collections.Generic;
using BomberMan.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens.Menus
{
    public class MainMenuScreen : Menu
    {
        private const int Shift = 100;

        public MainMenuScreen(int options, List<Texture2D> buttonsTextures) : base(options, buttonsTextures)
        {
        }

        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            double frameTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            MouseState mouseState = Mouse.GetState();
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;
            int width = windowWidth/ 4;
            int height = (windowHeight - 2 * Shift - (Options)*Gap )/ Options;
            int x = (windowWidth)/2;
            int y = Shift + height/2;
            for (int i = 0; i < Options; i++ )
            {
                OptionButtons[i].Position = new Vector2(x,y);
                OptionButtons[i].Scale = new Vector2((float)width / (float)OptionButtons[i].Texture.Width,
                    (float)height / (float)OptionButtons[i].Texture.Height);
                OptionButtons[i].Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
                y += height + Gap;
            }
            HandleKeyboard();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < Options; i++)
                OptionButtons[i].Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void HandleKeyboard()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            Keys[] keymap = KeyboardState.GetPressedKeys();
            foreach (Keys k in keymap)
            {
                char key = k.ToString()[0];
                switch (key)
                {
                    case 'e':
                    case 'E':
                        OptionButtons[0].OnClick(0.25);
                        break;
                    case 'm':
                    case 'M':
                        OptionButtons[1].OnClick(0.25);
                        break;
                    case 'h':
                    case 'H':
                        OptionButtons[2].OnClick(0.25);
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
