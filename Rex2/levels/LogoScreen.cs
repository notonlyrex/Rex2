﻿using Raylib_cs;
using System.Numerics;

namespace Rex2.levels
{
    internal class LogoScreen : LevelBase
    {
        private Texture2D logo;

        public LogoScreen(int screenHeight, int screenWidth, ref RenderTexture2D screenPlayer1, ref RenderTexture2D screenPlayer2) : base(screenHeight, screenWidth, ref screenPlayer1, ref screenPlayer2)
        {
            logo = Raylib.LoadTexture("assets/logo.png");
        }

        public override void Update(float deltaTime)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                LevelManager.Instance.Menu();
            }
        }

        internal override void DrawMain()
        {
            Raylib.DrawTextureEx(logo, Vector2.Zero, 0, 2, Color.WHITE);
        }

        public override void Start()
        {
            base.Start();
            AudioManager.Instance.PlayMusic();
        }
    }
}