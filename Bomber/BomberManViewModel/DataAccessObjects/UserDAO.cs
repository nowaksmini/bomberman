using System;
using BomberManModel.Entities;

namespace BomberManViewModel.DataAccessObjects
{
    /// <summary>
    /// Klasa reprezentująca obiekt gracza przekazywany do bazy danych z widoku i odwrotnie.
    /// </summary>
    public class UserDao
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public bool IsMusic { get; set; }
        public bool IsAnimation { get; set; }
        public KeyboardOption KeyboardOption { get; set; }
        public BombKeyboardOption BombKeyboardOption { get; set; }
    }
}
