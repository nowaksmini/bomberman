namespace BomberManViewModel.DataAccessObjects
{
    /// <summary>
    /// Klasa reprezentująca rozmieszczenie przeciwników na planszy,
    /// przekazywane z widoku do bazy danych i odwrotnie.
    /// </summary>
    public class OpponentLocationDao
    {
        public int Id { get; set; }
        public OpponentDao Oponent { get; set; }
        public uint XLocation { get; set; }
        public uint YLocation { get; set; }
        public GameDao Game { get; set; }
    }
}
