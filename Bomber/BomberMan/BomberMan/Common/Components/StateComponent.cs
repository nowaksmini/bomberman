using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components
{
    public class StateComponent : Component
    {
        public Rectangle Rectangle { get; set; }

        public StateComponent() : base() { }

        public StateComponent(Texture2D texture, Color color, Rectangle rectangle)
            : base (texture, color)
        {
            Rectangle = rectangle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }
    }
}
