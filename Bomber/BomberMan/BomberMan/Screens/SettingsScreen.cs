using System;
using System.Collections.Generic;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberManModel.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Screens.Menus
{
    public class SettingsScreen : Screen
    {
        private const String MoveKeys = "Poruszanie się";
        private const String BombKey = "Zostawianie bomby";
        private const String Music = "Muzyka";
        private const String Animation = "Aniamcje";
        private const String UserName = "Login";
        private const String Password = "Hasło";
        private const String SaveChanges = "Zapisz zmiany";
        private const String ShowPassword = "Pokaż hasło";
        private const float ShiftX = 250;
        private const float ShiftY = 200;
        private const float ShiftRow = 40;
        private const float ButtonsShift = 50;
        private static readonly Color LabelsColor = Color.White;
        private static readonly Color ErrorColor = Color.Red;
        private static readonly Color SelectedOptionColor = Color.BlueViolet;
        public List<TextInput> TextInputs;
        private List<Label> _labels;
        private List<Button> _buttons;
        private readonly SpriteFont _spriteFontLables;
        private readonly SpriteFont _spriteFontAdditionalOption;
        private readonly List<Texture2D> _buttonsTextures; 


        public SettingsScreen(SpriteFont spriteFontLabels, SpriteFont spriteFontAdditionalOption, List<Texture2D> buttonsTextures)
        {

            _buttonsTextures = buttonsTextures;
            _spriteFontAdditionalOption = spriteFontAdditionalOption;
            _spriteFontLables = spriteFontLabels;
            GenerateLabels();
        }

        private void GenerateLabels()
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
                new Label(_spriteFontAdditionalOption, "XX", ErrorColor)
            };
            _buttons = new List<Button>();
            if (Utils.User != null)
            {
                if (Utils.User.KeyboardOption == KeyboardOption.Arrows)
                {

                }
                if (Utils.User.BombKeyboardOption == BombKeyboardOption.Spcace)
                {

                }
            }
            
            foreach (Texture2D texture in _buttonsTextures)
            {
                _buttons.Add(new Button(texture, LabelsColor, null, ""));
            }
        }

        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            double frameTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            MouseState mouseState = Mouse.GetState();
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;
            float x = (float)windowWidth/2 - ShiftX;
            float y = (float)windowHeight/2 - ShiftY;
            float maxLabelWidth = 0.0f;
            float maxLabelHeight = 0.0f;
            foreach (var label in _labels)
            {
                label.Position = new Vector2(x,y);
                maxLabelWidth = Math.Max(maxLabelWidth, _spriteFontLables.MeasureString(label.Text).X);
                maxLabelHeight = Math.Max(maxLabelHeight, _spriteFontLables.MeasureString(label.Text).Y);
                y += ShiftRow;
            }
            y = (float)windowHeight/2 - ShiftY + maxLabelHeight/2;
            float width = maxLabelWidth/2;
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].Scale = new Vector2(width/_buttons[i].Texture.Width,
                    maxLabelHeight/_buttons[i].Texture.Height);
                if (i%2 == 0)
                {
                    _buttons[i].Position = new Vector2(x + maxLabelWidth + ButtonsShift + width/2, y);
                }
                else
                {
                    _buttons[i].Position = new Vector2(_buttons[i-1].Position.X + width, y);
                    y += ShiftRow;
                }
                _buttons[i].Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            }
            HandleKeyboard();
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var label in _labels)
            {
                label.Draw(spriteBatch);
            }
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public override void HandleKeyboard()
        {
            
        }

    }
}
