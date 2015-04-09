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
        public virtual Location Location { get; set; }
        public virtual Game Game { get; set; }
        public ElementType ElementType { get; set; }
    }
}
