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
        public DateTime SaveTime { get; set; }
        public UserDAO User { get; set; }
        public LocationDAO PlayerLocation { get; set; }
        public ParticipantStateDAO PlayerState { get; set; }
        public int Points { get; set; }
        public bool Finished { get; set; }
    }
}
