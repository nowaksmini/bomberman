using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter rekordu tabeli odpowiedzialnej za przetrzymywanie informacji o grze na obiekt gry
    /// przekazywany do widoku.
    /// </summary>
    public class GameEntityToGameDao : ITypeConverter<Game, GameDao>
    {
        /// <summary>
        /// Konwertuj rekord tabeli z informacją o grze na obiekt przekazywany do widoku.
        /// </summary>
        /// <param name="source">obiekt to konwertowania</param>
        /// <returns>zwróć instancję obiektu gry, przekazywaną do widoku</returns>
        public GameDao Convert(Game source)
        {
            GameDao target = new GameDao
            {
                Finished = source.Finished,
                Id = source.Id,
                Level = source.Level,
                PlayerXLocation = source.PlayerXLocation,
                PlayerYLocation = source.PlayerYLocation,
                Points = source.Points,
                SaveTime = source.SaveTime,
                BombsAmount = source.BombsAmount,
                User = Mapper.Map<UserDao>(source.User)
            };
            return target;
        }

        public GameDao Convert(ResolutionContext context)
        {
            return Convert((Game)context.SourceValue);
        }
    }
}
