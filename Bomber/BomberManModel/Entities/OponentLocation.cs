using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManModel.Entities
{
    public class OponentLocation
    {
        public int ID { get; set; }

        public virtual Oponent Oponent { get; set; }
        public virtual Location Location { get; set; }
        public virtual ParticipantState State { get; set; }
        public virtual Game Game { get; set; }
    }
}
