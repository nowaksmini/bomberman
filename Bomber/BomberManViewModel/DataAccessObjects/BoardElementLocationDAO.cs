using System;

namespace BomberManViewModel.DataAccessObjects
{
    /// <summary>
    /// Klasa reprezentująca rozmieszczenie obiektów planszy, przekazywane z widoku do bazy danych i na odwrót
    /// </summary>
    public class BoardElementLocationDao
    {
        public int Id { get; set; }
        public uint XLocation { get; set; }
        public uint YLocation { get; set; }
        /// <summary>
        /// Czas wyrażony w milisekundach oznaczający ilość czasu życia jaki pozostał obiektowi.
        /// Nie dotyczy wszystkich elementów planszy.
        /// </summary>
        public Int64 Timeout { get; set; }
        public virtual BoardElementDao BoardElement { get; set; }
        public virtual GameDao Game { get; set; }
    }
}
