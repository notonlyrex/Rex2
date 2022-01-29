namespace Rex2
{
    internal class LevelManager
    {
        public static LevelManager Instance { get; private set; }

        private List<LevelBase> Levels { get; set; }

        private int currentIndex;
        private LevelBase welcome;
        private LevelBase menu;
        private LevelBase lose;
        private LevelBase win;
        private LevelBase credits;

        public LevelManager(LevelBase welcome, LevelBase menu, LevelBase lose, LevelBase win, LevelBase credits)
        {
            this.welcome = welcome;
            this.lose = lose;
            this.win = win;
            this.menu = menu;
            this.credits = credits;

            Levels = new List<LevelBase>();
            currentIndex = -1;
            Current = welcome;
            Instance = this;
        }

        public LevelBase Current { get; private set; }

        public void Next()
        {
            Current.Stop();
            if (currentIndex == -1 || currentIndex + 1 < Levels.Count)
            {
                currentIndex++;
                Current = Levels[currentIndex];
            }
            else
            {
                Win();
            }

            Current.Start();
        }

        public void Lose()
        {
            Current.Stop();
            currentIndex = -1;
            Current = lose;
            Current.Start();
        }

        public void Win()
        {
            Current.Stop();
            currentIndex = -1;
            Current = win;
            Current.Start();
        }

        public void Menu()
        {
            Current.Stop();
            currentIndex = -1;
            Current = menu;
            Current.Start();
        }

        public void Credits()
        {
            Current.Stop();
            currentIndex = -1;
            Current = credits;
            Current.Start();
        }

        public void Welcome()
        {
            Current.Stop();
            currentIndex = -1;
            Current = welcome;
            Current.Start();
        }

        public void Add(LevelBase level)
        {
            Levels.Add(level);
        }
    }
}