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
        private Norma norma;

        private Camera2D camera;
        private Player player;

        private DialogueManager dialogueManager;

        private Texture2D jewel;

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

            norma = new Norma();
            norma.GiveBuff += BuffPlayer;

            timer = new System.Timers.Timer();
            timer.Elapsed += UpdateTime;
            timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            timer.AutoReset = true;

            jewel = LoadTexture("assets/jewel.png");
        }

        private void BuffPlayer(TileType t, int count)
        {
            int multiply = 0;
            if (count == 3)
            {
                multiply = 1;
            }
            else if (count == 4)
            {
                multiply = 2;
            }
            else if (count == 5)
            {
                multiply = 3;
            }
            else if (count >= 6)
            {
                multiply = 4;
            }

            if (count >= 5)
            {
                dialogueManager.UpdateDialogueOnSituation(Situation.MegaBuff);
                player.HP += multiply;
                player.Shield += multiply;
                player.Ammo += multiply;
                level.LevelTime += multiply;
                return;
            }

            switch (t)
            {
                case TileType.RED:
                    player.HP += multiply;
                    break;

                case TileType.GREEN:
                    player.Shield += multiply;
                    break;

                case TileType.BLUE:
                    player.Ammo += multiply;
                    break;

                case TileType.YELLOW:
                    level.LevelTime += multiply;
                    break;

                case TileType.WHITE:
                    if (count >= 4) EnableHighJump();
                    break;
            }
        }

        private void UpdateTime(object? sender, System.Timers.ElapsedEventArgs e)
        {
            ElapsedTime++;
            dialogueManager.UpdateDialogue(this, level, player, norma);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdatePlayer1(deltaTime);
            UpdatePlayer2(deltaTime);

            CheckWinLose();

            Draw();
        }

        private void CheckWinLose()
        {
            if (ElapsedTime > level.LevelTime || player.HP == 0)
            {
                LevelManager.Instance.Lose();
            }

            if (level.Enemies.Count(x => x.IsBoss) == 0)
            {
                LevelManager.Instance.Next();
            }
        }

        private void UpdatePlayer2(float deltaTime)
        {
        }

        private void UpdatePlayer1(float deltaTime)
        {
            UpdatePlayer(deltaTime);

            UpdateBullets(deltaTime);
            UpdateEnemies(deltaTime);

            if (IsKeyPressed(KEY_F1))
            {
                EnableHighJump();
            }

            if (IsKeyPressed(KEY_F2))
            {
                AudioManager.Instance.Play(Sounds.ImportantDialogue);
                player.HP++;
            }

            if (IsKeyPressed(KEY_F3))
            {
                AudioManager.Instance.Play(Sounds.ImportantDialogue);
                player.Ammo++;
            }

            if (IsKeyPressed(KEY_F4))
            {
                AudioManager.Instance.Play(Sounds.ImportantDialogue);
                player.Shield++;
            }

            if (IsKeyPressed(KEY_F5))
            {
                AudioManager.Instance.Play(Sounds.ImportantDialogue);
                norma.Energy++;
            }

            if (IsKeyPressed(KEY_F6))
            {
                BuffPlayer(TileType.GREEN, 6);
            }

            UpdatePlayerOnPlatforms(deltaTime);
            UpdateCameraCenter(ref camera, ref player, level.Platforms, deltaTime, screenWidth, screenHeight);
        }

        private void UpdatePlayerOnPlatforms(float deltaTime)
        {
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
        }

        private void UpdatePlayer(float deltaTime)
        {
            if (IsKeyDown(KEY_LEFT) || IsKeyDown(KEY_A))
                player.Position = new Vector2 { X = player.Position.X - PLAYER_HOR_SPD * deltaTime, Y = player.Position.Y };

            if (IsKeyDown(KEY_RIGHT) || IsKeyDown(KEY_D))
                player.Position = new Vector2 { X = player.Position.X + PLAYER_HOR_SPD * deltaTime, Y = player.Position.Y };

            if ((IsKeyDown(KEY_SPACE) || IsKeyDown(KEY_W)) && player.CanJump)
            {
                AudioManager.Instance.Play(Sounds.Jump);

                player.Speed = -PLAYER_JUMP_SPD;

                player.CanJump = false;
            }

            if (IsKeyPressed(KEY_LEFT_CONTROL) || IsKeyPressed(KEY_RIGHT_CONTROL))
            {
                if (player.Ammo > 0)
                {
                    AudioManager.Instance.Play(Sounds.Bullet);
                    level.Bullets.Add(new Bullet { IsOrientedRight = true, RemainingTime = 5, Position = new Vector2(player.Position.X + 20, player.Position.Y - 10) });
                    player.Ammo--;
                }
            }

            // aktualizacja kolizji gracza z przeciwnikami
            foreach (var item in level.Enemies)
            {
                if (CheckCollisionRecs(item.Rect, player.Rect))
                {
                    item.HP = 0;
                    if (player.Shield > 0)
                    {
                        player.Shield--;
                    }
                    else
                    {
                        player.HP--;
                    }
                }
            }
        }

        private void UpdateEnemies(float deltaTime)
        {
            if (level.Enemies.Count(x => x.HP == 0) > 0)
            {
                AudioManager.Instance.Play(Sounds.Die);
            }

            // usuwanie martwych
            level.Enemies.RemoveAll(x => x.HP <= 0);
        }

        private void UpdateBullets(float deltaTime)
        {
            // aktualizacja pozycji
            foreach (var item in level.Bullets)
            {
                item.Position = new Vector2 { X = item.Position.X + (PLAYER_HOR_SPD + 10) * deltaTime, Y = item.Position.Y };
                item.RemainingTime -= deltaTime;
            }

            // aktualizacja kolizji
            foreach (var item in level.Bullets)
            {
                foreach (var e in level.Enemies)
                {
                    if (CheckCollisionRecs(item.Rect, e.Rect))
                    {
                        e.HP -= 1;
                        item.RemainingTime = 0;
                    }
                }
            }

            // usuwanie tych co poleciały za daleko
            level.Bullets.RemoveAll(x => x.RemainingTime <= 0);
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
            RenderBullets();
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

        private void DrawResources()
        {
            DrawText($"{player.HP}", 60, 15, 20, RED);
            DrawText($"{player.Shield}", 265, 15, 20, GREEN);
            DrawText($"{player.Ammo}", 470, 15, 20, BLUE);

            DrawText($"{norma.Energy}", 770, 15, 20, GOLD);
        }

        private void RenderBullets()
        {
            foreach (var item in level.Bullets)
            {
                DrawRectangleRec(item.Rect, RED);
            }
        }

        private void RenderPlayer2()
        {
            Vector2 virtualMouse = GetVirtualMouse();

            for (int i = 0; i < norma.Board.sizeX; i++)
            {
                for (int j = 0; j < norma.Board.sizeY; j++)
                {
                    var g = norma.Board.Grid[i, j];

                    if (g == null)
                        continue;

                    if (CheckCollisionPointRec(virtualMouse, g.Rect) && norma.Energy > 0)
                    {
                        if (!g.Hover)
                        {
                            g.Hover = true;
                        }

                        if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && !g.Active)
                        {
                            norma.Board.MarkActive(i, j, norma);
                        }
                    }
                    else if (g.Hover)
                    {
                        g.Hover = false;
                    }

                    Color c = WHITE;

                    if (g.type == TileType.RED)
                        c = RED;
                    else if (g.type == TileType.GREEN)
                        c = GREEN;
                    else if (g.type == TileType.BLUE)
                        c = BLUE;
                    else if (g.type == TileType.WHITE)
                        c = WHITE;
                    else if (g.type == TileType.YELLOW)
                        c = YELLOW;

                    if (g.Hover)
                        c = GOLD;

                    if (g.Active)
                        c = BLACK;

                    DrawTexture(jewel, (int)g.Rect.x, (int)g.Rect.y, c);
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
            DrawRectangleRec(player.Rect, RED);
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

            DrawResources();
            DrawDialogueText(dialogueManager.DisplayedDialogue.Text, dialogueManager.DisplayedDialogue.IsNorma ? BLUE : RED);
            DrawRemainingTime(level.LevelTime - ElapsedTime);
        }

        public override void Unload()
        {
            base.Unload();

            UnloadTexture(jewel);
        }

        public override void Start()
        {
            base.Start();
            timer.Start();
        }

        public override void Stop()
        {
            base.Stop();
            timer.Stop();
        }
    }
}