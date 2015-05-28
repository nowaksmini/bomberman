using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Converters
{
    /// <summary>
    /// Konwerter obiektu gracz otrzymanego z widoku na rekord tabeli User w bazie danych.
    /// </summary>
    public class UserDaoToUserEntity : ITypeConverter<UserDao, User>
    {
        /// <summary>
        /// Konwertuj otrzymany obiekt z widoku na rekord tabeli.
        /// </summary>
        /// <param name="source">obiekt do konwertowania</param>
        /// <returns>zwróć encję gracza</returns>
        public User Convert(UserDao source)
        {
            User target = new User
            {
                Id = source.Id,
                Name = source.Name,
                Password = source.Password,
                BombKeyboardOption = source.BombKeyboardOption,
                IsAnimation = source.IsAnimation,
                IsMusic = source.IsMusic,
                KeyboardOption = source.KeyboardOption
            };
            return target;
            
        }

        public User Convert(ResolutionContext context)
        {
            return Convert((UserDao)context.SourceValue);
        }
    }
}
