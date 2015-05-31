using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Xsl;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens
{
    /// <summary>
    /// Klasa odpowiedzialna za generowanie widoku najlepszych wyników.
    /// </summary>
    public class HighScoresScreen : Screen
    {
        private const int TopScores = 10;
        private readonly List<Label> _scores;
        private readonly List<Label> _levels;
        private readonly List<Label> _userNames;
        private readonly List<Label> _lps;
        private readonly SpriteFont _labelSpriteFont;
        private readonly SpriteFont _titleSpriteFont;
        private static readonly Color LabelsTextColor = Color.White;
        private readonly Label _title;
        private readonly Label _user;
        private readonly Label _lp;
        private readonly Label _level;
        private readonly Label _points;
        private readonly Button _backButton;
        private const int TitleGap = 100;
        private const int RowGap = 40;

        /// <summary>
        /// Inicjalizuje nową instancję <see cref="HighScoresScreen"/> class.
        /// </summary>
        /// <param name="labelSpriteFont">czcionka labelek</param>
        /// <param name="titleSpriteFont">czcionka  tytułu</param>
        /// <param name="backButtonTexture">przycisk powrotu</param>
        public HighScoresScreen(SpriteFont labelSpriteFont, SpriteFont titleSpriteFont, Texture2D backButtonTexture)
        {
            _titleSpriteFont = titleSpriteFont;
            _title = new Label(titleSpriteFont, "Najlepsze wyniki", Color.White);
            _level = new Label(labelSpriteFont, "Poziom", Color.White);
            _user = new Label(labelSpriteFont, "Gracz", Color.White);
            _lp = new Label(labelSpriteFont, "Lp.", Color.White);
            _points = new Label(labelSpriteFont, "Punkty", Color.White);
            _labelSpriteFont = labelSpriteFont;
            _scores = new List<Label>();
            _levels = new List<Label>();
            _userNames = new List<Label>();
            _lps = new List<Label>();
            LoadHighScores();
            _backButton = new Button(backButtonTexture, LabelsTextColor, null, "", LabelsTextColor)
            {
                Click = delegate()
                {
                    GameManager.ScreenType = ScreenType.MainMenu;
                    return Color.Transparent;
                }
            };
            _backButton.Scale = new Vector2(GameManager.BackButtonSize/_backButton.Texture.Width,
                GameManager.BackButtonSize/_backButton.Texture.Height);
            _backButton.Position = new Vector2(GameManager.BackButtonSize, GameManager.BackButtonSize);
        }

        /// <summary>
        /// Załaduj najlepsze wyniki.
        /// </summary>
        public void LoadHighScores()
        {
            String message;
            List<GameDao> games = GameService.GetBestHighSocredGames(TopScores, out message);
            _userNames.Clear();
            _scores.Clear();
            _levels.Clear();
            _lps.Clear();
            for (int i = 0; i < games.Count; i++)
            {
                _lps.Add(new Label(_labelSpriteFont, (i + 1).ToString(), LabelsTextColor));
                _userNames.Add(new Label(_labelSpriteFont, games[i].User.Name, LabelsTextColor));
                _scores.Add(new Label(_labelSpriteFont, games[i].Points.ToString(), LabelsTextColor));
                _levels.Add(new Label(_labelSpriteFont, (games[i].Level + 1).ToString(), LabelsTextColor));
                ;
            }
        }

        /// <summary>
        /// Narusuj wszystkie komponenty znajdujące się w silniku w aplikacji.
        /// </summary>
        /// <param name="spriteBatch">obiekt, w którym rysujemy komponenty</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _title.Draw(spriteBatch);
            _lp.Draw(spriteBatch);
            _user.Draw(spriteBatch);
            _level.Draw(spriteBatch);
            _points.Draw(spriteBatch);
            foreach (var lp in _lps)
            {
                lp.Draw(spriteBatch);
            }
            foreach (var user in _userNames)
            {
                user.Draw(spriteBatch);
            }
            foreach (var points in _scores)
            {
                points.Draw(spriteBatch);
            }
            foreach (var level in _levels)
            {
                level.Draw(spriteBatch);
            }
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
            LoadHighScores();
            double frameTime = gameTime.ElapsedGameTime.Milliseconds/1000.0;
            MouseState mouseState = Mouse.GetState();
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;
            _title.Position = new Vector2((float) windowWidth/2 - _titleSpriteFont.MeasureString(_title.Text).X/2, 0);
            _lp.Position = new Vector2(RowGap, TitleGap);
            _user.Position = new Vector2(_lp.Position.X + _labelSpriteFont.MeasureString(_user.Text).X
                                         + RowGap, _lp.Position.Y);
            _level.Position = new Vector2((float)windowWidth/2 + 
                                          _labelSpriteFont.MeasureString(_level.Text).X/2, _user.Position.Y);
            _points.Position =
                new Vector2(
                    windowWidth - 
                    (TitleGap + (float) TitleGap/2 + RowGap +
                    _labelSpriteFont.MeasureString(_points.Text).X),
                    _level.Position.Y);
            float y = _points.Position.Y + TitleGap;
            foreach (var lp in _lps)
            {
                lp.Position = new Vector2(_lp.Position.X, y);
                y += RowGap;
            }
            y = _points.Position.Y + TitleGap;
            foreach (var user in _userNames)
            {
                user.Position = new Vector2(_user.Position.X, y);
                y += RowGap;
            }
            y = _points.Position.Y + TitleGap;
            foreach (var level in _levels)
            {
                level.Position = new Vector2(_level.Position.X, y);
                y += RowGap;
            }
            y = _points.Position.Y + TitleGap;
            foreach (var points in _scores)
            {
                points.Position = new Vector2(_points.Position.X, y);
                y += RowGap;
            }
            _backButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            HandleKeyboard();
        }

        /// <summary>
        /// Obsłuż wciskane klawisze na klawiaturze.
        /// </summary>
        public override void HandleKeyboard()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            Keys[] keymap = KeyboardState.GetPressedKeys();
            foreach (Keys k in keymap)
            {
                switch (k)
                {
                    case Keys.Back:
                    case Keys.Home:
                    case Keys.Escape:
                        _backButton.Click();
                        break;
                }
            }
        }
    }
}
