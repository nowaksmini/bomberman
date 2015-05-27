using System;
using System.Collections.Generic;
using System.Configuration;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens
{
    /// <summary>
    /// Ekran pojawiający się na starcie aplikacji.
    /// Weryfikuje poprawność danych logowania do aplikacji.
    /// Pozwala założyć konto i zalogować się do aplikacji.
    /// </summary>
    public class LoginScreen : Screen
    {
        public List<Label> Labels;
        public List<TextInput> Fields;
        private readonly SpriteFont _spriteFont;
        private const float LoginPanelWidth = 300;
        private const float HeightShift = 100;
        private const float DataInputShift = 20;
        private const int MaxNameCharacters = 15;
        private const int MaxPasswordCharacters = 15;
        private int _inputIndex;
        private float countDuration = 0.2f;
        private float _currentTime;
        private Button _saveButton;

        public LoginScreen(SpriteFont spriteFont, Texture2D texture)
        {
            var color = Color.White;
            _spriteFont = spriteFont;
            _saveButton = new Button(BState.Up, texture, color, new Vector2(0,0), new Vector2(1,1), 0, 2f);
            Func<Color> save = delegate()
            {
                GameManager.ScreenType = ScreenType.Game;
                return Color.Transparent;
            };
            _saveButton.Click = save;
            Labels = new List<Label>();
            Fields = new List<TextInput>();
            Labels.Add(new Label(_spriteFont, "User Name", color, new Vector2(0, 0), new Vector2(1, 1), 0));
            Labels.Add(new Label(_spriteFont, "Password", color, new Vector2(0, 0), new Vector2(1, 1), 0));
            Fields.Add(new TextInput(texture, spriteFont, true, color, TextInputType.Name, MaxNameCharacters));
            Fields.Add(new TextInput(texture, spriteFont, true, color, TextInputType.Password, MaxPasswordCharacters));
            Fields[_inputIndex].Enabled = true;
            foreach (var input in Fields)
            {
                var textInput = input;
                Func<Color> enable = delegate()
                {
                    Fields.ForEach(x => x.Enabled = false);
                    textInput.Enabled = true;
                    return Color.Transparent;
                };
                textInput.OnClick(enable);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var label in Labels)
            {
                label.Draw(spriteBatch);
            }
            foreach (var textInput in Fields)
            {
                textInput.Draw(spriteBatch);
            }
            spriteBatch.Begin();
            _saveButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Zaktualizuj 
        /// </summary>
        /// <param name="gameTime">czas trwania gry</param>
        /// <param name="windowWidth">szerokość okna aplikacji</param>
        /// <param name="windowHeight">wysokość okna aplikacji</param>
        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            MouseState mouseState = Mouse.GetState();
            double frameTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;

            float x = (float)windowWidth/2 - LoginPanelWidth/2;
            float y = (float)windowHeight/2 - HeightShift ;
            foreach (var label in Labels)
            {
                label.Position = new Vector2(x,y);
                y += _spriteFont.LineSpacing;
            }
            y = (float)windowHeight / 2 - HeightShift;
            for (int i = 0; i < Fields.Count; i++)
            {
                if (i == 0)
                {
                    Vector2 label = _spriteFont.MeasureString(Labels[i].Text);
                    Fields[i].Position = new Vector2(x + label.X + DataInputShift, y);
                }
                else
                {
                    Fields[i].Position = new Vector2(Fields[i-1].Position.X, y);
                }
                y += _spriteFont.LineSpacing;
                if (Fields[i].Enabled)
                {
                    Fields[i].ProcessKeyboard(false);
                }
                Fields[i].Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            }
            _saveButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_currentTime >= countDuration)
            {
                _currentTime -= countDuration;
                HandleKeyboard();
            }
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
                    case Keys.Tab:
                    case Keys.Down:
                        _inputIndex++;
                        _inputIndex = _inputIndex >= Fields.Count ? Fields.Count-1 : _inputIndex;
                        Fields.ForEach(x => x.Enabled = false);
                        Fields[_inputIndex].Enabled = true;
                        break;
                    case Keys.Up:
                        _inputIndex--;
                        _inputIndex = _inputIndex < 0 ? 0 : _inputIndex;
                        _inputIndex = _inputIndex%Fields.Count;
                        Fields.ForEach(x => x.Enabled = false);
                        Fields[_inputIndex].Enabled = true;
                        break;
                }
            }
        }

        public void CreateUser() { }
        public void LoginUser() { }
    }
}
