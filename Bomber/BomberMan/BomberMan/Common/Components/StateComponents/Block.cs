using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Klasa reprezentująca pojedyncze pole planszy o tle wyszczególnionego kwadratu jednostkowego
    /// </summary>
    public class Block : Component
    {
        public BlockType BlockType { get; set; }

        /// <summary>
        /// Utwórz pojedyncze pole planszy z wybranym tłem.
        /// </summary>
        /// <param name="texture">tło jednostkowego pola</param>
        /// <param name="color">kolor jednostkowego pola</param>
        /// <param name="position">pozycja jednostkowego pola</param>
        /// <param name="scale">skala rozmiaru jednostkowego pola</param>
        /// <param name="angle">kąt nachylenia jednostkowego pola</param>
        /// <param name="blockType">rodzaj jednostkowego pola <example>Zniszczalne</example></param>
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
