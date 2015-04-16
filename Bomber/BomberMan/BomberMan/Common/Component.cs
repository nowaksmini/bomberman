using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common
{
    public class Component
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public float Angle { get; set; }
        public Vector2 Scale { get; set; }
        private const float ANGLE_SCALE = MathHelper.Pi * 2;

        public Component() { }

        public Component(Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle)
        {
            Texture = texture;
            Color = color;
            Position = position;
            Scale = scale;
            Angle = angle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
             Angle % ANGLE_SCALE, origin, Scale, SpriteEffects.None, 0f);
        }
    }
}
