using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BomberManModel.Entities
{
    public class User
    {
        public int ID { get; set; }
        [MinLength(4)]
        public String Name { get; set; }
        public String Password { get; set; }

        public virtual ICollection<Game> Game { get; set; }

    }
}
