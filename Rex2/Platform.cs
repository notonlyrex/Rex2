using Raylib_cs;

namespace Rex2
{
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
}