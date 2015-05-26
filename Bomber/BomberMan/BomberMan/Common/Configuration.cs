using System;

namespace BomberMan.Common
{
    /// <summary>
    /// Reprezentuje ustawienia aplikacji związane z Setting'sami
    /// </summary>
    public static class Configuration
    {
        public static bool IsMusic;
        public static KeybordOption KeybordOption;
        public static String Name;
        public static String Password;
        public static bool IsAnimation;
        public static bool IsMouse;
    }

    public enum KeybordOption
    {
        Arrows,
        Wsad
    }
}
