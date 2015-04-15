using BomberManModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManModel
{
    public class BomberManContext : DbContext 
    {
        public BomberManContext() : base("BomberMan") {}

        public DbSet<Game> Games { get; set; }
        public DbSet<BoardElementLocation> BoardElementLocations { get; set; }
        public DbSet<Oponent> Oponents { get; set; }
        public DbSet<OponentLocation> OponentLocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BoardElement> BoardElements { get; set; }

    }

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

    public enum OpponentType
    {
        Octopus,
        Ghost
    }
}
