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
        public DbSet<Location> Locations { get; set; }
        public DbSet<Oponent> Oponents { get; set; }
        public DbSet<OponentLocation> OponentLocations { get; set; }
        public DbSet<ParticipantState> ParticipantStates { get; set; }
        public DbSet<User> Users { get; set; }

    }

    public enum ElementType
    {
        SimpleBlock,
        ComplexBlock,
        Bomb,
        LifeBonus,
        FastBonus,
        SlowBonus,
        InmortalBonus,
        StrenghtBonus,
        BombAmountBonus
    }
}
