using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter rekordu tabeli odpowiedzialnej za przetrzymywanie informacji o przeciwniku
    /// na obiekt przekazywany do widoku.
    /// </summary>
    public class OpponentEntityToOpponentDao : ITypeConverter<Opponent, OpponentDao>
    {
        /// <summary>
        /// Konwertuj rekord tabeli z informacją o przeciwniku na obiek przekazywany do widoku
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć instancję obiektu przeciwnika, przekazywaną do widoku</returns>
        public OpponentDao Convert(Opponent source)
        {
            OpponentDao target = new OpponentDao
            {
                Description = source.Description,
                Id = source.Id,
                OpponentType = source.OpponentType
            };
            return target;
        }

        public OpponentDao Convert(ResolutionContext context)
        {
            return Convert((Opponent)context.SourceValue);
        }
    }
}
