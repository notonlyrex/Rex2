namespace Rex2
{
    internal enum TemplateType
    {
        Base,
        Platform,
        DestructiblePlatform,
        Enemy,
        BossEnemy,
        MovingEnemy,
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

    internal static class LevelParser
    {
        private static Platform TemplateToPlatform(Template pt)
        {
            if (pt.Type != TemplateType.Platform && pt.Type != TemplateType.Base && pt.Type != TemplateType.DestructiblePlatform)
                throw new ArgumentException("pt");

            Platform res = new Platform() { Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = pt.Width, height = (pt.Type == TemplateType.Base) ? 200 : 10 } };
            res.Blocking = true;

            if (pt.Type == TemplateType.DestructiblePlatform)
            {
                res.Durability = 2;
                res.Touched = false;
                res.Type = TemplateType.DestructiblePlatform;
            }

            if (pt.Type == TemplateType.Base)
                res.Type = TemplateType.Base;
            else if (pt.Type == TemplateType.Platform)
                res.Type = TemplateType.Platform;

            return res;
        }

        private static Enemy TemplateToEnemy(Template pt)
        {
            if (pt.Type != TemplateType.Enemy && pt.Type != TemplateType.BossEnemy && pt.Type != TemplateType.MovingEnemy)
                throw new ArgumentException("pt");

            Enemy res = new Enemy()
            {
                Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = 33, height = 22 },
                HP = 1
            };

            if (pt.Type == TemplateType.MovingEnemy)
            {
                res.Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = 32, height = 20 };
                res.HP = 1;
                res.Origin = new System.Numerics.Vector2(pt.X, pt.Y);
                res.IsMoving = true;
            }

            if (pt.Type == TemplateType.BossEnemy)
            {
                res.Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = 50, height = 55 };
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
                            p.Type = TemplateType.DestructiblePlatform;
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
                        else if (line[j] == 'f')
                        {
                            p = new Template();
                            p.Type = TemplateType.MovingEnemy;
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

            result.StartingY = (lines.Count - 1) * 20;

            return result;
        }

        private static void ConvertTemplate(LevelDefinition result, Template p)
        {
            switch (p.Type)
            {
                case TemplateType.Base:
                case TemplateType.Platform:
                case TemplateType.DestructiblePlatform:
                    result.Platforms.Add(TemplateToPlatform(p));
                    break;

                case TemplateType.Enemy:
                case TemplateType.BossEnemy:
                case TemplateType.MovingEnemy:
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
                Rect = new Raylib_cs.Rectangle { x = pt.X, y = pt.Y, width = 18, height = 18 },
            };
        }
    }
}