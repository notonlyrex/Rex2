using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Raylib;

namespace Rex2
{
    internal class TestLevel : LevelBase
    {
        private void UpdateCameraCenter(ref Camera2D camera, ref Player player, Platform[] envItems, float delta, int width, int height)
        {
            camera.offset = new Vector2(width / 2, height / 2);
            camera.target = player.position;
        }

        private void UpdatePlayer(ref Player player, Platform[] envItems, float delta)
        {
            if (IsKeyDown(KEY_LEFT))
                player.position.X -= PLAYER_HOR_SPD * delta;

            if (IsKeyDown(KEY_RIGHT))
                player.position.X += PLAYER_HOR_SPD * delta;

            if (IsKeyDown(KEY_SPACE) && player.canJump)
            {
                player.speed = -PLAYER_JUMP_SPD;
                player.canJump = false;
            }

            int hitObstacle = 0;
            for (int i = 0; i < envItems.Length; i++)
            {
                Platform ei = envItems[i];
                Vector2 p = player.position;
                if (ei.blocking != 0 &&
                    ei.rect.x <= p.X &&
                    ei.rect.x + ei.rect.width >= p.X &&
                    ei.rect.y >= p.Y &&
                    ei.rect.y < p.Y + player.speed * delta)
                {
                    hitObstacle = 1;
                    player.speed = 0.0f;
                    player.position.Y = ei.rect.y;
                }
            }

            if (hitObstacle == 0)
            {
                player.position.Y += player.speed * delta;
                player.speed += G * delta;
                player.canJump = false;
            }
            else
                player.canJump = true;
        }

        private const int G = 400;

        private const float PLAYER_JUMP_SPD = 350.0f;
        private const float PLAYER_HOR_SPD = 200.0f;
        private Vector2 origin;
        private Platform[] envItems;
        private Camera2D camera;
        private Player player;
        private int elapsedTime;
        private int levelTime = 99;
        private DialogueManager dialogueManager;

        public TestLevel(int screenHeight, int screenWidth, Vector2 origin, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2) : base(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2)
        {
            player = new Player();
            player.position = new Vector2(400, 280);
            player.speed = 0;
            player.canJump = false;
            this.origin = origin;

            envItems = new Platform[] {
                new Platform(new Rectangle(0, 0, 1000, 400), 0, LIGHTGRAY),
                new Platform(new Rectangle(0, 400, 1000, 200), 1, GRAY),
                new Platform(new Rectangle(300, 200, 400, 10), 1, GRAY),
                new Platform(new Rectangle(250, 300, 100, 10), 1, GRAY),
                new Platform(new Rectangle(650, 300, 100, 10), 1, GRAY)
            };

            camera = new Camera2D();
            camera.target = player.position;
            camera.offset = new Vector2(screenWidth / 2, screenHeight / 2);
            camera.rotation = 0.0f;
            camera.zoom = 1.0f;

            framesCounter = 0;

            elapsedTime = 0;

            dialogueManager = new DialogueManager();
            timerCallback = UpdateTime;
            timer = new Timer(timerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void UpdateTime(object? state)
        {
            elapsedTime++;
            dialogueManager.UpdateDialogueOnTime(elapsedTime);
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
            UpdatePlayer(ref player, envItems, deltaTime);
            UpdateCameraCenter(ref camera, ref player, envItems, deltaTime, screenWidth, screenHeight);
        }

        private void Draw()
        {
            BeginTextureMode(screenPlayer1);
            ClearBackground(LIGHTGRAY);

            BeginMode2D(camera);

            RenderPlatforms();
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

            Vector2 pos = new Vector2(10.0f, 10.0f);
            Vector2 pos2 = pos + origin;
            Vector2 size = new Vector2(10.0f, 10.0f);

            DrawText($"{virtualMouse.X}, {virtualMouse.Y}", 0, 0, 10, GOLD);

            Rectangle emojiRect = new Rectangle(pos.X, pos.Y, size.X, size.Y);

            if (CheckCollisionPointRec(virtualMouse, emojiRect))
            {
                DrawRectangle((int)emojiRect.x, (int)emojiRect.y, (int)emojiRect.width, (int)emojiRect.height, RED);
            }
            else
            {
                DrawRectangle((int)emojiRect.x, (int)emojiRect.y, (int)emojiRect.width, (int)emojiRect.height, BLUE);
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
            Rectangle playerRect = new Rectangle(player.position.X - 20, player.position.Y - 40, 40, 40);
            DrawRectangleRec(playerRect, RED);
        }

        private void RenderPlatforms()
        {
            for (int i = 0; i < envItems.Length; i++)
                DrawRectangleRec(envItems[i].rect, envItems[i].color);
        }

        internal override void DrawMain()
        {
            base.DrawMain();

            DrawDialogueText(dialogueManager.DisplayedDialogue.Text, dialogueManager.DisplayedDialogue.IsNorma ? BLUE : RED);
            DrawRemainingTime(levelTime - elapsedTime);
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}