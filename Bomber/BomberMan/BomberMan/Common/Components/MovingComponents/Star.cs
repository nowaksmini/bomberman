using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BomberMan.Common.Components.MovingComponents
{
    /// <summary>
    /// Klasa odpowiedzialna za generowanie gwiazdek i innych symboli podczas ruchu myszką
    /// </summary>
    public class Star : MovingComponent
    {
        /// <summary>
        /// Czas życia elementu
        /// </summary>
        public int Ttl { get; private set; }
        public float AngularVelocity { get; set; }

        public Star(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, Vector2 scale, int ttl)
            : base (texture, position, velocity, angle, scale, color) 
        {
            Ttl = ttl;
            AngularVelocity = angularVelocity;
        }

        /// <summary>
        /// Uaktualnij pozycję symbolu, zmień kąt poruszania się oraz pozostały czas życia elementu
        /// </summary>
        public override void Update()
        {
            Ttl--;
            Position += Velocity;
            Angle += AngularVelocity;
        }
    }
}
