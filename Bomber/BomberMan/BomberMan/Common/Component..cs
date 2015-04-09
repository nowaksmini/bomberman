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

        public Component() { }

        public Component(Texture2D texture, Color color)
        {
            Texture = texture;
            Color = color;
        }
    }
}
