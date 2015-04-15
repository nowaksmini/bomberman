using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.StateComponents
{
    public class Block : StateComponent
    {
        public BlockKind BlockType { get; set; }

        public Block (Texture2D texture, Color color, Rectangle rectangle, BlockKind blockType) 
            : base(texture, color, rectangle)
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
