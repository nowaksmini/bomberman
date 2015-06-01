using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BomberManModel;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Button = BomberMan.Common.Components.StateComponents.Button;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Label = BomberMan.Common.Components.StateComponents.Label;
using Screen = BomberMan.Common.Screen;

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
        private readonly SpriteFont _spriteFont;
        private const float ImageWidth = 90;
        private static readonly Color LabelColor = Color.White;
        private const float RowGap = 30f;
        private const float ColumnGap = 10f;

        private const String Ghost1 = "Zły duch. Potrafi poruszać się po polach białych i szarych. ";
        private const String Ghost2 = "Goni gracza, po usłyszeniu wybuchu bomby w okolicy ";
        private const String Ghost3 = "biegnie w jej kierunku sądząc, że zasta tam gracza.";
        private const String Ghost4 = "Spotkanie Ducha z graczem oznacza koniec gry dla gracza.";
        private const String Ghost5 = "Duch po wejściu na pole oznaczone kolorem czerwonym ginie.";

        private const String Octopus1 = "Zła ośmiornica. Potrafi poruszać się jedynie po polach białych. ";
        private const String Octopus2 = "Porusza się w losowym kierunku chyba, że w lini prostej";
        private const String Octopus3 = "zauważy gracza wówczas goni go do miejsca, gdzie ";
        private const String Octopus4 = "widziała go ostatnim razem. Spotkanie Ośmiornicy";
        private const String Octopus5 = "z graczem oznacza koniec gry dla gracza. Ośmiornica";
        private const String Octopus6 = "po wejściu na pole oznaczone kolorem czerwonym ginie.";
       
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
            _labels = new List<Label>();
            _labels.Add(new Label(_spriteFont, Octopus1, LabelColor));
            _labels.Add(new Label(_spriteFont, Octopus2, LabelColor));
            _labels.Add(new Label(_spriteFont, Octopus3, LabelColor));
            _labels.Add(new Label(_spriteFont, Octopus4, LabelColor));
            _labels.Add(new Label(_spriteFont, Octopus5, LabelColor));
            _labels.Add(new Label(_spriteFont, Octopus6, LabelColor));
            _labels.Add(new Label(_spriteFont, Ghost1, LabelColor));
            _labels.Add(new Label(_spriteFont, Ghost2, LabelColor));
            _labels.Add(new Label(_spriteFont, Ghost3, LabelColor));
            _labels.Add(new Label(_spriteFont, Ghost4, LabelColor));
            _labels.Add(new Label(_spriteFont, Ghost5, LabelColor));

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
            foreach (var label in _labels)
            {
                label.Draw(spriteBatch);
            }
            Vector2 scale = new Vector2(ImageWidth / _images[(int)OpponentType.Octopus].Width,
                ImageWidth / _images[(int)OpponentType.Octopus].Height);
            Rectangle sourceRectangle = new Rectangle(0, 0, _images[(int)OpponentType.Octopus].Width,
                _images[(int)OpponentType.Octopus].Height);
            Vector2 origin = new Vector2((float)_images[(int)OpponentType.Octopus].Width / 2,
                (float)_images[(int)OpponentType.Octopus].Height / 2);
            spriteBatch.Draw(_images[(int)OpponentType.Octopus],
                new Vector2(_labels[5].Position.X + ImageWidth/2, _labels[5].Position.Y + ImageWidth), 
                sourceRectangle, Color.White,
                0.0f, origin, scale, SpriteEffects.None, 0f);
            scale = new Vector2(ImageWidth / _images[(int)OpponentType.Ghost].Width,
                ImageWidth / _images[(int)OpponentType.Ghost].Height);
            sourceRectangle = new Rectangle(0, 0, _images[(int)OpponentType.Ghost].Width,
                _images[(int)OpponentType.Ghost].Height);
            origin = new Vector2((float)_images[(int)OpponentType.Ghost].Width / 2,
                (float)_images[(int)OpponentType.Ghost].Height / 2);
            spriteBatch.Draw(_images[(int)OpponentType.Ghost],
                new Vector2(_labels[10].Position.X + ImageWidth / 2, _labels[10].Position.Y + ImageWidth),
                sourceRectangle, Color.White,
                0.0f, origin, scale, SpriteEffects.None, 0f);
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
            float x = RowGap * 2;
            float y = ColumnGap;
            int index = 0;
            foreach (var label in _labels)
            {
                if (index == 6) y += ImageWidth;
                label.Position = new Vector2(x,y);
                y += RowGap;
                index ++;
            }
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
