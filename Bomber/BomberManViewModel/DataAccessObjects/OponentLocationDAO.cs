using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.DataAccessObjects
{
    public class OponentLocationDAO
    {
        public int ID { get; set; }
        public OponentDAO Oponent { get; set; }
        public LocationDAO Location { get; set; }
        public ParticipantStateDAO State { get; set; }
        public GameDAO Game { get; set; }
    }
}
