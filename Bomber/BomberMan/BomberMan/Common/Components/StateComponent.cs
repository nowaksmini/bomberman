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
        public int Height { get; set; }
        public int Width { get; set; }
        public Rectangle Rectangle { get; set; }

        public StateComponent() : base() { }

        public StateComponent(Texture2D texture, Color color, int height, int width, Rectangle rectangle)
            : base (texture, color)
        {
            Height = height;
            Width = width;
            Rectangle = rectangle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }
    }
}
