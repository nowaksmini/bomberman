using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components
{
    /// <summary>
    /// Klasa bazowa elementów poruszających się z przewidzianym kierunkiem bądź prędkością
    /// </summary>
    public abstract class MovingComponent : Component
    {
        public Vector2 Velocity { get; set; }

        protected MovingComponent(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, Vector2 scale, Color color) : base(texture, color, position, scale, angle)
        {
            Velocity = velocity;
        }

        public abstract void Update();
    }
}
