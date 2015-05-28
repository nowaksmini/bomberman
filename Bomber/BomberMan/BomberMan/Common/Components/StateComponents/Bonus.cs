using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Klasa reprezentująca pojedyncze pole planszy o tle wyszczególnionego bonusa
    /// </summary>
    public class Bonus : Component
    {
        public BonusType BonusType { get; set; }

        /// <summary>
        /// Utwórz bonus na jednostkowym polu. Bonusy podczas uruchomienia poziomu zawsze znajdują się pod
        /// szarymi, zniszczalnymi blokami.
        /// </summary>
        /// <param name="texture">obrazek bonusa</param>
        /// <param name="color">kolor bonusa</param>
        /// <param name="position">pozycja bonusa</param>
        /// <param name="scale">skala rozmiaru bonnusa</param>
        /// <param name="angle">kąt nachylenia bonusa</param>
        /// <param name="bonusType">rodzaj bonusa <example>Uzupełnij życie gracza o 50%</example></param>
        public Bonus(Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle, BonusType bonusType)
            : base(texture, color, position, scale, angle)
        {
            BonusType = bonusType;
        }

    }

    /// <summary>
    /// Rodzaje bonusów :
    /// zwiększ życie o 50%, przyspiesz ruch grcza dwukrotnie na określony czas,
    /// zwolnij ruch gracza dwukrotnie na określony czas, sraw by gracz stał się niezniszczalny na określony czas,
    /// zwiększ moc bomb 1,5 krotnie na określony czas, zwiększ ilość bomb do wykorzystania na planszy o 1
    /// Wszystkie bonusy początkowo znajdują się pod polami szarymi
    /// </summary>
    public enum BonusType
    {
        Life,
        Fast,
        Slow,
        Inmortal,
        Strenght,
        BombAmount
    }
}
