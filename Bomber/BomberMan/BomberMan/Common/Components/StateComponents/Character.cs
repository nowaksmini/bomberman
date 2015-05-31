using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Klasa reprezentująca postacie wyświetlane na ekranie: przeciwnicy + gracz
    /// </summary>
    public class Character : Component
    {
        /// <summary>
        /// Zwróć lub ustaw rodzaj postaci.
        /// </summary>
        /// <value>
        /// Rodzaj postaci
        /// </value>
        public CharacterType CharacterType { get; set; }

        /// <summary>
        /// Utwórz nową postać do wyświetlenia na planszy. 
        /// </summary>
        /// <param name="texture">tło postaci</param>
        /// <param name="color">kolor tła postaci</param>
        /// <param name="position">pozycja postaci</param>
        /// <param name="scale">skala rozmiaru postaci</param>
        /// <param name="angle">kąt nachylenia postaci</param>
        /// <param name="characterType">rodzaj postaci <example>Ośmiornica</example></param>
        public Character(Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle, CharacterType characterType)
            : base(texture, color, position, scale, angle)
        {
            CharacterType = characterType;
        }
    }

    /// <summary>
    /// Rodzaje postaci wyświetlanych na planszy
    /// </summary>
    public enum CharacterType
    {
        /// <summary>
        /// Duch
        /// </summary>
        Ghost,
        /// <summary>
        /// Ośmiornica
        /// </summary>
        Octopus,
        /// <summary>
        /// Gracz
        /// </summary>
        Player
    }
}
