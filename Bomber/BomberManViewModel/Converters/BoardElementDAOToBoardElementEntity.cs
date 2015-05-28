using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter obiektu odpowiedzialnego za przetrzymywanie informacji o elemencie planszy
    /// na rekord tabeli BoardElement.
    /// </summary>
    public class BoardElementDaoToBoardElementEntity : ITypeConverter<BoardElementDao, BoardElement>
    {
        /// <summary>
        /// Konwertuj obiekt z widoku z informacją o elemencie planszy na rekord tabeli BoardElement w bazie danych.
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć encję obiektu elementu planszy</returns>
        public BoardElement Convert(BoardElementDao source)
        {
            BoardElement target = new BoardElement
            {
                Description = source.Description,
                ElementType = source.ElementType,
                Id = source.Id
            };
            return target;
        }

        public BoardElement Convert(ResolutionContext context)
        {
            return Convert((BoardElementDao)context.SourceValue);
        }
    }
}
