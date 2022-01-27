using Raylib_cs;
using System.Numerics;

namespace Rex2
{
    internal enum PieceType
    {
        HP
    }

    internal class Piece
    {
        public PieceType Type { get; set; }
        public Rectangle Rect { get; set; }
        public bool Active { get; set; }
        public bool Hover { get; set; }
    }

    internal class Match3
    {
        private Vector2 pos;
        private Vector2 size;

        public Piece[,] Grid { get; private set; }

        public Match3()
        {
            pos = new Vector2(10.0f, 10.0f);
            size = new Vector2(41.0f, 41.0f);

            Grid = new Piece[6, 6];

            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j] = new Piece
                    {
                        Type = PieceType.HP,
                        Rect = new Rectangle(pos.X + j * 50, pos.Y + i * 50, size.X, size.Y)
                    };
                }
            }
        }
    }
}