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
            const int screenWidth = 310;
            const int screenHeight = 310;

            const int gameWidth = 640;
            const int gameHeight = 360;
            const int scale = 2;

            InitWindow(gameWidth * scale, gameHeight * scale, "Rex2");
            SetTargetFPS(120);
            RenderTexture2D screenPlayer1 = LoadRenderTexture(screenWidth, screenHeight);
            RenderTexture2D screenPlayer2 = LoadRenderTexture(screenWidth, screenHeight);
            Texture2D baseTexture = LoadTexture("assets/base.png");
            Texture2D chevron = LoadTexture("assets/chevron.png");

            Rectangle sourceRec = new Rectangle(0.0f, 0.0f, screenPlayer1.texture.width, -screenPlayer1.texture.height);
            Rectangle destRec = new Rectangle(6 * scale, 27 * scale, screenWidth * scale, screenHeight * scale);
            Rectangle destRec2 = new Rectangle(324 * scale, 27 * scale, screenWidth * scale, screenHeight * scale);
            Rectangle chevroSrc = new Rectangle(0, 0, chevron.width, chevron.height);
            Rectangle chevroDest = new Rectangle(468 * scale, 330 * scale, 22 * scale, 12 * scale);
            TestLevel test = new TestLevel(screenHeight, screenWidth, new Vector2(324, 27), ref screenPlayer1, ref screenPlayer2);

            LevelManager levelManager = new LevelManager(test, null!, null!, null!);
            levelManager.Welcome();

            while (!WindowShouldClose())
            {
                float deltaTime = GetFrameTime();

                levelManager.Current.Update(deltaTime);

                BeginDrawing();
                ClearBackground(BLACK);

                DrawTextureEx(baseTexture, Vector2.Zero, 0, 2, WHITE);
                DrawTexturePro(screenPlayer1.texture, sourceRec, destRec, new Vector2(0, 0), 0.0f, WHITE);
                DrawTexturePro(screenPlayer2.texture, sourceRec, destRec2, new Vector2(0, 0), 0.0f, WHITE);

                levelManager.Current.DrawMain();

                DrawTexturePro(chevron, chevroSrc, chevroDest, new Vector2(0, 0), 0, WHITE);

                EndDrawing();
            }

            UnloadRenderTexture(screenPlayer1);
            UnloadRenderTexture(screenPlayer2);
            UnloadTexture(baseTexture);
            levelManager.Current.Unload();

            CloseWindow();
        }
    }
}