using System;
using System.Collections.Generic;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberManModel.Entities;
using BomberManViewModel.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens
{
    public class SettingsScreen : Screen
    {
        private const int MaxNameCharacters = 15;
        private const int MaxPasswordCharacters = 15;
        private const String MoveKeys = "Poruszanie się";
        private const String BombKey = "Zostawianie bomby";
        private const String Music = "Muzyka";
        private const String Animation = "Aniamcje";
        private const String UserName = "Login";
        private const String Password = "Nowe Hasło";
        private const String ShowPassword = "Pokaż hasło";
        private const String CheckBoxCheckedSymbol = "x";
        private const float ShiftX = 250;
        private const float ShiftY = 200;
        private const float ShiftRow = 40;
        private const float ButtonsShift = 50;
        private static readonly Color ChceckBoxColor = Color.Black;
        private static readonly Color LabelsColor = Color.White;
        private static readonly Color ErrorColor = Color.Red;
        private static readonly Color SelectedOptionColor = Color.BlueViolet;
        public List<TextInput> TextInputs;
        private List<Label> _labels;
        private List<Button> _buttons;
        private readonly SpriteFont _spriteFontLables;
        private readonly SpriteFont _spriteFontAdditionalOption;
        private readonly List<Texture2D> _buttonsTextures;
        private readonly Texture2D _checkBoxTexture;
        private readonly Texture2D _saveChangesTexture;
        private int _inputIndex;
        private readonly Button _backButton;

        /// <summary>
        /// Klasa odpowiedzialna za widok ustawień konta użytkownika
        /// </summary>
        /// <param name="spriteFontLabels">czcionka dla labelek</param>
        /// <param name="spriteFontAdditionalOption">czcionka dla dodatkowych opcji</param>
        /// <param name="buttonsTextures">tło przycisków</param>
        /// <param name="checkBoxTexture">tło chceckboxów</param>
        /// <param name="saveChangesTexture">tło przycisku zapisz zamiany</param>
        public SettingsScreen(SpriteFont spriteFontLabels, SpriteFont spriteFontAdditionalOption,
            List<Texture2D> buttonsTextures, Texture2D checkBoxTexture, Texture2D saveChangesTexture, Texture2D backButtonTexture)
        {
            _checkBoxTexture = checkBoxTexture;
            _buttonsTextures = buttonsTextures;
            _spriteFontAdditionalOption = spriteFontAdditionalOption;
            _spriteFontLables = spriteFontLabels;
            _saveChangesTexture = saveChangesTexture;
            GenerateLabelsAndTextInputs();
            CreateButtons();
            _backButton = new Button(backButtonTexture, LabelsColor, null, "", LabelsColor)
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

        /// <summary>
        /// Utwórz wszytskie labelki na ekranie.
        /// </summary>
        private void GenerateLabelsAndTextInputs()
        {
            _labels = new List<Label>
            {
                new Label(_spriteFontLables, MoveKeys, LabelsColor),
                new Label(_spriteFontLables, BombKey, LabelsColor),
                new Label(_spriteFontLables, Music, LabelsColor),
                new Label(_spriteFontLables, Animation, LabelsColor),
                new Label(_spriteFontLables, UserName, LabelsColor),
                new Label(_spriteFontLables, Password, LabelsColor),
                new Label(_spriteFontAdditionalOption, ShowPassword, LabelsColor),
                new Label(_spriteFontAdditionalOption, "", ErrorColor)
            };
            TextInputs = new List<TextInput>();
            TextInputs.Add(new TextInput(_checkBoxTexture, _spriteFontLables, true, Color.Black, TextInputType.Name,
                MaxNameCharacters));
            TextInputs.Add(new TextInput(_checkBoxTexture, _spriteFontLables, true, Color.Black, TextInputType.Password,
                MaxPasswordCharacters));
            TextInputs[_inputIndex].Enabled = true;
            if (Utils.User != null)
            {
                TextInputs[0].TextValue = Utils.User.Name;
            }
            foreach (var input in TextInputs)
            {
                var textInput = input;
                Func<Color> enable = delegate()
                {
                    TextInputs.ForEach(x => x.Enabled = false);
                    textInput.Enabled = true;
                    _labels[_labels.Count - 1].Text = "";
                    return Color.Transparent;
                };
                textInput.OnClick(enable);
            }
        }

        /// <summary>
        /// Utwórz wszytskie przyciski na ekranie Settings.
        /// </summary>
        private void CreateButtons()
        {
            _buttons = new List<Button>();
            foreach (Texture2D texture in _buttonsTextures)
            {
                _buttons.Add(new Button(texture, LabelsColor, null, "", ChceckBoxColor));
            }
            _buttons.Add(new Button(_checkBoxTexture, LabelsColor, _spriteFontLables,
                Utils.User.IsMusic ? CheckBoxCheckedSymbol : "", ChceckBoxColor));
            _buttons.Add(new Button(_checkBoxTexture, LabelsColor, _spriteFontLables,
                Utils.User.IsAnimation ? CheckBoxCheckedSymbol : "", ChceckBoxColor));
            _buttons.Add(new Button(_checkBoxTexture, LabelsColor, _spriteFontLables, "", ChceckBoxColor));
            _buttons.Add(new Button(_saveChangesTexture, LabelsColor, null, "", LabelsColor));
            CreateClickHandlersForButtons();
        }

        /// <summary>
        /// Utwórz obsługę przyciskania przycisków.
        /// </summary>
        private void CreateClickHandlersForButtons()
        {
            if (Utils.User.KeyboardOption == KeyboardOption.Arrows)
            {
                _buttons[0].NormalColor = SelectedOptionColor;
                _buttons[1].NormalColor = LabelsColor;
            }
            else
            {
                _buttons[0].NormalColor = SelectedOptionColor;
                _buttons[1].NormalColor = LabelsColor;
            }
            if (Utils.User.BombKeyboardOption == BombKeyboardOption.Spcace)
            {
                _buttons[2].NormalColor = SelectedOptionColor;
                _buttons[3].NormalColor = LabelsColor;
            }
            else
            {
                _buttons[2].NormalColor = SelectedOptionColor;
                _buttons[3].NormalColor = LabelsColor;
            }
            _buttons[0].Click = delegate
            {
                _buttons[1].NormalColor = LabelsColor;
                Utils.User.KeyboardOption = KeyboardOption.Arrows;
                return SelectedOptionColor;
            };
            _buttons[1].Click = delegate
            {
                _buttons[0].NormalColor = LabelsColor;
                Utils.User.KeyboardOption = KeyboardOption.Wsad;
                return SelectedOptionColor;
            };
            _buttons[2].Click = delegate
            {
                _buttons[3].NormalColor = LabelsColor;
                Utils.User.BombKeyboardOption = BombKeyboardOption.Spcace;
                return SelectedOptionColor;
            };
            _buttons[3].Click = delegate
            {
                _buttons[2].NormalColor = LabelsColor;
                Utils.User.BombKeyboardOption = BombKeyboardOption.P;
                return SelectedOptionColor;
            };
            _buttons[_buttons.Count - 2].Click = delegate()
            {
                _buttons[_buttons.Count - 2].Text = _buttons[_buttons.Count - 2].Text.Length == 0
                    ? CheckBoxCheckedSymbol
                    : "";
                TextInputs[1].TextInputType = TextInputs[1].TextInputType == TextInputType.Name
                    ? TextInputType.Password
                    : TextInputType.Name;
                _labels[_labels.Count - 1].Text = "";
                return Color.Transparent;
            };
            _buttons[_buttons.Count - 3].Click = delegate()
            {
                _buttons[_buttons.Count - 3].Text = _buttons[_buttons.Count - 3].Text.Length == 0
                    ? CheckBoxCheckedSymbol
                    : "";
                Utils.User.IsAnimation = !Utils.User.IsAnimation;
                return Color.Transparent;
            };
            _buttons[_buttons.Count - 4].Click = delegate()
            {
                _buttons[_buttons.Count - 4].Text = _buttons[_buttons.Count - 4].Text.Length == 0
                    ? CheckBoxCheckedSymbol
                    : "";
                Utils.User.IsMusic = !Utils.User.IsMusic;
                return Color.Transparent;
            };
            _buttons[_buttons.Count - 1].Click = delegate()
            {
                SaveChanges();
                return Color.Transparent;
            };
        }

        /// <summary>
        /// Zapisz dotychczasową konfigurację. Ta opcja próbuje nadpisać hasło znajdujące się w bazie danych.
        /// </summary>
        private void SaveChanges()
        {
            String message;
            Utils.User.Name = TextInputs[0].TextValue;
            Utils.User.Password = TextInputs[1].TextValue;
            if (!UserService.UpdateUser(Utils.User, out message))
            {
                _labels[_labels.Count - 1].Text = "Zapisano pomyślnie zmiany";
            }
            else
            {
                _labels[_labels.Count - 1].Text = message;
            }
        }

        /// <summary>
        /// Uaktualnij widok całego panelu
        /// </summary>
        /// <param name="gameTime">czas gry</param>
        /// <param name="windowWidth">szerokość okna aplikacji</param>
        /// <param name="windowHeight">wysokość okna aplikcaji</param>
        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            double frameTime = gameTime.ElapsedGameTime.Milliseconds/1000.0;
            MouseState mouseState = Mouse.GetState();
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;
            float x = (float) windowWidth/2 - ShiftX;
            float y = (float) windowHeight/2 - ShiftY;
            float maxLabelWidth = 0.0f;
            float maxLabelHeight = 0.0f;
            foreach (var label in _labels)
            {
                if(label == _labels[_labels.Count-1]) continue;
                label.Position = new Vector2(x, y);
                maxLabelWidth = Math.Max(maxLabelWidth, _spriteFontLables.MeasureString(label.Text).X);
                maxLabelHeight = Math.Max(maxLabelHeight, _spriteFontLables.MeasureString(label.Text).Y);
                y += ShiftRow;
            }
            y = (float) windowHeight/2 - ShiftY + maxLabelHeight/2;
            float width = maxLabelWidth/2;
            for (int i = 0; i < 4; i++)
            {
                _buttons[i].Scale = new Vector2(width/_buttons[i].Texture.Width,
                    maxLabelHeight/_buttons[i].Texture.Height);
                if (i%2 == 0)
                {
                    _buttons[i].Position = new Vector2(x + maxLabelWidth + ButtonsShift + width/2, y);
                }
                else
                {
                    _buttons[i].Position = new Vector2(_buttons[i - 1].Position.X + width, y);
                    y += ShiftRow;
                }
            }
            width = (_spriteFontLables.MeasureString(CheckBoxCheckedSymbol).X + 3);
            for (int i = 4; i < 6; i++)
            {
                _buttons[i].Scale = new Vector2(width/_buttons[i].Texture.Width,
                    _spriteFontLables.MeasureString(CheckBoxCheckedSymbol).Y/_buttons[i].Texture.Height);
                _buttons[i].Position = new Vector2(x + maxLabelWidth + ButtonsShift + width/2, y);
                y += ShiftRow;
            }
            foreach (TextInput textInput in TextInputs)
            {
                textInput.Position = new Vector2(x + maxLabelWidth + ButtonsShift, y - maxLabelHeight/2);
                if (textInput.Enabled)
                {
                    textInput.ProcessKeyboard(false);
                }
                textInput.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
                y += ShiftRow;
            }
            _buttons[_buttons.Count - 2].Scale = new Vector2(width/_buttons[_buttons.Count - 2].Texture.Width,
                _spriteFontLables.MeasureString(CheckBoxCheckedSymbol).Y/_buttons[_buttons.Count - 2].Texture.Height);
            _buttons[_buttons.Count - 2].Position = new Vector2(x + maxLabelWidth + ButtonsShift + width/2, y);
            y += ShiftRow;
            _buttons[_buttons.Count - 1].Scale = new Vector2(maxLabelWidth * 0.8f/_buttons[_buttons.Count - 1].Texture.Width,
                maxLabelHeight*2/_buttons[_buttons.Count - 1].Texture.Height);
            _buttons[_buttons.Count - 1].Position = new Vector2(x + maxLabelWidth + maxLabelWidth, y + ButtonsShift);

            foreach (var button in _buttons)
            {
                button.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            }
            _backButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            
            HandleKeyboard();
        }

        /// <summary>
        /// Narysuj wszytskie komponenty.
        /// </summary>
        /// <param name="spriteBatch">obiekt do rysowania komponentów</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _backButton.Draw(spriteBatch);
            foreach (var label in _labels)
            {
                label.Draw(spriteBatch);
            }
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
            foreach (var textInput in TextInputs)
            {
                textInput.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Obsłuż wciskane przyciski na klawiaturze.
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
                        _inputIndex = _inputIndex >= TextInputs.Count ? TextInputs.Count - 1 : _inputIndex;
                        TextInputs.ForEach(x => x.Enabled = false);
                        TextInputs[_inputIndex].Enabled = true;
                        break;
                    case Keys.Up:
                        _inputIndex--;
                        _inputIndex = _inputIndex < 0 ? 0 : _inputIndex;
                        _inputIndex = _inputIndex%TextInputs.Count;
                        TextInputs.ForEach(x => x.Enabled = false);
                        TextInputs[_inputIndex].Enabled = true;
                        break;
                    case Keys.Enter:
                        _buttons[_buttons.Count - 1].Click();
                        break;
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
