using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Rex2.levels
{
    internal class CreditsScreen : LevelBase
    {
        private List<MenuOption> menu = new List<MenuOption>();

        public CreditsScreen(int screenHeight, int screenWidth, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2) : base(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2)
        {
            menu.Add(new MenuOption { Rect = new Rectangle { x = 10, y = 10, width = 200, height = 50 }, Text = "Start game", Action = () => LevelManager.Instance.Next() });
            menu.Add(new MenuOption { Rect = new Rectangle { x = 10, y = 130, width = 200, height = 50 }, Text = "Exit", Action = () => CloseWindow() });
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                LevelManager.Instance.Next();
            }

            Draw();
        }

        private void Draw()
        {
            BeginTextureMode(screenPlayer1);
            ClearBackground(Color.LIGHTGRAY);

            DrawAnimatedText("Code: Marcin \"Ktos\" Badurowicz", 10, 10, 10, 15, Color.RED);
            DrawRot13AnimatedText("Graphics: Stanislaw Skulimowski", 10, 10, 40, 15, Color.GREEN);
            DrawRot13AnimatedText("Music: Maryla Kapustka", 10, 10, 60, 15, Color.BLUE);
            DrawRot13AnimatedText("Sounds: Kenney (www.kenney.nl)", 10, 10, 80, 15, Color.GOLD);

            /*
             * surt https://opengameart.org/content/darknes
             * SpriteLib Original copyright © 1996-2017 by Ari Feldman
             * © 2017 - 2022 EEEnt-OFFICIAL
             */

            EndTextureMode();

            BeginTextureMode(screenPlayer2);
            ClearBackground(Color.LIGHTGRAY);

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