using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManModel.Entities
{
    public class Location
    {
        public int ID { get; set; }
        public uint XLocation { get; set; }
        public uint YLocation { get; set; }

        public virtual ICollection<OponentLocation> OponentLocation { get; set; }
    }
}
