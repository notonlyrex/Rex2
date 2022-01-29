using Raylib_cs;
using System.Numerics;

namespace Rex2
{
    internal class Enemy
    {
        public int HP { get; set; }
        public Rectangle Rect { get; set; }
        public bool IsBoss { get; set; } = false;
        public Vector2 Origin { get; set; }
        public bool IsMoving { get; set; } = false;
    }
}