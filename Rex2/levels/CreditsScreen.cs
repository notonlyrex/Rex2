﻿using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Rex2.levels
{
    internal class CreditsScreen : LevelBase
    {
        private List<MenuOption> menu = new List<MenuOption>();

        public CreditsScreen(int screenHeight, int screenWidth, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2) : base(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2)
        {
            menu.Add(new MenuOption { Rect = new Rectangle { x = 10, y = 10, width = 200, height = 50 }, Text = "Start game", Action = () => LevelManager.Instance.Next() });
            menu.Add(new MenuOption { Rect = new Rectangle { x = 10, y = 130, width = 200, height = 50 }, Text = "Exit", Action = () => Environment.Exit(0) });
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
            ClearBackground(Textures.Background);

            DrawAnimatedText("Code: Marcin \"Ktos\" Badurowicz", 10, 10, 10, 15, Color.RED);
            DrawRot13AnimatedText("Graphics: Stanislaw Skulimowski", 10, 10, 40, 15, Color.GREEN);
            DrawRot13AnimatedText("+ surt https://opengameart.org/content/darknes", 10, 10, 60, 15, Color.GREEN);
            DrawRot13AnimatedText("+ SpriteLib by Ari Feldman", 10, 10, 80, 15, Color.GREEN);
            DrawRot13AnimatedText("+ EEEnt-OFFICIAL", 10, 10, 100, 15, Color.GREEN);
            DrawRot13AnimatedText("+ Pixel Ship Generator: https://thydungeonsean.itch.io/pixel-ship-generator", 10, 10, 120, 15, Color.GREEN);
            DrawRot13AnimatedText("Music: Marylka", 10, 10, 140, 15, Color.BLUE);
            DrawRot13AnimatedText("Sounds: Kenney (www.kenney.nl)", 10, 10, 160, 15, Color.GOLD);

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