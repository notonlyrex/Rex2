using Raylib_cs;

namespace Rex2
{
    public class AudioManager
    {
        private Dictionary<Sounds, Sound> _sounds;
        private Music music;

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

            music = Raylib.LoadMusicStream("assets/music.mp3");
        }

        public void PlayMusic()
        {
            Raylib.SetMusicVolume(music, 0.1f);
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