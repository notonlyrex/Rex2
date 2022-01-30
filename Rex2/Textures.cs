using Raylib_cs;

namespace Rex2
{
    internal static class Textures
    {
        public static Texture2D Platform = Raylib.LoadTexture("assets/platform.png");
        public static Texture2D DestructiblePlatform = Raylib.LoadTexture("assets/destructible-platform.png");
        public static Texture2D CrystalRed = Raylib.LoadTexture("assets/crystal-red.png");
        public static Texture2D CrystalGreen = Raylib.LoadTexture("assets/crystal-green.png");
        public static Texture2D CrystalBlue = Raylib.LoadTexture("assets/crystal-blue.png");
        public static Texture2D CrystalYellow = Raylib.LoadTexture("assets/crystal-yellow.png");
        public static Texture2D CrystalWhite = Raylib.LoadTexture("assets/crystal-white.png");
        public static Texture2D BasePlatform = Raylib.LoadTexture("assets/base-platform.png");
        public static Texture2D Enemy = Raylib.LoadTexture("assets/enemy.png");
        public static Texture2D Enemy2 = Raylib.LoadTexture("assets/enemy2.png");
        public static Texture2D Boss = Raylib.LoadTexture("assets/boss.png");
        public static Texture2D Player = Raylib.LoadTexture("assets/player.png");
        public static Texture2D Powerup = Raylib.LoadTexture("assets/powerup.png");
        public static Texture2D BackgroundScreen1 = Raylib.LoadTexture("assets/background.png");

        public static Color Background = new Color(9, 37, 34, 255);
    }
}