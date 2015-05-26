using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components.MovingComponents
{
    /// <summary>
    /// Klasa odpowiedzialna za tworzenie rakiet w tle podczas trwania całej aplikacji
    /// </summary>
    public class Rocket : MovingComponent
    {
        public Rocket(Texture2D texture, Vector2 position, Vector2 velocity,
                float angle, Color color, Vector2 scale)
            : base(texture, position, velocity, angle, scale, color) { }

        /// <summary>
        /// Uaktualnij pozycję rakiety, która porusza się z określoną prędkością w zadanym kierunku
        /// </summary>
        public override void Update()
        {
            Position += Velocity;
        }

    }
}
