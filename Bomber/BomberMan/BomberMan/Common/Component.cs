using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common
{
    /// <summary>
    /// Klasa bazowa reprezentująca wszystkie obiekty pojawiające się w aplikacji.
    /// </summary>
    public class Component
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public float Angle { get; set; }
        public Vector2 Scale { get; set; }
        private const float AngleScale = MathHelper.Pi * 2;

        public Component() { }

        /// <summary>
        /// Utwórz nowy komponent.
        /// </summary>
        /// <param name="texture">tło komponentu</param>
        /// <param name="color">kolor komponentu</param>
        /// <param name="position">pozycja wektorowa komponentu</param>
        /// <param name="scale">skala wektorowa rozmiaru komponentu</param>
        /// <param name="angle">kąt obrotu komponentu</param>
        public Component(Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle)
        {
            Texture = texture;
            Color = color;
            Position = position;
            Scale = scale;
            Angle = angle;
        }

        /// <summary>
        /// Narysuj na podanym SpriteBatch komponent.
        /// </summary>
        /// <param name="spriteBatch">Obiekt, do ktorego dorysowujemy własny komponent</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
             Angle % AngleScale, origin, Scale, SpriteEffects.None, 0f);
        }
    }
}
