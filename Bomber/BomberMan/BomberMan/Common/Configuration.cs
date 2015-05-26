using System;

namespace BomberMan.Common
{
    public static class Configuration
    {
        public static bool isMusic;
        public static KeybordOption keybordOption;
        public static String name;
        public static String password;
        public static bool isAnimation;
        public static bool isMouse;
    }

    public enum KeybordOption
    {
        Arrows,
        Wsad
    }
}
