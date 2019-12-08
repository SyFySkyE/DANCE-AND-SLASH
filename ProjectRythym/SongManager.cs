using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ProjectRythym
{    
    class SongManager : GameComponent
    {
        private Song song;
        private int bpm = 174;
        public int Bpm { get { return this.bpm; } }
        private float bps = 2.9f;
        public float Bps { get { return this.bps; } }
        private float bpms = 0;
        private float previousFrameTime;
        private bool isPlaying;
        public bool IsPlaying { get { return this.isPlaying; }
            set
            {
                if (this.isPlaying != value)
                {
                    this.isPlaying = value;
                }
            }
        }

        public static event Action OnBeat;
        public event Action OnSongEnd;

        public SongManager( Game game) : base(game)
        {            
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        public override void Initialize()
        {
            song = Game.Content.Load<Song>("Crystal Tokyo by FantoMenk and Meganeko");
            ScoreManager.SongName = song.Name;
            bps = (bpm / 60);
            bpms = 60000 / bpm; // Every beat per bpms            
            isPlaying = false;
            base.Initialize();
        }

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            MediaPlayer.Volume = 0.5f;            
        }

        public override void Update(GameTime gameTime)
        {
            ScoreManager.SongLength = $"{MediaPlayer.PlayPosition} / {song.Duration}";
            if (MediaPlayer.PlayPosition == song.Duration)
            {
                OnSongEnd(); // Stil doesn't work
            }
            base.Update(gameTime);
        }

        public void QueueSong()
        {
            MediaPlayer.Play(song);
            MediaPlayer.Pause();
        }

        public void ResumeSong()
        {
            MediaPlayer.Resume();
            isPlaying = true;
        }

        public string GetSongInfo()
        {
            return $"{song.Name} by {song.Artist}"; ;
        }

        public string GetPlayPosition()
        {
            return $"{MediaPlayer.PlayPosition} / {song.Duration}";
        }
    }    
}
