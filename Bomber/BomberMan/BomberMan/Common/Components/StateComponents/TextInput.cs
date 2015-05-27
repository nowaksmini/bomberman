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
        private string _textValue;
        private int _time;
        private KeyboardState _keyboardState;
        private KeyboardState _lastKeyboardState;
        private DateTime _prevUpdate = DateTime.Now;
        private readonly SpriteFont _font;
        public bool ShowCursor;
        private readonly Color _color;
        public Vector2 Position { get; set; }
        private readonly Button _button;
        private readonly Texture2D _texture;
        private readonly TextInputType _textInputType;
        private readonly int _maxCharacters = Int32.MaxValue;

        public bool Enabled { get; set; }

        public TextInput(Texture2D texture, SpriteFont font, bool showCursor, Color color, TextInputType textInputType, int maxCharacters = Int32.MaxValue)
        {
            _maxCharacters = maxCharacters;
            _textInputType = textInputType;
            _font = font;
            _texture = texture;
            _textValue = String.Empty;
            ShowCursor = showCursor;
            _color = color;
            _button = new Button(BState.Up, texture, color, new Vector2(0,0), new Vector2(1,1), 0.0f, 2.0f);
        }

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
                        if (_textValue.Length >= 1)
                        {
                            _textValue = _textValue.Remove(_textValue.Length - 1, 1);
                        }
                    }
                    else
                    {
                        if(_textValue.Length < _maxCharacters)
                            _textValue += x;
                    }
                }
            }
            _lastKeyboardState = _keyboardState;
        }

        public void Update(int mx, int my, double frameTime, bool mousePressed, bool prevMousePressed)
        {
            _button.Update(mx, my, frameTime, mousePressed, prevMousePressed);
            String cursor = Enabled ? "_" : "";
            float width = _font.MeasureString(_textValue + cursor).X;
            float height = _font.MeasureString(_textValue + cursor).Y;
            Vector2 scale = new Vector2(width/_texture.Width, 
                                        height/ _texture.Height);
            _button.Scale = scale;
            _button.Position = new Vector2(Position.X + width/2, Position.Y + height/2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            String password = "";
            for (int i = 0; i < _textValue.Length; i++)
                password += "*";
            String text = _textInputType == TextInputType.Name ? _textValue : password;
            Vector2 stringSize = _font.MeasureString(text);
            spriteBatch.Begin();
            _button.Draw(spriteBatch);
            spriteBatch.DrawString(_font, text, Position, _color);
            if (ShowCursor && Enabled)
            {
                spriteBatch.DrawString(_font, "_", new Vector2(Position.X + stringSize.X, Position.Y), _color);
            }
            spriteBatch.End();
        }

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
                    else
                    {
                        if (now.Subtract(_prevUpdate).Milliseconds > _time)
                        {
                            _time = ShortDelay;
                            _prevUpdate = now;
                            keyPressed = true;
                            break;
                        }
                    }
                }
            }
            return keyPressed;
        }

        public void OnClick(Func<Color> clickAction)
        {
            _button.Click = clickAction;
        }
    }

    public enum TextInputType
    {
        Name,
        Password
    }
}
