using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter rekordu tabeli odpowiedzialnej za przetrzymywanie informacji o lokalizacji elementów na planszy
    /// na obiekt przekazywany do widoku.
    /// </summary>
    public class BoardElementLocationEntityToBoardElementLocationDao : ITypeConverter<BoardElementLocation, BoardElementLocationDao>
    {
        /// <summary>
        /// Konwertuj rekord tabeli z informacją o lokalizacji elementów na planszy na obiek przekazywany do widoku
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć instancję obiektu informującą o lokalizacji elementów na planszy, przekazywaną do widoku</returns>
        public BoardElementLocationDao Convert(BoardElementLocation source)
        {
            BoardElementLocationDao target = new BoardElementLocationDao();
            target.YLocation = target.YLocation;
            target.XLocation = target.XLocation;
            target.Id = source.Id;
            target.Game = Mapper.Map<GameDao>(source.Game);
            target.BoardElement = Mapper.Map<BoardElementDao>(source.BoardElement);
            target.Timeout = source.Timeout;
            return target;
        }

        public BoardElementLocationDao Convert(ResolutionContext context)
        {
            return Convert((BoardElementLocation)context.SourceValue);
        }
    }
}
