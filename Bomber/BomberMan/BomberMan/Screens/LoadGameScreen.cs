using System;
using System.Collections.Generic;
using System.Linq;
using BomberMan.Common.Components.StateComponents;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Label = BomberMan.Common.Components.StateComponents.Label;
using Screen = BomberMan.Common.Screen;

namespace BomberMan.Screens
{
    /// <summary>
    /// Klasa odpowiedzialna za tworzenie listy ostatnich gier, pozwalająca użytkownikowi łądować grę
    /// o ile nie jest ona skończona.
    /// </summary>
    public class LoadGameScreen : Screen
    {
        private const int NumberOfLastGames = 10;
        private const int RowGap = 10;
        private const int ColumnGap = 30;
        private const String Lp = "Lp.";
        private const String Time = "Czas zapisu";
        private const String Level = "Poziom";
        private const String Points = "Punkty";
        private const String Load = "Rozpocznij";
        private static readonly Color LabelColor = Color.White;

        private List<GameDao> _games;

        private readonly Texture2D _loadBackground;
        private readonly SpriteFont _spriteFont;
        private Label _lpLabel;
        private Label _saveTimeLabel;
        private Label _levelLabel;
        private Label _pointsLabel;
        private Label _loadLabel;
        private Button _backButton;

        private List<Label> _lpsLabels;
        private List<Label> _timesLabels;
        private List<Label> _levelsLabels;
        private List<Label> _pointsLabels;
        private List<Button> _optionsButtons;

        /// <summary>
        /// Zainicjalizuj nową instancję klasy <see cref="LoadGameScreen"/>.
        /// </summary>
        /// <param name="loadBackground">tło przycisku ładującego grę.</param>
        /// <param name="spriteFont">czcionka labelek</param>
        /// <param name="backButtonTexture">tło przycisku powrotu</param>
        public LoadGameScreen(Texture2D loadBackground, SpriteFont spriteFont, Texture2D backButtonTexture)
        {
            CreateBackButton(backButtonTexture);
            _spriteFont = spriteFont;
            _loadBackground = loadBackground;
            GenerateLabels();
            LoadGames();
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
                    GameManager.ScreenType = ScreenType.MainMenu;
                    return Color.Transparent;
                }
            };
            _backButton.Scale = new Vector2(GameManager.BackButtonSize/_backButton.Texture.Width,
                GameManager.BackButtonSize/_backButton.Texture.Height);
            _backButton.Position = new Vector2(GameManager.BackButtonSize/2, GameManager.BackButtonSize/2);
        }

        /// <summary>
        /// Utwóz opisy kolumn w wudoku ładowania gry.
        /// </summary>
        private void GenerateLabels()
        {
            _lpLabel = new Label(_spriteFont, Lp, LabelColor);
            _saveTimeLabel = new Label(_spriteFont, Time, LabelColor);
            _levelLabel = new Label(_spriteFont, Level, LabelColor);
            _pointsLabel = new Label(_spriteFont, Points, LabelColor);
            _loadLabel = new Label(_spriteFont, Load, LabelColor);
        }

        /// <summary>
        /// Załąduj n ostatnich, nieskończonych gier gracza.
        /// </summary>
        public void LoadGames()
        {
            String message;
            _games = GameService.GetLastGamesForUser(Utils.User, NumberOfLastGames, out message);
            _optionsButtons = new List<Button>();
            _lpsLabels = new List<Label>();
            _timesLabels = new List<Label>();
            _levelsLabels = new List<Label>();
            _pointsLabels = new List<Label>();
            int i = 1;
            foreach (var game in _games)
            {
                _lpsLabels.Add(new Label(_spriteFont, i.ToString(), LabelColor));
                _timesLabels.Add(new Label(_spriteFont, game.SaveTime.ToLongDateString() + " "
                    + game.SaveTime.ToLongTimeString(), LabelColor));
                _levelsLabels.Add(new Label(_spriteFont, (game.Level + 1).ToString(), LabelColor));
                _pointsLabels.Add(new Label(_spriteFont, game.Points.ToString(), LabelColor));
                var i1 = i;
                Button option = new Button(_loadBackground, LabelColor, null, "", Color.Black)
                {
                    Click = delegate
                    {
                        Utils.Game = _games[i1 - 1];
                        GameManager.ScreenType = ScreenType.Game;
                        return Color.Transparent;
                    }
                };
                _optionsButtons.Add(option);
                i++;
            }
        }

        /// <summary>
        /// Narysuj wszystkie komponenty znajdujące się w silniku w aplikacji.
        /// </summary>
        /// <param name="spriteBatch">obiekt, w którym rysujemy komponenty</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _backButton.Draw(spriteBatch);
            _lpLabel.Draw(spriteBatch);
            _saveTimeLabel.Draw(spriteBatch);
            _levelLabel.Draw(spriteBatch);
            _pointsLabel.Draw(spriteBatch);
            _loadLabel.Draw(spriteBatch);
            for (int i = 0; i < _lpsLabels.Count; i++)
            {
                _lpsLabels[i].Draw(spriteBatch);
                _timesLabels[i].Draw(spriteBatch);
                _levelsLabels[i].Draw(spriteBatch);
                _pointsLabels[i].Draw(spriteBatch);
                _optionsButtons[i].Draw(spriteBatch);
            }
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
            double frameTime = gameTime.ElapsedGameTime.Milliseconds/1000.0;
            MouseState mouseState = Mouse.GetState();
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;
            _backButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);

            float y = GameManager.BackButtonSize + ColumnGap;
            float x = RowGap;
            _lpLabel.Position = new Vector2(x, y);
            x += _spriteFont.MeasureString(_lpLabel.Text).X + ColumnGap;
            _saveTimeLabel.Position = new Vector2(x, y);
            x = (float) windowWidth/2 - _spriteFont.MeasureString(_levelLabel.Text).X/2 + ColumnGap;
            _levelLabel.Position = new Vector2(x, y);
            x = windowWidth - _spriteFont.MeasureString(_loadLabel.Text).X - ColumnGap;
            _loadLabel.Position = new Vector2(x, y);
            x = (float) windowWidth/2 + _spriteFont.MeasureString(_pointsLabel.Text).X + ColumnGap;
            _pointsLabel.Position = new Vector2(x, y);

            float rowHeight = _spriteFont.MeasureString(_lpLabel.Text).Y;
            x = _lpLabel.Position.X;
            y = _lpLabel.Position.Y + rowHeight + RowGap;
            foreach (var lp in _lpsLabels)
            {
                lp.Position = new Vector2(x, y);
                y += +rowHeight + RowGap;
            }
            x = _saveTimeLabel.Position.X;
            y = _lpLabel.Position.Y + rowHeight + RowGap;
            foreach (var time in _timesLabels)
            {
                time.Position = new Vector2(x, y);
                y += +rowHeight + RowGap;
            }
            x = _levelLabel.Position.X;
            y = _lpLabel.Position.Y + rowHeight + RowGap;
            foreach (var level in _levelsLabels)
            {
                level.Position = new Vector2(x, y);
                y += +rowHeight + RowGap;
            }
            x = _pointsLabel.Position.X;
            y = _lpLabel.Position.Y + rowHeight + RowGap;
            foreach (var points in _pointsLabels)
            {
                points.Position = new Vector2(x, y);
                y += +rowHeight + RowGap;
            }
            x = _loadLabel.Position.X;
            y = _lpLabel.Position.Y + rowHeight + RowGap;
            foreach (var button in _optionsButtons)
            {
                button.Scale = new Vector2(_spriteFont.MeasureString(_loadLabel.Text).X/_loadBackground.Width,
                    _spriteFont.MeasureString(_loadLabel.Text).Y/_loadBackground.Height);
                button.Position = new Vector2(x + _spriteFont.MeasureString(_loadLabel.Text).X/2,
                    y + _spriteFont.MeasureString(_loadLabel.Text).Y/2);
                button.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
                y += +rowHeight + RowGap;
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
