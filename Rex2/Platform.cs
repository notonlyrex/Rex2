using Raylib_cs;

namespace Rex2
{
    internal class Platform
    {
        public Rectangle Rect { get; set; }
        public bool Blocking { get; set; }
        public TemplateType Type { get; set; }
        public float Durability { get; set; }
        public bool Touched { get; internal set; }

        public Platform()
        {
        }

        public Platform(Rectangle rect, bool blocking, TemplateType type)
        {
            this.Rect = rect;
            this.Blocking = blocking;
            this.Type = type;
        }
    }
}