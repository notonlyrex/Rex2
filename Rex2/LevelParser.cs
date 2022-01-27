namespace Rex2
{
    internal class PlatformTemplate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Base { get; set; } = false;
    }

    internal class LevelParser
    {
        private static Platform TemplateToPlatform(PlatformTemplate pt)
        {
            Platform res = new Platform() { Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = pt.Width, height = (pt.Base) ? 200 : 10 } };
            res.Blocking = true;

            if (pt.Base)
                res.Color = Raylib_cs.Color.DARKGRAY;
            else
                res.Color = Raylib_cs.Color.GRAY;

            return res;
        }

        public static List<Platform> Parse(string fileName)
        {
            var result = new List<Platform>();

            var lines = File.ReadAllLines(fileName).ToList();
            lines = lines.Where(x => !x.StartsWith("#")).ToList();

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                PlatformTemplate? p = null;

                for (int j = 0; j < line.Length; j++)
                {
                    if (p == null)
                    {
                        if (line[j] == 'B')
                        {
                            p = new PlatformTemplate();
                            p.Base = true;
                            p.X = j * 20;
                            p.Y = i * 20;
                            p.Width += 20;
                        }

                        if (line[j] == 'X')
                        {
                            p = new PlatformTemplate();
                            p.Base = false;
                            p.X = j * 20;
                            p.Y = i * 20;
                            p.Width += 20;
                        }
                    }
                    else
                    {
                        if (line[j] == 'B')
                        {
                            p.Width += 20;
                        }

                        if (line[j] == 'X')
                        {
                            p.Width += 20;
                        }
                    }

                    if (line[j] == ' ')
                    {
                        if (p != null)
                        {
                            result.Add(TemplateToPlatform(p));
                            p = null;
                        }
                    }
                }

                if (p != null)
                {
                    result.Add(TemplateToPlatform(p));
                    p = null;
                }
            }

            return result;
        }
    }
}