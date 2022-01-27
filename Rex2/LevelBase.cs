using Raylib_cs;
using System.Numerics;

namespace Rex2
{
    internal class Player
    {
        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public bool CanJump { get; set; }
        public int HP { get; set; } = 5;
        public int Shield { get; set; }
        public int Ammo { get; set; }
    }

    internal class Enemy
    {
        public int HP { get; set; }
        public Rectangle Rect { get; set; }
    }

    internal class Powerup
    {
        public Rectangle Rect { get; set; }
    }

    internal class LevelDefinition
    {
        public List<Platform> Platforms { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Powerup> Powerups { get; set; }

        public LevelDefinition()
        {
            Platforms = new List<Platform>();
            Enemies = new List<Enemy>();
            Powerups = new List<Powerup>();
        }
    }

    internal class Platform
    {
        public Rectangle Rect { get; set; }
        public bool Blocking { get; set; }
        public Color Color { get; set; }
        public int Durability { get; set; }

        public Platform()
        {
        }

        public Platform(Rectangle rect, bool blocking, Color color)
        {
            this.Rect = rect;
            this.Blocking = blocking;
            this.Color = color;
        }
    }

    internal abstract class LevelBase
    {
        protected int screenHeight;
        protected int screenWidth;
        protected float scale = 2.0f;

        protected RenderTexture2D screenPlayer1;
        protected RenderTexture2D screenPlayer2;
        protected Font font;
        protected int framesCounter;
        protected Timer timer;
        protected TimerCallback timerCallback;

        public int ElapsedTime { get; protected set; }
        public int LevelTime { get; protected set; } = 99;

        public LevelBase(int screenHeight, int screenWidth, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.screenPlayer1 = screenPlayer1;
            this.screenPlayer2 = screenPlayer2;

            font = Raylib.LoadFontEx("assets/opensans.ttf", 48, null, 5000);
        }

        protected virtual void TimerCallback()
        {
        }

        public virtual void Update(float deltaTime)
        {
            framesCounter++;
        }

        public virtual void Unload()
        {
            Raylib.UnloadFont(font);
        }

        protected Vector2 GetVirtualMouse()
        {
            Vector2 mouse = Raylib.GetMousePosition();
            Vector2 virtualMouse = Vector2.Zero;

            virtualMouse.X = mouse.X / scale - screenWidth - 14.5f;
            virtualMouse.Y = (mouse.Y - (screenHeight * 2 - (screenHeight * scale)) * 0.5f) / scale - 27;

            Vector2 max = new Vector2((float)screenWidth, (float)screenHeight);
            virtualMouse = Vector2.Clamp(virtualMouse, Vector2.Zero, max);
            return virtualMouse;
        }

        internal virtual void DrawMain()
        {
        }

        protected void DrawRot13AnimatedTextEx(string text, int timing, int x, int y, int size, Color color)
        {
            Raylib.DrawTextEx(font, text.Rot13TextAnimation(0, framesCounter / timing), new Vector2(x, y), size, 1, color);
        }

        protected void DrawRot13AnimatedText(string text, int timing, int x, int y, int size, Color color)
        {
            Raylib.DrawText(text.Rot13TextAnimation(0, framesCounter / timing), x, y, size, color);
        }

        protected void DrawCenteredTextEx(string m, int y, int fontSize, Color color)
        {
            var width = Raylib.MeasureTextEx(font, m, fontSize, 1);
            Raylib.DrawTextEx(font, m, new Vector2((screenWidth - width.X) / 2, y), fontSize, 1, color);
        }

        protected void DrawCenteredText(string m, int y, int fontSize, Color color)
        {
            var width = Raylib.MeasureText(m, fontSize);
            Raylib.DrawText(m, (screenWidth - width) / 2, y, fontSize, color);
        }

        protected void DrawDialogueText(string m, Color color)
        {
            const int fontSize = 20;
            var width = Raylib.MeasureText(m, fontSize);
            Raylib.DrawText(m, 9 + (628 - (width) / 2), 344 * (int)scale, fontSize, color);
        }

        protected void DrawRemainingTime(int seconds)
        {
            const int fontSize = 19;
            Raylib.DrawText(seconds.ToString(), 315 * (int)scale, 7 * (int)scale, fontSize, Color.GOLD);
        }

        protected void DrawRectangledTextEx(Rectangle container, string text, int fontSize, Color borderColor, Color textColor)
        {
            Raylib.DrawRectangleLinesEx(container, 3, borderColor); // Draw container border

            // Draw text in container (add some padding)
            Raylib.DrawTextRec(font, text,
                       new Rectangle(container.x + 4, container.y + 4, container.width - 4, container.height - 4),
                       fontSize, 2.0f, true, textColor);

            Raylib.DrawRectangleRec(new Rectangle(container.x + container.width - 6, container.y + container.height - 6, 8, 8), borderColor);
        }
    }
}