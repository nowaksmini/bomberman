using System;

namespace BomberManModel.Entities
{
    /// <summary>
    /// Encja reprezentująca elementy nie poruszające się na planszy.
    /// </summary>
    public class BoardElement
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public BoardElementType ElementType { get; set; }
    }
}
