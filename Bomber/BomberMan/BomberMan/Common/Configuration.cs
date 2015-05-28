using System;

namespace BomberMan.Common
{
    /// <summary>
    /// Reprezentuje ustawienia aplikacji związane z Setting'sami
    /// </summary>
    public static class Configuration
    {
        public static bool IsMusic;

        /// <summary>
        /// Ustawienia konfiguracji poruszania się gracza
        /// </summary>
        public static KeyboardOption KeyboardOption = KeyboardOption.Arrows;

        /// <summary>
        /// Konfiguracja zostawiania bomby
        /// </summary>
        public static BombKeyboardOption BombKeyboardOption = BombKeyboardOption.Spcace;

        public static String Name;
        public static String Password;
        public static bool IsAnimation = true;
        public static bool IsMouse = true;

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
