using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter rekordu tabeli odpowiedzialnej za przetrzymywanie informacji o lokalizacji przeciwników na planszy
    /// na obiekt przekazywany do widoku.
    /// </summary>
    public class OpponentLocationEntityToOpponentLocationDao : ITypeConverter<OpponentLocation, OpponentLocationDao>
    {
        /// <summary>
        /// Konwertuj rekord tabeli z informacją o lokalizacji przeciwnika na obiek przekazywany do widoku
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć instancję obiektu informującą o lokalizacji przeciwnika, przekazywaną do widoku</returns>
        public OpponentLocationDao Convert(OpponentLocation source)
        {
            OpponentLocationDao target = new OpponentLocationDao
            {
                Game = Mapper.Map<GameDao>(source.Game),
                Id = source.Id,
                XLocation = (uint) source.XLocation,
                YLocation = (uint) source.YLocation,
                Oponent = Mapper.Map<OpponentDao>(source.Oponent)
            };
            return target;
        }

        public OpponentLocationDao Convert(ResolutionContext context)
        {
            return Convert((OpponentLocation)context.SourceValue);
        }
    }
}
