using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.MovingComponents
{
    public class Rocket : MovingComponent
    {
        public Rocket(Texture2D texture, Vector2 position, Vector2 velocity,
                float angle, Color color, float size)
            : base(texture, position, velocity, angle, size, color) { }

        public override void Update()
        {
            Position += Velocity;
        }

    }
}
