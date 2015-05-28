using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Klasa reprezentująca odpowiednik Label bez możliwości rysowania textury
    /// </summary>
    public class Label : Component
    {
        public String Text { get; set; }
        private readonly SpriteFont _spriteFont;

        /// <summary>
        /// Utwórz nową labelkę.
        /// </summary>
        /// <param name="spriteFont">czcionka</param>
        /// <param name="text">tekst</param>
        /// <param name="color">kolor napisu</param>
        /// <param name="position">pozycja obiektu</param>
        /// <param name="scale">skala rozmiaru obiektu</param>
        /// <param name="angle">kąt nachylenia obiektu</param>
        public Label(SpriteFont spriteFont, String text, Color color, Vector2 position, Vector2 scale,
            float angle)
        {
            Text = text;
            _spriteFont = spriteFont;
            Color = color;
            Position = position;
            Scale = scale;
            Angle = angle;
        }

        /// <summary>
        /// Utwóz nową labelkę z domyślną pozycją <value>0,0</value>,
        /// skalą <value>1,1</value> i kątem nachylenia <value>0</value>
        /// </summary>
        /// <param name="spriteFont">czcionka</param>
        /// <param name="text">tekst</param>
        /// <param name="color">kolor napisu</param>
        public Label(SpriteFont spriteFont, String text, Color color)
        {
            _spriteFont = spriteFont;
            Text = text;
            Color = color;
            Position = new Vector2(0, 0);
            Scale = new Vector2(1, 1);
            Angle = 0;
        }

        /// <summary>
        /// Narysuj w aplikacji labelkę.
        /// </summary>
        /// <param name="spriteBatch">obiekt, w którym rysujemy labelkę</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(0, 0);
            spriteBatch.DrawString(_spriteFont, Text, Position, Color, Angle, origin, Scale, SpriteEffects.None, 0);
        }
    }
}
