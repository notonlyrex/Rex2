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

        public Rectangle Rect => new Rectangle(Position.X - 9, Position.Y - 32, 18, 32);
    }
}