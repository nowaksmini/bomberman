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
        private const float DataInputShift = 30;
        private const float DataRowsShift = 10;
        private const int MaxNameCharacters = 15;
        private const int MaxPasswordCharacters = 15;
        private int _inputIndex;
        private float countDuration = 0.2f;
        private float _currentTime;
        private Button _saveButton;
        private const float SaveWidth = 100f;
        private const float SaveHeight = 40f;
        private const float SaveButtonShift = 50f;
        private Texture2D _bombTexture;
        private const float BombWidth = 200f;
        private const float BombHeight = 150f;
        private const String BomberManTitle = "BomberMan";
        private SpriteFont _spriteFontTitle;

        public LoginScreen(SpriteFont spriteFont, SpriteFont spriteFontTitle, Texture2D texture, Texture2D bombTexture)
        {
            _spriteFontTitle = spriteFontTitle;
            _bombTexture = bombTexture;
            var color = Color.White;
            var color_input = Color.Black;
            _spriteFont = spriteFont;
            _saveButton = new Button(BState.Up, texture, color_input, new Vector2(0,0), 
                new Vector2(1,1), 0, 2f, spriteFont, "LOG IN");
            Func<Color> save = delegate()
            {
                GameManager.ScreenType = ScreenType.MainMenu;
                return Color.LightBlue;
            };
            _saveButton.Click = save;
            Labels = new List<Label>();
            Fields = new List<TextInput>();
            Labels.Add(new Label(_spriteFont, "User Name", color, new Vector2(0, 0), new Vector2(1, 1), 0));
            Labels.Add(new Label(_spriteFont, "Password", color, new Vector2(0, 0), new Vector2(1, 1), 0));
            Fields.Add(new TextInput(texture, spriteFont, true, color_input, TextInputType.Name, MaxNameCharacters));
            Fields.Add(new TextInput(texture, spriteFont, true, color_input, TextInputType.Password, MaxPasswordCharacters));
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
            Vector2 scale = new Vector2(BombWidth/_bombTexture.Width, BombHeight/_bombTexture.Height);
            Rectangle sourceRectangle = new Rectangle(0, 0, _bombTexture.Width, _bombTexture.Height);
            Vector2 origin = new Vector2((float)_bombTexture.Width / 2, (float)_bombTexture.Height / 2);
            spriteBatch.Draw(_bombTexture, new Vector2(BombWidth/2,BombHeight/2), sourceRectangle, Color.White,
             0.0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_spriteFontTitle, BomberManTitle, new Vector2(Labels[0].Position.X,BombHeight/2), Color.White);
            _saveButton.Draw(spriteBatch);
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
                y += _spriteFont.LineSpacing + DataRowsShift;
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
                y += _spriteFont.LineSpacing + DataRowsShift;
                if (Fields[i].Enabled)
                {
                    Fields[i].ProcessKeyboard(false);
                }
                Fields[i].Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            }
            Vector2 savePosition = Fields[Fields.Count - 1].Position;
            savePosition.X += SaveWidth/2;
            savePosition.Y += SaveButtonShift;
            savePosition.Y += SaveHeight/2;
            _saveButton.Position = savePosition;
            _saveButton.Scale = new Vector2(
                SaveWidth / _saveButton.Texture.Width , SaveHeight / _saveButton.Texture.Height);
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
                    case Keys.Enter:
                        _saveButton.Click();
                        break;
                }
            }
        }

        public void CreateUser() { }
        public void LoginUser() { }
    }
}
