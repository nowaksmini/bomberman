using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Klasa reprezentująca EditText z możliwościć wpisywania niewyświetlanego hasła
    /// </summary>
    public class TextInput
    {
        private const int LongDelay = 500; // milisekundy
        private const int ShortDelay = 50;
        public string TextValue { get; set; }
        private int _time;
        private KeyboardState _keyboardState;
        private KeyboardState _lastKeyboardState;
        private DateTime _prevUpdate = DateTime.Now;
        private readonly SpriteFont _font;
        public bool ShowCursor;
        private readonly Color _textColor;
        public Vector2 Position { get; set; }
        private readonly Button _button;
        private readonly Texture2D _texture;
        private readonly int _maxCharacters;

        public TextInputType TextInputType { get; set; }
        public bool Enabled { get; set; }

        /// <summary>
        /// Utwórz nowe pole edytowalne.
        /// </summary>
        /// <param name="texture">kolor tła pola</param>
        /// <param name="font">czcionka tekstu</param>
        /// <param name="showCursor"><value>true</value> oznacza pokazywanie kursowa, <value>false</value> brak</param>
        /// <param name="textColor">kolor czcionki</param>
        /// <param name="textInputType">rodzaj pola <value>Hasło</value> oznacza zamianę wpisywanych znaków na symbole "*"</param>
        /// <param name="buttonColor">kolor tła przycisku</param>
        /// <param name="maxCharacters">maksymalna ilość znaków</param>
        public TextInput(Texture2D texture, SpriteFont font, bool showCursor,
            Color textColor, TextInputType textInputType, Color buttonColor, int maxCharacters = Int32.MaxValue)
        {
            _maxCharacters = maxCharacters;
            TextInputType = textInputType;
            _font = font;
            _texture = texture;
            TextValue = String.Empty;
            ShowCursor = showCursor;
            _textColor = textColor;
            _button = new Button(BState.Up, texture, buttonColor, new Vector2(0, 0), new Vector2(1, 1), 0.0f, 2.0f, Color.Black);
        }

        /// <summary>
        /// Obsłuż wpisywane znaki na klawiaturze, każdy osobno.
        /// </summary>
        /// <param name="capsLock"><value>true</value> oznacza włączony CapsLock</param>
        public void ProcessKeyboard(bool capsLock)
        {
            _keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = _keyboardState.GetPressedKeys();
            if (CheckPressedKeys(pressedKeys))
            {
                string keys = Convert(pressedKeys, capsLock);
                foreach (char x in keys)
                {
                    //process backspace
                    if (x == '\b')
                    {
                        if (TextValue.Length >= 1)
                        {
                            TextValue = TextValue.Remove(TextValue.Length - 1, 1);
                        }
                    }
                    else
                    {
                        if (TextValue.Length < _maxCharacters)
                            TextValue += x;
                    }
                }
            }
            _lastKeyboardState = _keyboardState;
        }

        /// <summary>
        /// Uaktualnij 
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="frameTime"></param>
        /// <param name="mousePressed"></param>
        /// <param name="prevMousePressed"></param>
        public void Update(int mx, int my, double frameTime, bool mousePressed, bool prevMousePressed)
        {
            _button.Update(mx, my, frameTime, mousePressed, prevMousePressed);
            String cursor = Enabled ? "_" : "";
            String password = "";
            for (int i = 0; i < TextValue.Length; i++)
                password += "*";
            String text = TextInputType == TextInputType.Name ? TextValue : password;
            float width = _font.MeasureString(text + cursor).X;
            float height = _font.MeasureString(text + cursor).Y;
            Vector2 scale = new Vector2(width/_texture.Width,
                height/_texture.Height);
            _button.Scale = scale;
            _button.Position = new Vector2(Position.X + width/2, Position.Y + height/2);
        }

        /// <summary>
        /// Narysuj edytowalne pole tekstowe na ekranie aplikacji.
        /// </summary>
        /// <param name="spriteBatch">obiekt, na którym rysujemy pole telstowe</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            String password = "";
            for (int i = 0; i < TextValue.Length; i++)
                password += "*";
            String text = TextInputType == TextInputType.Name ? TextValue : password;
            Vector2 stringSize = _font.MeasureString(text);
            _button.Draw(spriteBatch);
            spriteBatch.DrawString(_font, text, Position, _textColor);
            if (ShowCursor && Enabled)
            {
                spriteBatch.DrawString(_font, "_", new Vector2(Position.X + stringSize.X, Position.Y), _textColor);
            }
        }

        /// <summary>
        /// Zamień wciśnięty przycisk na klawiaturze na pojedynczy znak.
        /// </summary>
        /// <param name="keys">wciśnięte znaki na klawiaturze</param>
        /// <param name="capsLock"><value>true</value> oznacza, że wciśnięto CapsLock</param>
        /// <returns></returns>
        private string Convert(Keys[] keys, bool capsLock)
        {
            string output = "";
            bool useShift = (keys.Contains(Keys.LeftShift) || keys.Contains(Keys.RightShift));
            bool useCaps = ((capsLock && !useShift) || (useShift && !capsLock));
            if (keys.Length > 0)
            {
                Keys key = keys[0];
                if (key >= Keys.A && key <= Keys.Z)
                    output += key.ToString();
                else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    output += (key - Keys.NumPad0).ToString();
                else if (key >= Keys.D0 && key <= Keys.D9)
                {
                    string num = (key - Keys.D0).ToString();

                    #region special num chars

                    if (useCaps)
                    {
                        switch (num)
                        {
                            case "1":
                                num = "!";
                                break;
                            case "2":
                                num = "@";
                                break;
                            case "3":
                                num = "#";
                                break;
                            case "4":
                                num = "$";
                                break;
                            case "5":
                                num = "%";
                                break;
                            case "6":
                                num = "^";
                                break;
                            case "7":
                                num = "&";
                                break;
                            case "8":
                                num = "*";
                                break;
                            case "9":
                                num = "(";
                                break;
                            case "0":
                                num = ")";
                                break;
                        }
                    }

                    #endregion

                    output += num;
                }
                else if (key == Keys.OemPeriod)
                    output += ".";
                else if (key == Keys.OemTilde)
                    output += "'";
                else if (key == Keys.Space)
                    output += " ";
                else if (key == Keys.OemMinus)
                    output += "-";
                else if (key == Keys.OemPlus)
                    output += "+";
                else if (key == Keys.OemQuestion && useCaps)
                    output += "?";
                else if (key == Keys.Back) //backspace
                    output += "\b";
                if (!useCaps)
                    output = output.ToLower();
            }
            return output;
        }

        /// <summary>
        /// Sprawdź czy można wpisywać nowy znak do pola tekstowego w zależności od czasu ostatniego wpisania znaku.
        /// </summary>
        /// <param name="pressedKeys">wciśnięte znaki</param>
        /// <returns></returns>
        private bool CheckPressedKeys(Keys[] pressedKeys)
        {
            bool keyPressed = false;
            DateTime now = DateTime.Now;
            {
                foreach (Keys key in pressedKeys)
                {
                    if (_lastKeyboardState.IsKeyUp(key))
                    {
                        _time = LongDelay;
                        _prevUpdate = now;
                        keyPressed = true;
                        break;
                    }
                    if (now.Subtract(_prevUpdate).Milliseconds > _time)
                    {
                        _time = ShortDelay;
                        _prevUpdate = now;
                        keyPressed = true;
                        break;
                    }
                }
            }
            return keyPressed;
        }

        /// <summary>
        /// Metoda wywoływana podczas kliknięcia w pole tekstowe.
        /// </summary>
        /// <param name="clickAction">funkjca wywoływana po naciśnięciu na edytowalne pole tekstowe</param>
        public void OnClick(Func<Color> clickAction)
        {
            _button.Click = clickAction;
        }
    }

    /// <summary>
    /// Rodzaje pól teksowych: zwykłe, hasło
    /// </summary>
    public enum TextInputType
    {
        Name,
        Password
    }
}
