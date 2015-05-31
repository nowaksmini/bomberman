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
    /// <summary>
    /// Klasa reprezentująca widok zmiany ustawień konta użytkownika.
    /// </summary>
    public class SettingsScreen : Screen
    {
        private const int MaxNameCharacters = 15;
        private const int MaxPasswordCharacters = 15;
        private const String MoveKeys = "Poruszanie się";
        private const String BombKey = "Zostawianie bomby";
        private const String Music = "Muzyka";
        private const String Animation = "Aniamcje";
        private const String UserName = "Nowy Login";
        private const String Password = "Nowe Hasło";
        private const String ShowPassword = "Pokaż hasło";
        private const String CheckBoxCheckedSymbol = "x";
        private const float ShiftX = 250;
        private const float ShiftY = 200;
        private const float ShiftRow = 40;
        private const float ButtonsShift = 50;
        private const int ArrowsButtonIndex = 0;
        private const int WsadButtonIndex = 1;
        private const int SpaceButtonIndex = 2;
        private const int PButtonIndex = 3;
        private const int MusicCheckboxIndex = 4;
        private const int AnimationCheckboxIndex = 5;
        private const int ShowPasswordCheckboxIndex = 6;
        private const int SaveButtonIndex = 7;
        private const int UserNameInputIndex = 0;
        private const int PasswordInputIndex = 1;
        private static readonly Color ChceckBoxColor = Color.Black;
        private static readonly Color LabelsColor = Color.White;
        private static readonly Color ErrorColor = Color.Red;
        private static readonly Color SelectedOptionColor = Color.BlueViolet;
        private List<TextInput> _textInputs;
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
            _textInputs = new List<TextInput>
            {
                new TextInput(_checkBoxTexture, _spriteFontLables, true, Color.Black, TextInputType.Name, Color.White,
                    MaxNameCharacters),
                new TextInput(_checkBoxTexture, _spriteFontLables, true, Color.Black, TextInputType.Password, Color.White,
                    MaxPasswordCharacters)
            };
            _textInputs[_inputIndex].Enabled = true;
            if (Utils.User != null)
            {
                _textInputs[0].TextValue = Utils.User.Name;
            }
            foreach (var input in _textInputs)
            {
                var textInput = input;
                Func<Color> enable = delegate()
                {
                    _textInputs.ForEach(x => x.Enabled = false);
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
                _buttons[ArrowsButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[WsadButtonIndex].NormalColor = LabelsColor;
            }
            else
            {
                _buttons[WsadButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[ArrowsButtonIndex].NormalColor = LabelsColor;
            }
            if (Utils.User.BombKeyboardOption == BombKeyboardOption.Spcace)
            {
                _buttons[SpaceButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[PButtonIndex].NormalColor = LabelsColor;
            }
            else
            {
                _buttons[PButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[SpaceButtonIndex].NormalColor = LabelsColor;
            }
            _buttons[ArrowsButtonIndex].Click = delegate
            {
                _buttons[ArrowsButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[WsadButtonIndex].NormalColor = LabelsColor;
                Utils.User.KeyboardOption = KeyboardOption.Arrows;
                return SelectedOptionColor;
            };
            _buttons[WsadButtonIndex].Click = delegate
            {
                _buttons[WsadButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[ArrowsButtonIndex].NormalColor = LabelsColor;
                Utils.User.KeyboardOption = KeyboardOption.Wsad;
                return SelectedOptionColor;
            };
            _buttons[SpaceButtonIndex].Click = delegate
            {
                _buttons[SpaceButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[PButtonIndex].NormalColor = LabelsColor;
                Utils.User.BombKeyboardOption = BombKeyboardOption.Spcace;
                return SelectedOptionColor;
            };
            _buttons[PButtonIndex].Click = delegate
            {
                _buttons[PButtonIndex].NormalColor = SelectedOptionColor;
                _buttons[SpaceButtonIndex].NormalColor = LabelsColor;
                Utils.User.BombKeyboardOption = BombKeyboardOption.P;
                return SelectedOptionColor;
            };
            _buttons[ShowPasswordCheckboxIndex].Click = delegate()
            {
                _buttons[ShowPasswordCheckboxIndex].Text = _buttons[ShowPasswordCheckboxIndex].Text.Length == 0
                    ? CheckBoxCheckedSymbol
                    : "";
                _textInputs[PasswordInputIndex].TextInputType = _textInputs[PasswordInputIndex].TextInputType == TextInputType.Name
                    ? TextInputType.Password
                    : TextInputType.Name;
                _labels[_labels.Count - 1].Text = "";
                return Color.Transparent;
            };
            _buttons[AnimationCheckboxIndex].Click = delegate()
            {
                _buttons[AnimationCheckboxIndex].Text = _buttons[AnimationCheckboxIndex].Text.Length == 0
                    ? CheckBoxCheckedSymbol
                    : "";
                Utils.User.IsAnimation = !Utils.User.IsAnimation;
                return Color.Transparent;
            };
            _buttons[MusicCheckboxIndex].Click = delegate()
            {
                _buttons[MusicCheckboxIndex].Text = _buttons[MusicCheckboxIndex].Text.Length == 0
                    ? CheckBoxCheckedSymbol
                    : "";
                Utils.User.IsMusic = !Utils.User.IsMusic;
                return Color.Transparent;
            };
            _buttons[SaveButtonIndex].Click = delegate()
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
            bool withPassword = false;
            if (_textInputs[1].TextValue.Length > 0)
            {
                Utils.User.Name = _textInputs[0].TextValue;
                Utils.User.Password = _textInputs[1].TextValue;
                withPassword = true;
            }
            if (UserService.UpdateUser(Utils.User, out message, withPassword))
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
            foreach (TextInput textInput in _textInputs)
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
            _labels[_labels.Count - 1].Position = new Vector2(_buttons[_buttons.Count - 1].Position.X
                - _spriteFontLables.MeasureString(_labels[_labels.Count - 1].Text).X / 2,
                _buttons[_buttons.Count - 2].Position.Y
                + _spriteFontLables.MeasureString(_labels[_labels.Count - 1].Text).Y / 2);
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
            foreach (var textInput in _textInputs)
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
                        _inputIndex = _inputIndex >= _textInputs.Count ? _textInputs.Count - 1 : _inputIndex;
                        _textInputs.ForEach(x => x.Enabled = false);
                        _textInputs[_inputIndex].Enabled = true;
                        break;
                    case Keys.Up:
                        _inputIndex--;
                        _inputIndex = _inputIndex < 0 ? 0 : _inputIndex;
                        _inputIndex = _inputIndex%_textInputs.Count;
                        _textInputs.ForEach(x => x.Enabled = false);
                        _textInputs[_inputIndex].Enabled = true;
                        break;
                    case Keys.Enter:
                        _buttons[_buttons.Count - 1].Click();
                        break;
                    case Keys.Back:
                    case Keys.Escape:
                    case Keys.Home:
                        _backButton.Click();
                        break;
                }
            }
        }

        /// <summary>
        /// Załaduj ustawienia użytkownika z bazy.
        /// </summary>
        public void LoadUserSettings()
        {
            String message;
            if (!UserService.LoadSettingsToUser(ref Utils.User, out message))
            {
                if (message != null)
                {
                    _labels[_labels.Count - 1].Text = message;
                }
            }
            else
            {
                _buttons[AnimationCheckboxIndex].Text = Utils.User.IsAnimation
                    ? CheckBoxCheckedSymbol : String.Empty;
                _buttons[MusicCheckboxIndex].Text = Utils.User.IsMusic
                    ? CheckBoxCheckedSymbol : String.Empty;
                _buttons[ShowPasswordCheckboxIndex].Text = String.Empty;
                if (Utils.User.BombKeyboardOption == BombKeyboardOption.P)
                    _buttons[PButtonIndex].Click();
                else _buttons[SpaceButtonIndex].Click();
                if (Utils.User.KeyboardOption == KeyboardOption.Arrows)
                    _buttons[ArrowsButtonIndex].Click();
                else _buttons[WsadButtonIndex].Click();
                _labels[_labels.Count - 1].Text = "";
                _textInputs[UserNameInputIndex].TextValue = Utils.User.Name;
                _textInputs[PasswordInputIndex].TextValue = String.Empty;
            }
        }
    }
}
