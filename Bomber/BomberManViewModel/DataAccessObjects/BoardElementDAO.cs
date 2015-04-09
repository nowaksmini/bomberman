using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.DataAccessObjects
{
    public class BoardElementDAO
    {
        public int ID { get; set; }
        public String Description { get; set; }
        public LocationDAO Location { get; set; }
        public GameDAO Game { get; set; }
        public BomberManModel.ElementType ElementType { get; set; }
    }
}
