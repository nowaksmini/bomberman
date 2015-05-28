using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components
{
    /// <summary>
    /// Klasa bazowa elementów poruszających się z przewidzianym kierunkiem bądź prędkością.
    /// </summary>
    public abstract class MovingComponent : Component
    {
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Utwóz nowy poruszający się komponent.
        /// </summary>
        /// <param name="texture">tło komponentu</param>
        /// <param name="position">pozycja startowa komponentu</param>
        /// <param name="velocity">prędkośc poruszania się</param>
        /// <param name="angle">kąt nachylenia</param>
        /// <param name="scale">skala rozmiaru</param>
        /// <param name="color">kolor komponentu</param>
        protected MovingComponent(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, Vector2 scale, Color color) : base(texture, color, position, scale, angle)
        {
            Velocity = velocity;
        }

        /// <summary>
        /// Metoda wykonywana podczas aktualizacji rozmiarów i położenia komponentu.
        /// </summary>
        public abstract void Update();
    }
}
