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
        public static bool IsAnimation;
        public static bool IsMouse;
    }

    public enum KeyboardOption
    {
        Arrows,
        Wsad
    }

    public enum BombKeyboardOption
    {
        Spcace,
        P
    }
}
