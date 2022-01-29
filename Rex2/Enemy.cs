using Raylib_cs;

namespace Rex2
{
    internal class Enemy
    {
        public int HP { get; set; }
        public Rectangle Rect { get; set; }
        public bool IsBoss { get; set; } = false;
    }
}