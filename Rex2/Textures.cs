using Raylib_cs;

namespace Rex2
{
    internal static class Textures
    {
        public static Texture2D Platform = Raylib.LoadTexture("assets/platform.png");
        public static Texture2D DestructiblePlatform = Raylib.LoadTexture("assets/destructible-platform.png");
        public static Texture2D Jewel = Raylib.LoadTexture("assets/jewel.png");
        public static Texture2D BasePlatform = Raylib.LoadTexture("assets/base-platform.png");
        public static Texture2D Enemy = Raylib.LoadTexture("assets/enemy.png");
        public static Texture2D Boss = Raylib.LoadTexture("assets/boss.png");
        public static Texture2D Player = Raylib.LoadTexture("assets/player.png");

        public static Color Background = new Color(9, 37, 34, 255);
    }
}