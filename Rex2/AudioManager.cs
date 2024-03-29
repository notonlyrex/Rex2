﻿using Raylib_cs;

namespace Rex2
{
    public class AudioManager
    {
        private Dictionary<Sounds, Sound> _sounds;
        private Music levelMusic;
        private Music menuMusic;

        private Music music;

        private bool isPlaying = false;
        private bool isLevelMusicPlaying = false;

        public static AudioManager Instance { get; private set; } = new AudioManager();

        public AudioManager()
        {
            _sounds = new Dictionary<Sounds, Sound>();
            Raylib.InitAudioDevice();

            _sounds[Sounds.Bullet] = Raylib.LoadSound(@"assets/laser.ogg");
            _sounds[Sounds.Jump] = Raylib.LoadSound(@"assets/jump.ogg");
            _sounds[Sounds.Dialogue] = Raylib.LoadSound(@"assets/dialogue.ogg");
            _sounds[Sounds.ImportantDialogue] = Raylib.LoadSound(@"assets/important-dialogue.ogg");
            _sounds[Sounds.Crush] = Raylib.LoadSound(@"assets/crush.ogg");
            _sounds[Sounds.Die] = Raylib.LoadSound(@"assets/die.ogg");
            _sounds[Sounds.Take] = Raylib.LoadSound(@"assets/take.ogg");

            levelMusic = Raylib.LoadMusicStream("assets/level.mp3");
            menuMusic = Raylib.LoadMusicStream("assets/menu.mp3");
        }

        public void PlayMusic(bool isLevel)
        {
            if (isPlaying && isLevel != isLevelMusicPlaying)
            {
                Raylib.StopMusicStream(music);
                isPlaying = false;
            }

            if (isLevel)
            {
                music = levelMusic;
            }
            else
            {
                music = menuMusic;
            }

            isLevelMusicPlaying = isLevel;
            isPlaying = true;
            Raylib.SetMusicVolume(music, 0.5f);
            Raylib.PlayMusicStream(music);
        }

        public void Play(Sounds s)
        {
            Raylib.PlaySound(_sounds[s]);
        }

        public void UpdateMusicStream()
        {
            Raylib.UpdateMusicStream(music);
        }
    }

    public enum Sounds
    {
        Bullet,
        Jump,
        Dialogue,
        ImportantDialogue,
        Crush,
        Die,
        Take
    }
}