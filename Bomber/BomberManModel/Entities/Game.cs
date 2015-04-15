using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManModel.Entities
{
    public class Game
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public int Points { get; set; }
        public bool Finished { get; set; }  // czy jest sens ładować
        public double Life { get; set; } // procentowo npp 10,23
        public DateTime SaveTime { get;set; }
        public virtual User User { get; set; }
        public uint PlayerXLocation { get; set; }
        public uint PlayerYLocation { get; set; }

        public virtual ICollection<OponentLocation> OponentLocation { get; set; }
        public virtual ICollection<BoardElementLocation> BoardElementLocation { get; set; }
    }
}
