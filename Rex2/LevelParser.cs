namespace Rex2
{
    internal enum TemplateType
    {
        Base,
        Platform,
        DestroyablePlatform,
        Enemy,
        BossEnemy,
        Powerup
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
            if (pt.Type != TemplateType.Platform && pt.Type != TemplateType.Base && pt.Type != TemplateType.DestroyablePlatform)
                throw new ArgumentException("pt");

            Platform res = new Platform() { Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = pt.Width, height = (pt.Type == TemplateType.Base) ? 200 : 10 } };
            res.Blocking = true;

            if (pt.Type == TemplateType.DestroyablePlatform)
            {
                res.Durability = 2;
                res.Touched = false;
                res.Color = Raylib_cs.Color.BEIGE;
            }

            if (pt.Type == TemplateType.Base)
                res.Color = Raylib_cs.Color.DARKGRAY;
            else if (pt.Type == TemplateType.Platform)
                res.Color = Raylib_cs.Color.GRAY;

            return res;
        }

        private static Enemy TemplateToEnemy(Template pt)
        {
            if (pt.Type != TemplateType.Enemy && pt.Type != TemplateType.BossEnemy)
                throw new ArgumentException("pt");

            Enemy res = new Enemy()
            {
                Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = 10, height = 10 },
                HP = 1
            };

            if (pt.Type == TemplateType.BossEnemy)
            {
                res.HP = 5;
                res.IsBoss = true;
            }

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
                        else if (line[j] == 'Z')
                        {
                            p = new Template();
                            p.Type = TemplateType.DestroyablePlatform;
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
                        else if (line[j] == 'E')
                        {
                            p = new Template();
                            p.Type = TemplateType.BossEnemy;
                            p.X = j * 20;
                            p.Y = i * 20;
                        }
                        else if (line[j] == 'p')
                        {
                            p = new Template();
                            p.Type = TemplateType.Powerup;
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
                        else if (line[j] == 'Z')
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
                case TemplateType.DestroyablePlatform:
                    result.Platforms.Add(TemplateToPlatform(p));
                    break;

                case TemplateType.Enemy:
                case TemplateType.BossEnemy:
                    result.Enemies.Add(TemplateToEnemy(p));
                    break;

                case TemplateType.Powerup:
                    result.Powerups.Add(TemplateToPowerup(p));
                    break;
            }
        }

        private static Powerup TemplateToPowerup(Template pt)
        {
            if (pt.Type != TemplateType.Powerup)
                throw new ArgumentException("pt");

            return new Powerup()
            {
                Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = 10, height = 10 },
            };
        }
    }
}