using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter obiektu odpowiedzialnego za przetrzymywanie informacji o przeciniku na planszy
    /// na rekord tabeli Opponent.
    /// </summary>
    public class OpponentDaoToOpponentEntity : ITypeConverter<OpponentDao, Opponent>
    {
        /// <summary>
        /// Konwertuj obiekt z widoku z informacją o przeciwniku na rekord tabeli Opponent w bazie danych.
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć encję obiektu przeciwnika</returns>
        public Opponent Convert(OpponentDao source)
        {
            Opponent target = new Opponent
            {
                Id = source.Id,
                Description = source.Description,
                OpponentType = source.OpponentType
            };
            return target;
        }

        public Opponent Convert(ResolutionContext context)
        {
            return Convert((OpponentDao)context.SourceValue);
        }
    }
}
