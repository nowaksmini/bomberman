using System;
using System.Collections.Generic;
using System.Configuration;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
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
        private const float CountDuration = 0.2f;
        private float _currentTime;
        private readonly Button _saveButton;
        private const float SaveWidth = 100f;
        private const float SaveHeight = 40f;
        private const float SaveButtonShift = 50f;
        private readonly Texture2D _bombTexture;
        private const float BombWidth = 200f;
        private const float BombHeight = 150f;
        private const String BomberManTitle = "BomberMan";
        private const String CheckBoxCheckedSymbol = "x";
        private const String LogInButton = "Zaloguj";
        private const String UserName = "Login";
        private const String Password = "Hasło";
        private const String Register = "Zarejestruj";
        private const String ShowPassword = "Pokaż hasło";
        private readonly SpriteFont _spriteFontTitle;
        private readonly SpriteFont _spriteFontCheckBox;
        private readonly Button _showPassword;
        private readonly Button _regiter;

        /// <summary>
        /// Utwórz nowy ekran logowania.
        /// </summary>
        /// <param name="spriteFont">czcionka dla labelek</param>
        /// <param name="spriteFontTitle">czcionka dla nagłówka</param>
        /// <param name="checkBoxFont">czcionka dla dodatkowych opcji jak rejestruj czy pokaż hasło</param>
        /// <param name="texture">tło przycisków i edytowalnych pól</param>
        /// <param name="bombTexture">obrazek bomby</param>
        public LoginScreen(SpriteFont spriteFont, SpriteFont spriteFontTitle, SpriteFont checkBoxFont,
            Texture2D texture, Texture2D bombTexture)
        {
            _spriteFontCheckBox = checkBoxFont;
            _spriteFontTitle = spriteFontTitle;
            _bombTexture = bombTexture;
            _spriteFont = spriteFont;
            var colorInput = Color.Black;
            var colorButtons = Color.White;
            CreateLabelsAndFields(colorButtons, colorInput, texture);
            _regiter = new Button(texture, colorButtons, spriteFont, "", colorInput);
            _saveButton = new Button(texture, colorButtons, spriteFont, LogInButton, colorInput);
            _showPassword = new Button(texture, colorButtons, spriteFont, "", colorInput);
            _showPassword.Click = delegate()
            {
                _showPassword.Text = _showPassword.Text.Length == 0 ? CheckBoxCheckedSymbol : "";
                Fields[1].TextInputType = Fields[1].TextInputType == TextInputType.Name
                    ? TextInputType.Password
                    : TextInputType.Name;
                Labels[Labels.Count - 1].Text = "";
                return Color.Transparent;
            };
            _regiter.Click = delegate()
            {
                _regiter.Text = _regiter.Text.Length == 0 ? CheckBoxCheckedSymbol : "";
                Labels[Labels.Count - 1].Text = "";
                return Color.Transparent;
            };
            Func<Color> save = delegate()
            {
                LogIn();
                return Color.Transparent;
            };
            _saveButton.Click = save;
        }

        /// <summary>
        /// Wygeneruj wszystkie labelki oraz textinputs dla widoku login'a.
        /// </summary>
        /// <param name="color">color textu labelek</param>
        /// <param name="colorInput">color textu textinputs</param>
        /// <param name="texture">textura koloru tła inputtexts</param>
        private void CreateLabelsAndFields(Color color, Color colorInput, Texture2D texture)
        {
            Labels = new List<Label>();
            Fields = new List<TextInput>();
            Labels.Add(new Label(_spriteFont, UserName, color));
            Labels.Add(new Label(_spriteFont, Password, color));
            Labels.Add(new Label(_spriteFontCheckBox, ShowPassword, color));
            Labels.Add(new Label(_spriteFontCheckBox, Register, color));
            Labels.Add(new Label(_spriteFontCheckBox, "", Color.Red));

            Fields.Add(new TextInput(texture, _spriteFont, true, colorInput, TextInputType.Name, MaxNameCharacters));
            Fields.Add(new TextInput(texture, _spriteFont, true, colorInput, TextInputType.Password,
                MaxPasswordCharacters));
            Fields[_inputIndex].Enabled = true;
            foreach (var input in Fields)
            {
                var textInput = input;
                Func<Color> enable = delegate()
                {
                    Fields.ForEach(x => x.Enabled = false);
                    textInput.Enabled = true;
                    Labels[Labels.Count - 1].Text = "";
                    return Color.Transparent;
                };
                textInput.OnClick(enable);
            }
        }

        /// <summary>
        /// Narysuj wszystkie komponenty.
        /// </summary>
        /// <param name="spriteBatch">Obiekt, na którym rysujemy wszytskie komponenty</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var label in Labels)
            {
                label.Draw(spriteBatch);
            }
            foreach (var textInput in Fields)
            {
                textInput.Draw(spriteBatch);
            }
            Vector2 scale = new Vector2(BombWidth/_bombTexture.Width, BombHeight/_bombTexture.Height);
            Rectangle sourceRectangle = new Rectangle(0, 0, _bombTexture.Width, _bombTexture.Height);
            Vector2 origin = new Vector2((float) _bombTexture.Width/2, (float) _bombTexture.Height/2);
            spriteBatch.Draw(_bombTexture, new Vector2(BombWidth/2, BombHeight/2), sourceRectangle, Color.White,
                0.0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_spriteFontTitle, BomberManTitle, new Vector2(Labels[0].Position.X, BombHeight/2),
                Color.White);
            _saveButton.Draw(spriteBatch);
            _regiter.Draw(spriteBatch);
            _showPassword.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Zaktualizuj widok panelu logowania w zależności od czasu trwania gry oraz rozmiaru okna aplikacji.
        /// </summary>
        /// <param name="gameTime">czas trwania gry</param>
        /// <param name="windowWidth">szerokość okna aplikacji</param>
        /// <param name="windowHeight">wysokość okna aplikacji</param>
        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            MouseState mouseState = Mouse.GetState();
            double frameTime = gameTime.ElapsedGameTime.Milliseconds/1000.0;
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;

            float x = (float) windowWidth/2 - LoginPanelWidth/2;
            float y = (float) windowHeight/2 - HeightShift;
            foreach (var label in Labels)
            {
                label.Position = new Vector2(x, y);
                y += _spriteFont.LineSpacing + DataRowsShift;
            }
            y = (float) windowHeight/2 - HeightShift;
            for (int i = 0; i < Fields.Count; i++)
            {
                if (i == 0)
                {
                    Vector2 label = _spriteFont.MeasureString(Labels[i].Text);
                    Fields[i].Position = new Vector2(x + label.X + DataInputShift, y);
                }
                else
                {
                    Fields[i].Position = new Vector2(Fields[i - 1].Position.X, y);
                }
                y += _spriteFont.LineSpacing + DataRowsShift;
                if (Fields[i].Enabled)
                {
                    Fields[i].ProcessKeyboard(false);
                }
                Fields[i].Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            }

            Vector2 scaleCheckbox =
                new Vector2(_spriteFontTitle.MeasureString(CheckBoxCheckedSymbol).X/_showPassword.Texture.Width,
                    _spriteFontTitle.MeasureString(CheckBoxCheckedSymbol).X/_showPassword.Texture.Height);
            Vector2 showPasswordPosition = Fields[Fields.Count - 1].Position;
            _showPassword.Scale = scaleCheckbox;
            float width = scaleCheckbox.X*_showPassword.Texture.Width;
            float height = scaleCheckbox.Y*_showPassword.Texture.Height;
            showPasswordPosition.Y += DataRowsShift;
            showPasswordPosition.Y += SaveHeight/2;
            showPasswordPosition.Y += height/2;
            showPasswordPosition.X += width/2;
            _showPassword.Position = new Vector2(showPasswordPosition.X, showPasswordPosition.Y + DataRowsShift);
            _showPassword.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);

            Vector2 checkBoxRegisterPosition = _showPassword.Position;
            checkBoxRegisterPosition.Y += DataRowsShift + DataInputShift;
            _regiter.Scale = scaleCheckbox;
            _regiter.Position = checkBoxRegisterPosition;
            _regiter.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);

            Vector2 savePosition = _regiter.Position;
            savePosition.X += SaveWidth;
            savePosition.Y += SaveButtonShift;
            savePosition.Y += SaveHeight/2;
            _saveButton.Position = savePosition;
            _saveButton.Scale = new Vector2(
                SaveWidth/_saveButton.Texture.Width, SaveHeight/_saveButton.Texture.Height);
            _saveButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);

            _currentTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_currentTime >= CountDuration)
            {
                _currentTime -= CountDuration;
                HandleKeyboard();
            }
        }

        /// <summary>
        /// Przechwytuj wciśnięte przyciski na klawiaturze i obsłuż je odpowiednio.
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
                    case Keys.Tab:
                    case Keys.Down:
                        _inputIndex++;
                        _inputIndex = _inputIndex >= Fields.Count ? Fields.Count - 1 : _inputIndex;
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

        /// <summary>
        /// Zaloguj użytkownika lub utwórz nowe konto w zależności od zaznaczonej opcji.
        /// </summary>
        private void LogIn()
        {
            String message;
            if (Utils.User == null)
            {
                Utils.User = new UserDao() { Name = Fields[0].TextValue, Password = Fields[1].TextValue };
            }
            else
            {
                Utils.User.Name = Fields[0].TextValue;
                Utils.User.Password = Fields[1].TextValue;
            }
            if (!_regiter.Text.Equals(CheckBoxCheckedSymbol))
            {
                if (UserService.VerificateUser(ref Utils.User, out message))
                {
                    GameManager.ScreenType = ScreenType.MainMenu;
                }
            }
            else
            {
                Utils.User.BombKeyboardOption = BombKeyboardOption.Spcace;
                Utils.User.IsAnimation = true;
                Utils.User.IsMusic = true;
                Utils.User.KeyboardOption = KeyboardOption.Arrows;
                if (UserService.CreateUser(Utils.User, out message))
                {
                    GameManager.ScreenType = ScreenType.MainMenu;
                }
            }
            if (message != null) Labels[Labels.Count - 1].Text = message;
        }
    }
}
