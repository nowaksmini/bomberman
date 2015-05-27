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

        public void Update(int windowWidth, int windowHeight)
        { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(0,0);
            spriteBatch.DrawString(_spriteFont, Text, Position, Color, Angle, origin, Scale, SpriteEffects.None, 0 );
        }
    }
}
