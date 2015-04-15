using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.DataAccessObjects
{
    public class GameDAO
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public int Points { get; set; }
        public bool Finished { get; set; }  // czy jest sens ładować
        public double Life { get; set; } // procentowo npp 10,23
        public DateTime SaveTime { get; set; }
        public virtual UserDAO User { get; set; }
        public uint PlayerXLocation { get; set; }
        public uint PlayerYLocation { get; set; }
    }
}
