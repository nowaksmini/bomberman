using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter obiektu gry na rekord tabeli Game.
    /// </summary>
    public class GameDaoToGameEntity : ITypeConverter<GameDao, Game>
    {
        /// <summary>
        /// Konwertuj obiekt z widoku z informacją o grze na rekord tabeli Game w bazie danych.
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć encję obiektu gry</returns>
        public Game Convert(GameDao source)
        {
            Game target = new Game
            {
                Finished = source.Finished,
                Id = source.Id,
                Level = source.Level,
                PlayerXLocation = source.PlayerXLocation,
                PlayerYLocation = source.PlayerYLocation,
                Points = source.Points,
                SaveTime = source.SaveTime,
                BombsAmount = source.BombsAmount,
                User = Mapper.Map<User>(source.User)
            };
            return target;
        }

        public Game Convert(ResolutionContext context)
        {
            return Convert((GameDao)context.SourceValue);
        }
    }
}
