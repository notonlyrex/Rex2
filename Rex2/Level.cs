using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Raylib;

namespace Rex2
{
    internal class Level : LevelBase
    {
        private void UpdateCameraCenter(ref Camera2D camera, ref Player player, IEnumerable<Platform> envItems, float delta, int width, int height)
        {
            camera.offset = new Vector2(width / 2, height / 2);
            camera.target = player.Position;
        }

        private const int G = 400;

        private float PlayerJumpSpeed = 350.0f;
        private float PlayerSpeed = 200.0f;
        private float BulletSpeed = 250.0f;
        private float EnemySpeed = 100.0f;
        private float BossSpeed = 50.0f;

        private LevelDefinition level;
        private Norma norma;

        private Camera2D camera;
        private Player player;

        private DialogueManager dialogueManager;

        private bool seenBoss = false;

        private string fileName;
        private bool showIntro;

        public Level(int screenHeight, int screenWidth, bool showIntro, string fileName, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2) : base(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2)
        {
            this.fileName = fileName;
            this.showIntro = showIntro;
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

            if (!level.Enemies.Any(x => x.IsBoss))
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
            UpdatePowerups(deltaTime);

#if DEBUG
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
#endif

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
                    ei.Rect.y < p.Y + (player.Speed * deltaTime))
                {
                    hitObstacle = 1;
                    player.Speed = 0.0f;
                    player.Position = new Vector2 { X = player.Position.X, Y = ei.Rect.y };

                    if (ei.Durability > 0 && !ei.Touched)
                    {
                        dialogueManager.UpdateDialogueOnSituation(Situation.JumpNow);
                        ei.Touched = true;
                    }
                }
            }

            if (hitObstacle == 0)
            {
                player.Position = new Vector2 { X = player.Position.X, Y = player.Position.Y + (player.Speed * deltaTime) };
                player.Speed += G * deltaTime;
                player.CanJump = false;
            }
            else
                player.CanJump = true;

            foreach (var item in level.Platforms.Where(p => p.Durability > 0 && p.Touched))
            {
                item.Durability -= 1 * deltaTime;
            }

            level.Platforms.RemoveAll(p => p.Durability <= 0 && p.Touched);
        }

        private void UpdatePlayer(float deltaTime)
        {
            if (IsKeyDown(KEY_LEFT) || IsKeyDown(KEY_A))
                player.Position = new Vector2 { X = player.Position.X - (PlayerSpeed * deltaTime), Y = player.Position.Y };

            if (IsKeyDown(KEY_RIGHT) || IsKeyDown(KEY_D))
                player.Position = new Vector2 { X = player.Position.X + (PlayerSpeed * deltaTime), Y = player.Position.Y };

            if ((IsKeyDown(KEY_SPACE) || IsKeyDown(KEY_W)) && player.CanJump)
            {
                AudioManager.Instance.Play(Sounds.Jump);

                player.Speed = -PlayerJumpSpeed;

                player.CanJump = false;
            }

            if (IsKeyPressed(KEY_LEFT_CONTROL) || IsKeyPressed(KEY_RIGHT_CONTROL))
            {
                if (player.Ammo > 0)
                {
                    AudioManager.Instance.Play(Sounds.Bullet);
                    level.Bullets.Add(new Bullet { IsOrientedRight = true, RemainingTime = 5, Position = new Vector2(player.Position.X + 20, player.Position.Y - 17) });
                    player.Ammo--;
                }
            }

            if (!seenBoss && player.Position.Y < 50)
            {
                dialogueManager.UpdateDialogueOnSituation(Situation.Boss);
                seenBoss = true;
            }

            UpdatePlayerOnEnemies();
            UpdatePlayerOnPowerups();
        }

        private void UpdatePlayerOnPowerups()
        {
            // aktualizacja kolizji gracza z przeciwnikami
            foreach (var item in level.Powerups)
            {
                if (CheckCollisionRecs(item.Rect, player.Rect))
                {
                    norma.Energy++;
                    item.IsTaken = true;
                }
            }
        }

        private void UpdatePlayerOnEnemies()
        {
            // aktualizacja kolizji gracza z przeciwnikami
            foreach (var item in level.Enemies)
            {
                if (CheckCollisionRecs(item.Rect, player.Rect))
                {
                    dialogueManager.UpdateDialogueOnSituation(Situation.DoNotTouch);
                    if (player.Shield > 0)
                    {
                        player.Shield--;
                    }
                    else
                    {
                        player.HP--;
                    }

                    if (item.IsBoss)
                    {
                        player.Position = new Vector2(player.Position.X - 120, player.Position.Y - 120);
                        item.HP--;
                    }
                    else
                    {
                        item.HP = 0;
                    }
                }
            }
        }

        private void UpdateEnemies(float deltaTime)
        {
            foreach (var item in level.Enemies.Where(x => x.IsMoving))
            {
                item.Rect = new Rectangle(item.Rect.x - (EnemySpeed * deltaTime), item.Rect.y, item.Rect.width, item.Rect.height);
                if ((new Vector2(item.Rect.x, item.Rect.y) - item.Origin).Length() > 100)
                    EnemySpeed = -EnemySpeed;
            }

            foreach (var item in level.Enemies.Where(x => x.IsBoss))
            {
                item.Rect = new Rectangle(item.Rect.x, item.Rect.y - (BossSpeed * deltaTime), item.Rect.width, item.Rect.height);
                if ((new Vector2(item.Rect.x, item.Rect.y) - item.Origin).Length() > 30)
                    BossSpeed = -BossSpeed;
            }

            if (level.Enemies.Any(x => x.HP == 0))
            {
                AudioManager.Instance.Play(Sounds.Die);
            }

            // usuwanie martwych
            level.Enemies.RemoveAll(x => x.HP <= 0);
        }

        private void UpdatePowerups(float deltaTime)
        {
            if (level.Powerups.Count(x => x.IsTaken) > 0)
            {
                AudioManager.Instance.Play(Sounds.Take);
            }

            // usuwanie wziętych
            level.Powerups.RemoveAll(x => x.IsTaken);
        }

        private void UpdateBullets(float deltaTime)
        {
            // aktualizacja pozycji
            foreach (var item in level.Bullets)
            {
                item.Position = new Vector2 { X = item.Position.X + (BulletSpeed * deltaTime), Y = item.Position.Y };
                item.RemainingTime -= deltaTime;
            }

            // aktualizacja kolizji
            foreach (var item in level.Bullets)
            {
                foreach (var e in level.Enemies)
                {
                    if (CheckCollisionRecs(item.Rect, e.Rect))
                    {
                        e.HP--;
                        item.RemainingTime = 0;
                    }
                }
            }

            // usuwanie tych co poleciały za daleko
            level.Bullets.RemoveAll(x => x.RemainingTime <= 0);
        }

        private void EnableHighJump()
        {
            PlayerJumpSpeed += 30;
            dialogueManager.UpdateDialogueOnSituation(Situation.HighJump);
        }

        private void Draw()
        {
            BeginTextureMode(screenPlayer1);
            ClearBackground(Textures.Background);
            DrawTexture(Textures.BackgroundScreen1, 0, 0, WHITE);

            BeginMode2D(camera);

            RenderPlatforms();
            RenderBullets();
            RenderEnemies();
            RenderPowerups();
            RenderPlayer();

            EndMode2D();

#if DEBUG
            DrawText($"FPS: {GetFPS()}", 0, 0, 10, GOLD);
#endif

            EndTextureMode();

            BeginTextureMode(screenPlayer2);
            ClearBackground(Textures.Background);

            RenderPlayer2();

            EndTextureMode();
        }

        private void RenderPowerups()
        {
            foreach (var item in level.Powerups)
            {
                DrawTexturePro(Textures.Powerup, new Rectangle(0, 0, Textures.Powerup.width, Textures.Powerup.height), item.Rect, new Vector2(0, 0), 0, WHITE);
            }
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

                    DrawTexture(Textures.Jewel, (int)g.Rect.x, (int)g.Rect.y, c);
                }
            }
        }

        private void RenderPlayer()
        {
            DrawTextureTiled(Textures.Player, new Rectangle(0, 0, Textures.Player.width, Textures.Player.height), player.Rect, new Vector2(0, 0), 0, 1, WHITE);
        }

        private void RenderPlatforms()
        {
            foreach (var item in level.Platforms)
            {
                if (item.Type == TemplateType.Base)
                {
                    DrawTextureTiled(Textures.BasePlatform, new Rectangle(0, 0, Textures.BasePlatform.width, Textures.BasePlatform.height), item.Rect, new Vector2(0, 0), 0, 1, WHITE);
                }
                else if (item.Type == TemplateType.Platform)
                {
                    DrawTextureTiled(Textures.Platform, new Rectangle(0, 0, Textures.Platform.width, Textures.Platform.height), item.Rect, new Vector2(0, 0), 0, 1, WHITE);
                }
                else if (item.Type == TemplateType.DestructiblePlatform)
                {
                    DrawTextureTiled(Textures.DestructiblePlatform, new Rectangle(0, 0, Textures.DestructiblePlatform.width, Textures.DestructiblePlatform.height), item.Rect, new Vector2(0, 0), 0, 1, WHITE);
                }
            }
        }

        private void RenderEnemies()
        {
            foreach (var item in level.Enemies)
            {
                if (item.IsBoss)
                {
                    DrawTexturePro(Textures.Boss, new Rectangle(0, 0, Textures.Boss.width, Textures.Boss.height), item.Rect, new Vector2(0, 0), 0, WHITE);
                }
                else if (item.IsMoving)
                {
                    DrawTexturePro(Textures.Enemy2, new Rectangle(0, 0, Textures.Enemy2.width, Textures.Enemy2.height), item.Rect, new Vector2(0, 0), 0, WHITE);
                }
                else
                {
                    DrawTexturePro(Textures.Enemy, new Rectangle(0, 0, Textures.Enemy.width, Textures.Enemy.height), item.Rect, new Vector2(0, 0), 0, WHITE);
                }
            }
        }

        internal override void DrawMain()
        {
            base.DrawMain();

            DrawResources();
            DrawDialogueText(dialogueManager.DisplayedDialogue.Text, dialogueManager.DisplayedDialogue.IsNorma ? BLUE : RED);
            DrawRemainingTime(level.LevelTime - ElapsedTime);
        }

        public override void Start()
        {
            base.Start();
            level = LevelParser.Parse($"levels/{fileName}.txt");

            player = new Player();
            player.Position = new Vector2(400, level.StartingY);
            player.Speed = 0;
            player.CanJump = false;

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

            if (!showIntro)
            {
                seenBoss = true;
            }

            PlayerJumpSpeed = 350.0f;
            PlayerSpeed = 200.0f;
            BulletSpeed = 250.0f;
            EnemySpeed = 100.0f;
            BossSpeed = 50.0f;

            timer.Start();
        }

        public override void Stop()
        {
            base.Stop();
            timer.Stop();
        }
    }
}