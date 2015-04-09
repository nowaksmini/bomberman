using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components
{
    public abstract class MovingComponent : Component
    {
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public Vector2 Position { get; set; }
        public float Size { get; set; }
        private const float SCALE = MathHelper.Pi * 2;

        public MovingComponent(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float size, Color color) : base(texture, color)
        {
            Position = position;
            Angle = angle;
            Velocity = velocity;
            Size = size;
            Velocity = velocity;
        }

        public abstract void Update();

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
             Angle % SCALE, origin, Size, SpriteEffects.None, 0f);
        }

    }
}
