namespace Rex2
{
    internal class LevelManager
    {
        private List<LevelBase> Levels { get; set; }

        private LevelBase welcome;
        private LevelBase menu;
        private LevelBase lose;
        private LevelBase win;

        public LevelManager(LevelBase welcome, LevelBase menu, LevelBase lose, LevelBase win)
        {
            this.welcome = welcome;
            this.lose = lose;
            this.win = win;
            this.menu = menu;

            Levels = new List<LevelBase>();
            Current = welcome;
        }

        public LevelBase Current { get; private set; }

        public void Lose()
        {
            Current = lose;
        }

        public void Win()
        {
            Current = win;
        }

        public void Menu()
        {
            Current = menu;
        }

        public void Welcome()
        {
            Current = welcome;
        }

        public void Add(LevelBase level)
        {
            Levels.Add(level);
        }
    }
}