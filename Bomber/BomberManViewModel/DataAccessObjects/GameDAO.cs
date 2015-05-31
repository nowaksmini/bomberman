using System;

namespace BomberManViewModel.DataAccessObjects
{
    /// <summary>
    /// Klasa reprezentuje grę wczytaną z widoku i przekazywaną do bazy danych lub odwrotnie.
    /// </summary>
    public class GameDao
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Points { get; set; }
        public bool Finished { get; set; }
        public DateTime SaveTime { get; set; }
        public virtual UserDao User { get; set; }
        public int PlayerXLocation { get; set; }
        public int PlayerYLocation { get; set; }
        public int BombsAmount { get; set; }
    }
}
