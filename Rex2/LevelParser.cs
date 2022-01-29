namespace Rex2
{
    internal enum TemplateType
    {
        Base,
        Platform,
        Enemy,
        Ammo
    }

    internal class Template
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public TemplateType Type { get; set; }
    }

    internal class LevelParser
    {
        private static Platform TemplateToPlatform(Template pt)
        {
            if (pt.Type != TemplateType.Platform && pt.Type != TemplateType.Base)
                throw new ArgumentException("pt");

            Platform res = new Platform() { Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = pt.Width, height = (pt.Type == TemplateType.Base) ? 200 : 10 } };
            res.Blocking = true;

            if (pt.Type == TemplateType.Base)
                res.Color = Raylib_cs.Color.DARKGRAY;
            else if (pt.Type == TemplateType.Platform)
                res.Color = Raylib_cs.Color.GRAY;

            return res;
        }

        private static Enemy TemplateToEnemy(Template pt)
        {
            if (pt.Type != TemplateType.Enemy)
                throw new ArgumentException("pt");

            Enemy res = new Enemy()
            {
                Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = 10, height = 10 }
            };

            res.HP = 1;

            return res;
        }

        public static LevelDefinition Parse(string fileName)
        {
            var result = new LevelDefinition();

            var lines = File.ReadAllLines(fileName).ToList();
            lines = lines.Where(x => !x.StartsWith("#")).ToList();

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];
                Template? p = null;

                for (int j = 0; j < line.Length; j++)
                {
                    if (p == null)
                    {
                        if (line[j] == 'B')
                        {
                            p = new Template();
                            p.Type = TemplateType.Base;
                            p.X = j * 20;
                            p.Y = i * 20;
                            p.Width += 20;
                        }
                        else if (line[j] == 'X')
                        {
                            p = new Template();
                            p.Type = TemplateType.Platform;
                            p.X = j * 20;
                            p.Y = i * 20;
                            p.Width += 20;
                        }
                        else if (line[j] == 'e')
                        {
                            p = new Template();
                            p.Type = TemplateType.Enemy;
                            p.X = j * 20;
                            p.Y = i * 20;
                        }
                    }
                    else
                    {
                        if (line[j] == 'B')
                        {
                            p.Width += 20;
                        }
                        else if (line[j] == 'X')
                        {
                            p.Width += 20;
                        }
                    }

                    if (line[j] == ' ' && p != null)
                    {
                        ConvertTemplate(result, p);
                        p = null;
                    }
                }

                if (p != null)
                {
                    ConvertTemplate(result, p);
                    p = null;
                }
            }

            return result;
        }

        private static void ConvertTemplate(LevelDefinition result, Template p)
        {
            switch (p.Type)
            {
                case TemplateType.Base:
                case TemplateType.Platform:
                    result.Platforms.Add(TemplateToPlatform(p));
                    break;

                case TemplateType.Enemy:
                    result.Enemies.Add(TemplateToEnemy(p));
                    break;
            }
        }
    }
}