using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter obiektu odpowiedzialnego za przetrzymywanie informacji o lokalizacji przeciwników na planszy
    /// na rekord tabeli OpponentLocation.
    /// </summary>
    public class OpponentLocationDaoToOpponentLocationEntity : ITypeConverter<OpponentLocationDao, OpponentLocation>
    {
        /// <summary>
        /// Konwertuj obiekt z widoku z informacją o lokalizacji przeciwnika na rekord tabeli OpponentLocation w bazie danych.
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć encję obiektu informującą o lokalizacji przeciwnika</returns>
        public OpponentLocation Convert(OpponentLocationDao source)
        {
            OpponentLocation target = new OpponentLocation
            {
                Game = Mapper.Map<Game>(source.Game),
                Id = source.Id,
                XLocation = (int) source.XLocation,
                YLocation = (int) source.YLocation,
                Oponent = Mapper.Map<Opponent>(source.Oponent)
            };
            return target;
        }

        public OpponentLocation Convert(ResolutionContext context)
        {
            return Convert((OpponentLocationDao)context.SourceValue);
        }
    }
}
