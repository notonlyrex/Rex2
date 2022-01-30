using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Rex2.levels
{
    internal class MenuOption
    {
        public Rectangle Rect { get; set; }
        public string Text { get; set; }
        public Action Action { get; set; }
    }

    internal class MenuScreen : LevelBase
    {
        private List<Dialogue> intro;
        private int introIndex = 0;

        private List<MenuOption> menu = new List<MenuOption>();

        public MenuScreen(int screenHeight, int screenWidth, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2) : base(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2)
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;

            var dm = new DialogueManager();
            intro = dm.Introduction();

            menu.Add(new MenuOption { Rect = new Rectangle { x = 10, y = 10, width = 200, height = 50 }, Text = "Start game", Action = () => LevelManager.Instance.Next() });
            menu.Add(new MenuOption { Rect = new Rectangle { x = 10, y = 70, width = 200, height = 50 }, Text = "Credits", Action = () => LevelManager.Instance.Credits() });
            menu.Add(new MenuOption { Rect = new Rectangle { x = 10, y = 130, width = 200, height = 50 }, Text = "Exit", Action = () => CloseWindow() });
        }

        public override void Start()
        {
            base.Start();
            timer.Start();
            AudioManager.Instance.PlayMusic(false);
        }

        public override void Stop()
        {
            base.Stop();
            timer.Stop();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (introIndex < intro.Count - 1)
            {
                introIndex++;
                AudioManager.Instance.Play(Sounds.Dialogue);
            }
            else
            {
                timer.Stop();
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                LevelManager.Instance.Next();
            }

            Draw();
        }

        private void Draw()
        {
            BeginTextureMode(screenPlayer1);
            DrawTexture(Textures.BackgroundScreen1, 0, 0, Color.WHITE);

            for (int i = 0; i < introIndex; i++)
            {
                var t = intro[i];
                DrawText(t.Text, 10, 15 * (i + 1), 10, t.IsNorma ? Color.BLUE : Color.RED);
            }

            EndTextureMode();

            BeginTextureMode(screenPlayer2);
            ClearBackground(Textures.Background);

            foreach (var item in menu)
            {
                Color c = Color.GOLD;

                if (CheckCollisionPointRec(GetVirtualMouse(), item.Rect))
                {
                    c = Color.RED;
                    if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                    {
                        item.Action();
                        return;
                    }
                }

                DrawRectangledTextEx(item.Rect, item.Text, 30, c, c);
            }

            EndTextureMode();
        }
    }
}