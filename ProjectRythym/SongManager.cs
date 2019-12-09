using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ProjectRythym
{
    public enum SongState { HasNotBeenPlayed, IsQueued, IsPlaying, HasEnded }

    class SongManager : GameComponent
    {
        private Song song;
        private int bpm = 140; // 174 for Crystal Tokyo, 140 for Positive Force
        public int Bpm { get { return this.bpm; } }
        private float bps = 2.9f; // 2.9 for Crystal Tokyo, 2.33 for Positive Force. This gets set below no need to change this.
        public float Bps { get { return this.bps; } }
        private float bpms = 0;
        private SongState songState = SongState.HasNotBeenPlayed;
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

        public event Action OnSongEnd;

        public SongManager( Game game) : base(game)
        {            
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            song = Game.Content.Load<Song>("positiveforce"); // 'Crystal Tokyo by FantoMenk and Meganeko' or 'positiveforce'
        }

        public override void Initialize()
        {            
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
            if (this.songState == SongState.IsPlaying)
            {
                if (MediaPlayer.State == MediaState.Stopped && songState == SongState.IsPlaying)
                {
                    OnSongEnd();
                    this.songState = SongState.HasEnded;
                }
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
            this.songState = SongState.IsPlaying;
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
