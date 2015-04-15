using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberManModel.Entities
{
    public class BoardElementLocation
    {
        public int ID { get; set; }
        public uint XLocation { get; set; }
        public uint YLocation { get; set; }
        public int Timeout { get; set; } // czas w milisekndach ile np zostało życia bombie czy przyspieszeniu jak < 0 to znaczy że umarło
        public virtual BoardElement BoardElement{ get; set; }
        public virtual Game Game { get; set; }
    }
}
