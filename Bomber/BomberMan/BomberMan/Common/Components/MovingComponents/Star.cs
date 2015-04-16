using BomberMan.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.MovingComponents
{
    public class Star : MovingComponent
    {
        public int TTL { get; set; }
        public float AngularVelocity { get; set; }

        public Star(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, Vector2 scale, int ttl)
            : base (texture, position, velocity, angle, scale, color) 
        {
            TTL = ttl;
            AngularVelocity = angularVelocity;
        }

        public override void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }
    }
}
