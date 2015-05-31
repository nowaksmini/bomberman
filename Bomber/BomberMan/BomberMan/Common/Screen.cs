using System.Collections.Generic;
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
        protected int PrevSelectedOption, SelectedOption;
        protected KeyboardState KeyboardState, LastKeyboardState;
        protected bool MousePressed, PrevMousePressed = false;
        protected const int Gap = 10;

        /// <summary>
        /// Narysuj wszystkie komponenty znajdujące się w silniku w aplikacji.
        /// </summary>
        /// <param name="spriteBatch">obiekt, w którym rysujemy komponenty</param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Uaktualnij pozycje i rozmiary komponentów znajdujących się w silniku.
        /// </summary>
        /// <param name="gameTime">czas trwania gry</param>
        /// <param name="windowWidth">szerokość okna aplikacji</param>
        /// <param name="windowHeight">wysokość okna aplikacji</param>
        public abstract void Update(GameTime gameTime, int windowWidth, int windowHeight);

        /// <summary>
        /// Obsłuż wciskane klawisze na klawiaturze.
        /// </summary>
        public abstract void HandleKeyboard();
    }
}
