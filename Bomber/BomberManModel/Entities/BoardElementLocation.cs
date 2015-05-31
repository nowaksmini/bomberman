using System;

namespace BomberManModel.Entities
{
    /// <summary>
    /// Encja reprezentująca położenie elementów nie poruszających się na planszy.
    /// </summary>
    public class BoardElementLocation
    {
        public int Id { get; set; }
        /// <summary>
        /// Lokalizacja zgodna z oznaczeniami na mapie np punkt 0,0 to pole w lewym górnym rogu planszy
        /// </summary>
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        /// <summary>
        /// Czas w sekundach ile pozostało życia np bonus prędkości
        /// Jeżeli mamy null znaczy, że nie dotyczy np pole planszy Black
        /// </summary>
        public float? Timeout { get; set; } 
        public virtual BoardElement BoardElement{ get; set; }
        public virtual Game Game { get; set; }
    }
}
