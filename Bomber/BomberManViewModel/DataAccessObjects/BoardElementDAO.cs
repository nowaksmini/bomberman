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
        public BomberManModel.BoardElementType ElementType { get; set; }
    }
}
