namespace BomberManModel.Entities
{
    /// <summary>
    /// Encja reprezentująca położenie przeciwników na planszy.
    /// </summary>
    public class OpponentLocation
    {
        public int Id { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        public virtual Opponent Oponent { get; set; }
        public virtual Game Game { get; set; }
    }
}
