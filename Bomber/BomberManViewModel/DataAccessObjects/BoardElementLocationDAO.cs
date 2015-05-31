using System;

namespace BomberManViewModel.DataAccessObjects
{
    /// <summary>
    /// Klasa reprezentująca rozmieszczenie obiektów planszy, przekazywane z widoku do bazy danych i na odwrót
    /// </summary>
    public class BoardElementLocationDao
    {
        public int Id { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        /// <summary>
        /// Czas wyrażony w sekundach oznaczający ilość czasu życia jaki pozostał obiektowi.
        /// Nie dotyczy wszystkich elementów planszy.
        /// </summary>
        public float? Timeout { get; set; }
        public virtual BoardElementDao BoardElement { get; set; }
        public virtual GameDao Game { get; set; }
    }
}
