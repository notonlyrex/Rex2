using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

namespace Rex2
{
    public class Norma
    {
        public Match3GameManager Board { get; set; }
        public int Energy { get; set; } = 10;

        public Action<TileType, int> GiveBuff { get; set; }

        public Norma()
        {
            Board = new Match3GameManager(7, 6);
            Board.OnDestroyed += (e) =>
            {
                foreach (var item in e.Distinct())
                {
                    GiveBuff?.Invoke(item, e.Count(x => x == item));
                }
            };
        }
    }

    public enum TileType
    {
        RED,
        GREEN,
        BLUE,
        WHITE,
        YELLOW
    }

    public class Tile
    {
        public TileType type;

        public int x, y;

        public Rectangle Rect { get; set; }
        public bool Hover { get; set; }
        public bool Active { get; set; }

        public Tile(int X, int Y, TileType type)
        {
            x = X;
            y = Y;
            this.type = type;

            Rect = new Rectangle { x = x * 45, y = y * 45, width = 40, height = 40 };
        }

        public void ChangePosition(int X, int Y)
        {
            x = X;
            y = Y;
            Rect = new Rectangle { x = x * 45, y = y * 45, width = 40, height = 40 };
        }
    }

    public class Match3GameManager
    {
        public Tile?[,] Grid;

        public int sizeX;
        public int sizeY;

        private Random random = new Random();

        public Action<IList<TileType>> OnDestroyed { get; set; }

        public Match3GameManager(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            Grid = new Tile[sizeX, sizeY * 2];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY * 2; j++)
                {
                    InstantiateTile(i, j);
                }
            }

            Show();

            Check();
        }

        private void Show()
        {
            Debug.WriteLine("---");
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (Grid[i, j] != null)
                        Debug.Write(Grid[i, j].type.ToString().Substring(0, 1) + " ");
                    else
                        Debug.Write("0 ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("---");
        }

        private void SwapTiles(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2)
                return;
            MoveTile(x1, y1, x2, y2);

            List<Tile> TilesToCheck = CheckHorizontalMatches();
            TilesToCheck.AddRange(CheckVerticalMatches());

            if (TilesToCheck.Count == 0)
            {
                MoveTile(x1, y1, x2, y2);
            }
            Check();
        }

        private void Check()
        {
            List<Tile> TilesToDestroy = CheckHorizontalMatches();
            TilesToDestroy.AddRange(CheckVerticalMatches());

            TilesToDestroy = TilesToDestroy.Distinct().ToList();

            var destroyedColors = TilesToDestroy.Select(x => x.type).ToList();
            OnDestroyed?.Invoke(destroyedColors);

            bool sw = TilesToDestroy.Count == 0;

            for (int i = 0; i < TilesToDestroy.Count; i++)
            {
                if (TilesToDestroy[i] != null)
                {
                    Grid[TilesToDestroy[i].x, TilesToDestroy[i].y] = null;
                }
            }

            if (!sw)
            {
                Gravity();
            }
        }

        private void Gravity()
        {
            bool Sw = true;
            while (Sw)
            {
                Sw = false;
                for (int j = 0; j < sizeY * 2; j++)
                {
                    for (int i = 0; i < sizeX; i++)
                    {
                        if (Fall(i, j))
                        {
                            Sw = true;
                        }
                    }
                }
            }

            Check();
        }

        private bool Fall(int x, int y)
        {
            if (x < 0 || y <= 0 || x >= sizeX || y >= sizeY * 2) // <- SizeY * 2
                return false;
            if (Grid[x, y] == null)
                return false;
            if (Grid[x, y - 1] != null)
                return false;

            MoveTile(x, y, x, y - 1);
            return true;
        }

        public Vector2? FirstActive { get; set; }
        public Vector2? SecondActive { get; set; }

        public void MarkActive(int x, int y, Norma parent)
        {
            if (parent.Energy <= 0)
                return;

            if (FirstActive == null)
            {
                FirstActive = new Vector2(x, y);
                Grid[x, y].Active = true;
            }
            else if (SecondActive == null)
            {
                SecondActive = new Vector2(x, y);
                Grid[x, y].Active = true;

                var l = (FirstActive.Value - SecondActive.Value).Length();

                Grid[(int)FirstActive.Value.X, (int)FirstActive.Value.Y].Active = false;
                Grid[(int)SecondActive.Value.X, (int)SecondActive.Value.Y].Active = false;

                if (l == 1)
                {
                    SwapTiles((int)FirstActive.Value.X, (int)FirstActive.Value.Y, (int)SecondActive.Value.X, (int)SecondActive.Value.Y);
                    parent.Energy--;
                    AudioManager.Instance.Play(Sounds.Crush);
                    Check();
                }

                FirstActive = null;
                SecondActive = null;
            }
        }

        private List<Tile> CheckHorizontalMatches()
        {
            List<Tile> TilesToCheck = new List<Tile>();
            List<Tile> TilesToReturn = new List<Tile>();

            for (int j = 0; j < sizeY; j++)
            {
                if (Grid[0, j] == null)
                    InstantiateTile(0, j);

                TileType Type = Grid[0, j].type;
                for (int i = 0; i < sizeX; i++)
                {
                    if (Grid[i, j] == null)
                        InstantiateTile(i, j);

                    if (Grid[i, j].type != Type)
                    {
                        if (TilesToCheck.Count >= 3)
                        {
                            TilesToReturn.AddRange(TilesToCheck);
                        }
                        TilesToCheck.Clear();
                    }
                    Type = Grid[i, j].type;
                    TilesToCheck.Add(Grid[i, j]);
                }

                if (TilesToCheck.Count >= 3)
                {
                    TilesToReturn.AddRange(TilesToCheck);
                }
                TilesToCheck.Clear();
            }
            return TilesToReturn;
        }

        private List<Tile> CheckVerticalMatches()
        {
            List<Tile> TilesToCheck = new List<Tile>();
            List<Tile> TilesToReturn = new List<Tile>();
            TileType Type = TileType.WHITE;

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (Grid[i, j].type != Type)
                    {
                        if (TilesToCheck.Count >= 3)
                        {
                            TilesToReturn.AddRange(TilesToCheck);
                        }
                        TilesToCheck.Clear();
                    }
                    Type = Grid[i, j].type;
                    TilesToCheck.Add(Grid[i, j]);
                }

                if (TilesToCheck.Count >= 3)
                {
                    TilesToReturn.AddRange(TilesToCheck);
                }
                TilesToCheck.Clear();
            }
            return TilesToReturn;
        }

        private void MoveTile(int x1, int y1, int x2, int y2)
        {
            Tile temp = Grid[x1, y1];
            Grid[x1, y1] = Grid[x2, y2];
            Grid[x2, y2] = temp;

            if (Grid[x1, y1] != null)
                Grid[x1, y1].ChangePosition(x1, y1);
            if (Grid[x2, y2] != null)
                Grid[x2, y2].ChangePosition(x2, y2);
        }

        private void InstantiateTile(int x, int y)
        {
            Grid[x, y] = new Tile(x, y, (TileType)random.Next(0, 5));
        }
    }
}