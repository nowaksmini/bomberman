using BomberManViewModel.DataAccessObjects;

namespace BomberMan
{
    /// <summary>
    /// Klasa przechowująca informacje na temat zalogowanego uczestnika i aktualnie toczącej się gry.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Aktualnie trwająca gra
        /// </summary>
        public static GameDAO Game;
        /// <summary>
        /// Zalogowany użytkownik
        /// </summary>
        public static UserDAO User;
    }
}
