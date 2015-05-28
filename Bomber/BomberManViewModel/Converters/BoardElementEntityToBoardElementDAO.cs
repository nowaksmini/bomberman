using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter rekordu tabeli odpowiedzialnej za przetrzymywanie informacji o elementach planszy
    /// na obiekt przekazywany do widoku.
    /// </summary>
    public class BoardElementEntityToBoardElementDao : ITypeConverter<BoardElement, BoardElementDao>
    {
        /// <summary>
        /// Konwertuj rekord tabeli z informacją o elemencie planszy na obiek przekazywany do widoku
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć instancję obiektu elementu planszy, przekazywaną do widoku</returns>
        public BoardElementDao Convert(BoardElement source)
        {
            BoardElementDao target = new BoardElementDao
            {
                Description = source.Description,
                ElementType = source.ElementType,
                Id = source.Id
            };
            return target;
        }

        public BoardElementDao Convert(ResolutionContext context)
        {
            return Convert((BoardElement)context.SourceValue);
        }
    }
}
