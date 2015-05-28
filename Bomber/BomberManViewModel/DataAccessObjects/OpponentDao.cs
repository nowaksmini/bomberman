using System;
using BomberManModel;

namespace BomberManViewModel.DataAccessObjects
{
    /// <summary>
    /// Klasa reprezentująca przeciników, przekazywanych z bazy danych do widoku i odwrotnie.
    /// </summary>
    public class OpponentDao
    {
        public int Id { get; set; }
        public OpponentType OpponentType { get; set; }
        public String Description { get; set; }
    }
}
