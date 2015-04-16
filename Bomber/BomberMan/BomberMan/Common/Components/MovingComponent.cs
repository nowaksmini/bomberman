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

        public MovingComponent(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, Vector2 scale, Color color) : base(texture, color, position, scale, angle)
        {
            Velocity = velocity;
        }

        public abstract void Update();
    }
}
