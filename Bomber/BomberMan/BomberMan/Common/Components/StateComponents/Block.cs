using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BomberManModel;

namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Klasa reprezentująca pojedyncze pole planszy o tle wyszczególnionego kwadratu jednostkowego
    /// </summary>
    public class Block : Component
    {
        public BlockType BlockType { get; set; }

        public Block(Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle, BlockType blockType)
            : base(texture, color, position, scale, angle)
        {
            BlockType = blockType;
        }

    }

    /// <summary>
    /// Rodzaje jednostkowych kwadratów
    /// Niezniszczalny, Zniszczalny, W trakcie niszczenia, Zwykły
    /// </summary>
    public enum BlockType
    {
        Black,
        Grey,
        Red,
        White
    }

}
