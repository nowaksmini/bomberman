using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberManModel.Entities
{
    public class BoardElement
    {
        public int ID { get; set; }
        public String Description { get; set; }
        public BoardElementType ElementType { get; set; }
    }
}
