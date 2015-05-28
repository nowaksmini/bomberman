using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BomberManModel.Entities
{
    /// <summary>
    /// Encja reprezentująca gracza, użytkownika aplikacji BomberMan.
    /// Przechowywane są również dane dotyczące preferencji konfiguracji ustawień gry np. dźwięk.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [MinLength(4)]
        public String Name { get; set; }

        [MinLength(4)]
        public String Password { get; set; }

        public bool IsMusic { get; set; }
        public bool IsAnimation { get; set; }
        public KeyboardOption KeyboardOption { get; set; }
        public BombKeyboardOption BombKeyboardOption { get; set; }

        public virtual ICollection<Game> Game { get; set; }
    }

    /// <summary>
    /// Dostępne opcje poruszania się w grze poprzez ustawienia klawiatury.
    /// </summary>
    public enum KeyboardOption
    {
        Arrows,
        Wsad
    }

    /// <summary>
    /// Dostępne opje klawiszowe ustawienia kalwisza odpowiadającego za zostawienie bomby.
    /// </summary>
    public enum BombKeyboardOption
    {
        Spcace,
        P
    }
}
