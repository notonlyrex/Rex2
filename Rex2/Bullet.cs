using Raylib_cs;
using System.Numerics;

namespace Rex2
{
    internal class Bullet
    {
        public bool IsOrientedRight { get; set; }
        public Vector2 Position { get; set; }
        public float RemainingTime { get; set; } = 5;
        public Rectangle Rect => new Rectangle(Position.X, Position.Y, 10, 2);
    }
}