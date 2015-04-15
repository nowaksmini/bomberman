using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberManViewModel.DataAccessObjects
{
    public class BoardElementLocationDAO
    {
        public int ID { get; set; }
        public uint XLocation { get; set; }
        public uint YLocation { get; set; }
        public int Timeout { get; set; } // czas w milisekndach ile np zostało życia bombie czy przyspieszeniu jak < 0 to znaczy że umarło
        public virtual BoardElementDAO BoardElement { get; set; }
        public virtual GameDAO Game { get; set; }
    }
}
