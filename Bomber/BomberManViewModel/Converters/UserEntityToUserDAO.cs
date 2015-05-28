using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter rekordu tabeli User w bazie danych na obiekt gracza przekazywany do widoku.
    /// </summary>
    public class UserEntityToUserDao : ITypeConverter<User, UserDao>
    {
        /// <summary>
        /// Konwertuj encję użytkownika otrzymaną z bazy danych na obiekt użytkownika widoku.
        /// </summary>
        /// <param name="source">obiekt do konwertowania</param>
        /// <returns>zwróć obiekt użytkownika przekazywany dalej do widoku</returns>
        public UserDao Convert(User source)
        {
            UserDao target = new UserDao
            {
                Id = source.Id,
                Name = source.Name,
                Password = source.Password,
                BombKeyboardOption = source.BombKeyboardOption,
                KeyboardOption = source.KeyboardOption,
                IsAnimation = source.IsAnimation,
                IsMusic = source.IsMusic
            };
            return target;
        }

        public UserDao Convert(ResolutionContext context)
        {
            return Convert((User)context.SourceValue);
        }
    }
}
