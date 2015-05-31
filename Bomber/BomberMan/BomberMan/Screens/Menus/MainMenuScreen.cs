using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens.Menus
{
    /// <summary>
    /// Klasa odpowiedzialna za tworzenie okna menu oraz wybieranie opcji dostępnego menu.
    /// </summary>
    public class MainMenuScreen : Menu
    {
        private const int Shift = 100;
        private float countDuration = 0.1f; //every  0.1s.
        private float _currentTime;

        /// <summary>
        /// Zainicjalizuj instancje <see cref="MainMenuScreen"/>.
        /// </summary>
        /// <param name="options">Ilość dostępnych opcji</param>
        /// <param name="buttonsTextures">Tła przycisków</param>
        public MainMenuScreen(int options, List<Texture2D> buttonsTextures) : base(options, buttonsTextures)
        {
            Func<Color> newGame = delegate
            {
                GameManager.ScreenType = ScreenType.Game;
                return Color.White;
            };
            OptionButtons[(int) MainMenuOptions.NewGame].Click = newGame;
            OptionButtons[(int) MainMenuOptions.LogOut].Click = delegate
            {
                GameManager.ScreenType = ScreenType.Login;
                Utils.User = null;
                return Color.Transparent;
            };
            OptionButtons[(int) MainMenuOptions.Settings].Click = delegate
            {
                GameManager.ScreenType = ScreenType.Settings;
                return Color.Transparent;
            };
            OptionButtons[(int)MainMenuOptions.HighScores].Click = delegate
            {
                GameManager.ScreenType = ScreenType.HighScores;
                return Color.Transparent;
            };
            OptionButtons[(int)MainMenuOptions.LoadGame].Click = delegate
            {
                GameManager.ScreenType = ScreenType.LoadGame;
                return Color.Transparent;
            };

        }

        /// <summary>
        /// Uaktualnij pozycje i rozmiary komponentów znajdujących się w menu.
        /// </summary>
        /// <param name="gameTime">czas trwania gry</param>
        /// <param name="windowWidth">szerokość okna aplikacji</param>
        /// <param name="windowHeight">wysokość okna aplikacji</param>
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
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_currentTime >= countDuration)
            {
                _currentTime -= countDuration;
                HandleKeyboard();
            }
        }

        /// <summary>
        /// Narysuj wszystkie komponenty znajdujące się w menu.
        /// </summary>
        /// <param name="spriteBatch">obiekt, w którym rysujemy komponenty</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < Options; i++)
                OptionButtons[i].Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Obsłuż wciskane klawisze na klawiaturze.
        /// </summary>
        public override void HandleKeyboard()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }
    }

    /// <summary>
    /// Opcje dostępne w menu gry.
    /// </summary>
    public enum MainMenuOptions
    {
        NewGame,
        LoadGame,
        HighScores,
        Settings,
        LogOut
    }
}
