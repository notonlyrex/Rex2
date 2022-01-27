using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Raylib;

namespace Rex2
{
    internal class TestLevel : LevelBase
    {
        private void UpdateCameraCenter(ref Camera2D camera, ref Player player, IEnumerable<Platform> envItems, float delta, int width, int height)
        {
            camera.offset = new Vector2(width / 2, height / 2);
            camera.target = player.Position;
        }

        private const int G = 400;

        private float PLAYER_JUMP_SPD = 350.0f;
        private float PLAYER_HOR_SPD = 200.0f;

        private LevelDefinition level;
        private Match3 norma;

        private Camera2D camera;
        private Player player;

        private DialogueManager dialogueManager;

        public TestLevel(int screenHeight, int screenWidth, Vector2 origin, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2) : base(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2)
        {
            player = new Player();
            player.Position = new Vector2(400, 280);
            player.Speed = 0;
            player.CanJump = false;

            level = LevelParser.Parse("levels/testlevel.txt");

            camera = new Camera2D();
            camera.target = player.Position;
            camera.offset = new Vector2(screenWidth / 2, screenHeight / 2);
            camera.rotation = 0.0f;
            camera.zoom = 1.0f;

            framesCounter = 0;

            ElapsedTime = 0;

            dialogueManager = new DialogueManager();

            norma = new Match3();

            timerCallback = UpdateTime;
            timer = new Timer(timerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void UpdateTime(object? state)
        {
            ElapsedTime++;
            dialogueManager.UpdateDialogue(this, player);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdatePlayer1(deltaTime);
            UpdatePlayer2(deltaTime);

            Draw();
        }

        private void UpdatePlayer2(float deltaTime)
        {
        }

        private void UpdatePlayer1(float deltaTime)
        {
            if (IsKeyDown(KEY_LEFT) || IsKeyDown(KEY_A))
                player.Position = new Vector2 { X = player.Position.X - PLAYER_HOR_SPD * deltaTime, Y = player.Position.Y };

            if (IsKeyDown(KEY_RIGHT) || IsKeyDown(KEY_D))
                player.Position = new Vector2 { X = player.Position.X + PLAYER_HOR_SPD * deltaTime, Y = player.Position.Y };

            if ((IsKeyDown(KEY_SPACE) || IsKeyDown(KEY_W)) && player.CanJump)
            {
                player.Speed = -PLAYER_JUMP_SPD;

                player.CanJump = false;
            }

            if (IsKeyDown(KEY_F1))
            {
                EnableHighJump();
            }

            int hitObstacle = 0;
            for (int i = 0; i < level.Platforms.Count; i++)
            {
                Platform ei = level.Platforms[i];
                Vector2 p = player.Position;
                if (ei.Blocking &&
                    ei.Rect.x <= p.X &&
                    ei.Rect.x + ei.Rect.width >= p.X &&
                    ei.Rect.y >= p.Y &&
                    ei.Rect.y < p.Y + player.Speed * deltaTime)
                {
                    hitObstacle = 1;
                    player.Speed = 0.0f;
                    player.Position = new Vector2 { X = player.Position.X, Y = ei.Rect.y };
                }
            }

            if (hitObstacle == 0)
            {
                player.Position = new Vector2 { X = player.Position.X, Y = player.Position.Y + player.Speed * deltaTime };
                player.Speed += G * deltaTime;
                player.CanJump = false;
            }
            else
                player.CanJump = true;
            UpdateCameraCenter(ref camera, ref player, level.Platforms, deltaTime, screenWidth, screenHeight);
        }

        private void EnableHighJump()
        {
            PLAYER_JUMP_SPD = PLAYER_JUMP_SPD + 10;
            dialogueManager.UpdateDialogueOnSituation(Situation.HighJump);
        }

        private void Draw()
        {
            BeginTextureMode(screenPlayer1);
            ClearBackground(LIGHTGRAY);

            BeginMode2D(camera);

            RenderPlatforms();
            RenderEnemies();
            RenderPlayer();
            EndMode2D();

            RenderHelp();
            DrawText($"FPS: {GetFPS()}", 0, 0, 10, GOLD);

            EndTextureMode();

            BeginTextureMode(screenPlayer2);
            ClearBackground(WHITE);

            RenderPlayer2();

            EndTextureMode();
        }

        private void RenderPlayer2()
        {
            DrawRot13AnimatedText("Wake up, Rex!", 10, 0, 220, 14, MAGENTA);
            //DrawCenteredText("Zażółć gęślą jaźń", 50, 18, GREEN);
            DrawRectangledTextEx(new Rectangle(20, 20, 120, 120), "This is a longer text with wrapping.", 14, DARKGREEN, GRAY);

            //DrawRectangle(0, 0, 319, 10, RED);

            Vector2 virtualMouse = GetVirtualMouse();

            DrawText($"{virtualMouse.X}, {virtualMouse.Y}", 0, 0, 10, GOLD);

            //Rectangle emojiRect = new Rectangle(pos.X, pos.Y, size.X, size.Y);

            for (int i = 0; i < norma.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < norma.Grid.GetLength(1); j++)
                {
                    var g = norma.Grid[i, j];

                    g.Hover = CheckCollisionPointRec(virtualMouse, g.Rect);

                    Color c = RED;

                    if (g.Active)
                        c = GREEN;

                    if (g.Hover)
                        c = BLUE;

                    DrawRectangle((int)g.Rect.x, (int)g.Rect.y, (int)g.Rect.width, (int)g.Rect.height, c);
                }
            }
        }

        private void RenderHelp()
        {
            DrawText("Controls:", 20, 20, 10, BLACK);
            DrawText("- Right/Left to move", 40, 40, 10, DARKGRAY);
            DrawText("- Space to jump", 40, 60, 10, DARKGRAY);
        }

        private void RenderPlayer()
        {
            Rectangle playerRect = new Rectangle(player.Position.X - 20, player.Position.Y - 40, 40, 40);
            DrawRectangleRec(playerRect, RED);
        }

        private void RenderPlatforms()
        {
            foreach (var item in level.Platforms)
            {
                DrawRectangleRec(item.Rect, item.Color);
            }
        }

        private void RenderEnemies()
        {
            foreach (var item in level.Enemies)
            {
                DrawRectangleRec(item.Rect, GOLD);
            }
        }

        internal override void DrawMain()
        {
            base.DrawMain();

            DrawDialogueText(dialogueManager.DisplayedDialogue.Text, dialogueManager.DisplayedDialogue.IsNorma ? BLUE : RED);
            DrawRemainingTime(LevelTime - ElapsedTime);
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}