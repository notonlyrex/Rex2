using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace Rex2
{
    internal static class Rex2
    {
        public static void Main()
        {
            const int screenWidth = 320;
            const int screenHeight = 320;

            const int gameWidth = screenWidth * 4;
            const int gameHeight = screenHeight * 2;

            InitWindow(gameWidth, gameHeight, "Rex2");
            SetTargetFPS(60);
            RenderTexture2D screenPlayer1 = LoadRenderTexture(screenWidth, screenHeight);
            RenderTexture2D screenPlayer2 = LoadRenderTexture(screenWidth, screenHeight);
            float scale = gameHeight / screenHeight;
            Rectangle sourceRec = new Rectangle(0.0f, 0.0f, screenPlayer1.texture.width, -screenPlayer1.texture.height);
            Rectangle destRec = new Rectangle(0, 0, screenWidth * scale, screenHeight * scale);
            Rectangle destRec2 = new Rectangle(screenWidth * scale, 0, screenWidth * scale, screenHeight * scale);

            TestLevel test = new TestLevel(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2);
            LevelManager levelManager = new LevelManager(test, null!, null!, null!);
            levelManager.Welcome();

            while (!WindowShouldClose())
            {
                float deltaTime = GetFrameTime();

                levelManager.Current.Update(deltaTime);

                BeginDrawing();
                ClearBackground(BLACK);
                DrawTexturePro(screenPlayer1.texture, sourceRec, destRec, new Vector2(0, 0), 0.0f, WHITE);
                DrawTexturePro(screenPlayer2.texture, sourceRec, destRec2, new Vector2(0, 0), 0.0f, WHITE);
                EndDrawing();
            }

            UnloadRenderTexture(screenPlayer1);
            UnloadRenderTexture(screenPlayer2);
            levelManager.Current.Unload();

            CloseWindow();
        }
    }
}