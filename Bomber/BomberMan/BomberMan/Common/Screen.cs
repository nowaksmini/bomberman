using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan
{
    public abstract class Screen
    {
        public Button Music;
        public static Texture2D[] MusicTexture;
        public Button Back;
        public static Texture2D BackTexture;
        public int prevSelectedOption, selectedOption;
        public KeyboardState KeyboardState, LastKeyboardState;
        public bool mousePressed, prevMousePressed = false;
        public const int GAP = 10;

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime, int windowWidth, int windowHeight);
        public abstract void HandleKeyboard();
    }
}
