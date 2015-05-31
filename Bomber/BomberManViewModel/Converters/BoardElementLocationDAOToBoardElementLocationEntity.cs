using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter obiektu odpowiedzialnego za przetrzymywanie informacji o lokalizacji elementów na planszy
    /// na rekord tabeli BoardElementLocation.
    /// </summary>
    public class BoardElementLocationDaoToBoardElementLocationEntity : ITypeConverter<BoardElementLocationDao, BoardElementLocation>
    {
        /// <summary>
        /// Konwertuj obiekt z widoku z informacją o lokalizacji elementów na planszy na rekord tabeli BoardElementLocation w bazie danych.
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć encję obiektu informującą o lokalizacji elementów na planszy</returns>
        public BoardElementLocation Convert(BoardElementLocationDao source)
        {
            BoardElementLocation target = new BoardElementLocation();
            target.YLocation = source.YLocation;
            target.XLocation = source.XLocation;
            target.Id = source.Id;
            target.Game = Mapper.Map<Game>(source.Game);
            target.BoardElement = Mapper.Map<BoardElement>(source.BoardElement);
            target.Timeout = source.Timeout;
            return target;
        }

        public BoardElementLocation Convert(ResolutionContext context)
        {
            return Convert((BoardElementLocationDao)context.SourceValue);
        }
    }
}
