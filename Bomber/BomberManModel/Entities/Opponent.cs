using System;
using System.Collections.Generic;

namespace BomberManModel.Entities
{
    /// <summary>
    /// Encja reprezentująca przeciwników gracza.
    /// </summary>
    public class Opponent
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public OpponentType OpponentType { get; set; }
        public virtual ICollection<OpponentLocation> OponentLocation { get; set; }
    }
}
