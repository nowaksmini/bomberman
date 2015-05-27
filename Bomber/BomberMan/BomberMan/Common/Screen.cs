using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberMan.Common
{
    /// <summary>
    /// Klasa reprezentująca widok poszczególnego okna <example>Menu, Gra, Opcje</example>
    /// </summary>
    public abstract class Screen
    {
        public Button Music;
        public static Texture2D[] MusicTexture;
        public Button Back;
        protected int PrevSelectedOption, SelectedOption;
        protected KeyboardState KeyboardState, LastKeyboardState;
        protected bool MousePressed, PrevMousePressed = false;
        protected const int Gap = 10;

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime, int windowWidth, int windowHeight);
        public abstract void HandleKeyboard();

    }
}
