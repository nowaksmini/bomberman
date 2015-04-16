using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.StateComponents
{
    public class Block : Component
    {
        public BlockKind BlockType { get; set; }

        public Block (Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle,  BlockKind blockType) 
            : base(texture, color, position, scale, angle)
        {
            BlockType = blockType;
        }

    }

    public enum BlockKind
    {
        Black,
        Grey,
        Red,
        White
    }

}
