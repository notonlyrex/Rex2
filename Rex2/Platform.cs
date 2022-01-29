using Raylib_cs;

namespace Rex2
{
    internal class Platform
    {
        public Rectangle Rect { get; set; }
        public bool Blocking { get; set; }
        public Color Color { get; set; }
        public float Durability { get; set; }
        public bool Touched { get; internal set; }

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