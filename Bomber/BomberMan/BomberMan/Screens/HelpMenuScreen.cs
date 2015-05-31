using System;
using System.Collections.Generic;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberManModel;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens
{
    /// <summary>
    /// Klasa odpowiedzialna za generowanie opisów poruszania się po grze.
    /// </summary>
    public class HelpMenuScreen : Screen
    {
        private List<Texture2D> _images;
        private List<Label> _labels;
        private Button _backButton;
        private SpriteFont _spriteFont;
        private const float ImageWidth = 90;
        private static Color LabelColor = Color.White;

        /// <summary>
        /// Zainicjalizuj nową instancję klasy <see cref="HelpMenuScreen"/>
        /// </summary>
        /// <param name="images">obrazki do wyświetlenia</param>
        /// <param name="backButtonTexture">tło przycisku powrót</param>
        public HelpMenuScreen(List<Texture2D> images, Texture2D backButtonTexture, SpriteFont spriteFont)
        {
            _spriteFont = spriteFont;
            _images = images;
            CreateBackButton(backButtonTexture);
            LoadDescriptions();
        }

        /// <summary>
        /// Pobierz opis kompnentów z bazy danych.
        /// </summary>
        private void LoadDescriptions()
        {
            String message;
            OpponentDao opponent = OpponentService.FindBoardElementByType(OpponentType.Ghost, out message);
            //_labels.Add(new Label(_spriteFont, opponent.Description, ));
        }

        /// <summary>
        /// Utwórz przycisk powrotu do głównego menu.
        /// </summary>
        /// <param name="backButtonTexture">tło przycisku powrotu</param>
        private void CreateBackButton(Texture2D backButtonTexture)
        {
            _backButton = new Button(backButtonTexture, Color.White, null, "", Color.White)
            {
                Click = delegate()
                {
                    GameManager.ScreenType = ScreenType.Game;
                    return Color.Transparent;
                }
            };
            _backButton.Scale = new Vector2(GameManager.BackButtonSize / _backButton.Texture.Width,
                GameManager.BackButtonSize / _backButton.Texture.Height);
            _backButton.Position = new Vector2(GameManager.BackButtonSize / 2, GameManager.BackButtonSize / 2);
        }

        /// <summary>
        /// Narysuj wszystkie komponenty znajdujące się w silniku w aplikacji.
        /// </summary>
        /// <param name="spriteBatch">obiekt, w którym rysujemy komponenty</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _backButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Uaktualnij pozycje i rozmiary komponentów znajdujących się w silniku.
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
            _backButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            HandleKeyboard();
        }

        /// <summary>
        /// Obsłuż wciskane klawisze na klawiaturze.
        /// </summary>
        public override void HandleKeyboard()
        {
            KeyboardState = Keyboard.GetState();
            Keys[] keymap = KeyboardState.GetPressedKeys();
            foreach (Keys k in keymap)
            {
                switch (k)
                {
                    case Keys.Back:
                    case Keys.Escape:
                    case Keys.Home:
                        _backButton.Click();
                        break;
                }
            }
            LastKeyboardState = KeyboardState;
        }
    }
}
