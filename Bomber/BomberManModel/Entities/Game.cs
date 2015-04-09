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
        public DateTime SaveTime { get;set; }
        public virtual User User { get; set; }
        public virtual Location PlayerLocation { get; set; }
        public virtual ParticipantState PlayerState { get; set; }

        public virtual ICollection<OponentLocation> OponentLocation { get; set; }
        public virtual ICollection<BoardElement> BoardElement { get; set; }
    }
}
