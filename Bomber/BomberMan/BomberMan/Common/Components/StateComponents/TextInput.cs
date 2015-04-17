using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.StateComponents
{
    public class TextInput
    {
        private const int longDelay = 500; // milisekundy
        private const int shortDelay = 50;
        private string theValue;
        private int time;
        private KeyboardState keyboardState;
        private KeyboardState lastKeyboardState;
        private DateTime prevUpdate = DateTime.Now;


        public TextInput()
        {
            theValue = String.Empty;
        }

        public void Update(int windowWidth, int windowHeight)
        { }

        public void ProcessKeyboard(bool capsLock)
        {
            keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();
            if (CheckPressedKeys(pressedKeys))
            {
                string keys = Convert(pressedKeys, capsLock);
                foreach (char x in keys)
                {
                    //process backspace
                    if (x == '\b')
                    {
                        if (theValue.Length >= 1)
                        {
                            theValue = theValue.Remove(theValue.Length - 1, 1);
                        }
                    }
                    else
                    {
                        theValue += x;
                    }
                }
            }
            lastKeyboardState = keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, bool showCursor)
        {
            const string nameLabel = "Name: ";
            Vector2 stringSize = font.MeasureString(nameLabel);
            Vector2 stringSize2 = font.MeasureString(theValue);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, nameLabel, Vector2.Zero, Color.Black);
            spriteBatch.DrawString(font, theValue, new Vector2(stringSize.X, 0), Color.Black);
            if (showCursor)
            {
                spriteBatch.DrawString(font, "_", new Vector2(stringSize.X + stringSize2.X, 0), Color.Black);
            }
            spriteBatch.End();
        }

        private string Convert(Keys[] keys, bool capsLock)
        {
            string output = "";
            bool useShift = (keys.Contains(Keys.LeftShift) || keys.Contains(Keys.RightShift));
            bool useCaps = ((capsLock && !useShift) || (useShift && !capsLock));
            // Lets process only first key in buffer. Multiple key pres is not supported (wanted).
            if (keys.Length > 0)
            {
                Keys key = keys[0];
                if (key >= Keys.A && key <= Keys.Z)
                    output += key.ToString();
                else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    output += ((int)(key - Keys.NumPad0)).ToString();
                else if (key >= Keys.D0 && key <= Keys.D9)
                {
                    string num = ((int)(key - Keys.D0)).ToString();
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
                            default:
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
                    if (lastKeyboardState.IsKeyUp(key))
                    {
                        time = longDelay;
                        prevUpdate = now;
                        keyPressed = true;
                        break;
                    }
                    else
                    {
                        if (now.Subtract(prevUpdate).Milliseconds > time)
                        {
                            time = shortDelay;
                            prevUpdate = now;
                            keyPressed = true;
                            break;
                        }
                    }
                }
            }
            return keyPressed;
        }

        public enum TextInputType
        {
            Name,
            Password
        }
    }
}
