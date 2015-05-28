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

        /// <summary>
        /// Utwórz nowy sybol pojawiający się podczas ruchu myszką.
        /// </summary>
        /// <param name="texture">tło obiektu</param>
        /// <param name="position">pozycja startowa obiektu</param>
        /// <param name="velocity">prędkość poruszania się obiektu w pionie i poziomie</param>
        /// <param name="angle">kąt nachylenia obiektu</param>
        /// <param name="angularVelocity">prędkość kątowa obiektu</param>
        /// <param name="color">kolor obiektu</param>
        /// <param name="scale">skala rozmiaru obiektu</param>
        /// <param name="ttl">czas życia obiektu</param>
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
