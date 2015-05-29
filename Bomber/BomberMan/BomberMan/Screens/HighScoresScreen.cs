using System;
using System.Collections.Generic;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens
{
    public class HighScoresScreen : Screen
    {
        private const int TopScores = 10;
        private readonly List<Label> _scores;
        private readonly List<Label> _levels;
        private readonly List<Label> _userNames;
        private readonly SpriteFont _labelSpriteFont;
        private readonly SpriteFont _titleSpriteFont;
        private static readonly Color LabelsTextColor = Color.White;
        private Label _title;
        private readonly Button _backButton;
        private const int TitleGap = 50;
        private const int RowGap = 30;


        public HighScoresScreen(SpriteFont labelSpriteFont, SpriteFont titleSpriteFont, Texture2D backButtonTexture)
        {
            _titleSpriteFont = titleSpriteFont;
            _title = new Label(titleSpriteFont, "Najlepsze wyniki", Color.White);
            _labelSpriteFont = labelSpriteFont;
            _scores = new List<Label>();
            _levels = new List<Label>();
            _userNames = new List<Label>();
            LoadHighScores();
            _backButton = new Button(backButtonTexture, LabelsTextColor, null, "", LabelsTextColor)
            {
                Click = delegate()
                {
                    GameManager.ScreenType = ScreenType.MainMenu;
                    return Color.Transparent;
                }
            };
            _backButton.Scale = new Vector2(GameManager.BackButtonSize / _backButton.Texture.Width,
                GameManager.BackButtonSize / _backButton.Texture.Height);
            _backButton.Position = new Vector2(GameManager.BackButtonSize, GameManager.BackButtonSize);
        }

        private void LoadHighScores()
        {
            String message;
            List<GameDao> games = GameService.GetBestHighSocredGames(TopScores, out message );
            for (int i = 0; i < games.Count; i++)
            {
                _userNames.Add(new Label(_labelSpriteFont, games[i].User.Name, LabelsTextColor));
                _scores.Add(new Label(_labelSpriteFont, games[i].Points.ToString(), LabelsTextColor));
                _levels.Add(new Label(_labelSpriteFont, games[i].Level.ToString(), LabelsTextColor)); ;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _title.Draw(spriteBatch);
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

        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            double frameTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            MouseState mouseState = Mouse.GetState();
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;
            _title.Position = new Vector2((float)windowWidth/2 - _titleSpriteFont.MeasureString(_title.Text).X/2, 0);
            int x = 0;
            int y = TitleGap;
            foreach (var user in _userNames)
            {
                user.Position = new Vector2(x,y);
                y += RowGap;
            }
            x += windowWidth/3;
            y = TitleGap;
            foreach (var level in _levels)
            {
                level.Position = new Vector2(x, y);
                y += RowGap;
            }
            x *= 2;
            y = TitleGap;
            foreach (var points in _scores)
            {
                points.Position = new Vector2(x, y);
                y += RowGap;
            }
            _backButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            HandleKeyboard();
        }

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
                    case Keys.B:
                    case Keys.Escape:
                        _backButton.Click();
                        break;
                }
            }
        }
    }
}
