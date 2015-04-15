using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManModel.Entities
{
    public class Oponent
    {
        public int ID { get; set; }
        public String Description { get; set; }
        public OpponentType OpponentType { get; set; }
        public virtual ICollection<OponentLocation> OponentLocation { get; set; }
    }
}
