using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Klasa reprezentująca postacie wyświetlane na ekranie: przeciwnicy + gracz
    /// </summary>
    public class Character : Component
    {
        public CharacterType CharacterType { get; set; }

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
        Ghost,
        Octopus,
        Player
    }
}
