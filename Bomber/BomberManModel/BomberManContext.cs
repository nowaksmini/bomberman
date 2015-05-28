using System.Data.Entity;
using BomberManModel.Entities;

namespace BomberManModel
{
    /// <summary>
    /// Klasa odpowiedzialna za generowanie i łączenie się z istniejącą bazą.
    /// Jeżeli nie istnieje już baza danych generuje ją la instancji local.
    /// </summary>
    public class BomberManContext : DbContext 
    {
        public BomberManContext() : base("BomberMan") {}

        public DbSet<Game> Games { get; set; }
        public DbSet<BoardElementLocation> BoardElementLocations { get; set; }
        public DbSet<Opponent> Oponents { get; set; }
        public DbSet<OpponentLocation> OponentLocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BoardElement> BoardElements { get; set; }

    }

    /// <summary>
    /// Rodzaje elementów na planszy <example>Bonus życia</example>
    /// </summary>
    public enum BoardElementType
    {
        WhiteBlock,
        GrayBlock,
        BlackBlock,
        RedBlock,
        Bomb,
        LifeBonus,
        FastBonus,
        SlowBonus,
        InmortalBonus,
        StrenghtBonus,
        BombAmountBonus
    }

    /// <summary>
    /// Rodzaje przeciwników <example>Ośmiornica</example>
    /// </summary>
    public enum OpponentType
    {
        Octopus,
        Ghost
    }
}
