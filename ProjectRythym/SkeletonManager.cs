﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ProjectRythym
{
    class SkeletonManager : DrawableGameComponent
    {
        private List<MonogameSkeleton> skeletons;
        private List<MonogameSkeleton> deadSkeletons;
        private SongManager songManager;
        private MonoGameSwordsPerson player;
        private float currentTime = 0;
        private bool isPlaying = false;
        private int numberOfSkeletonDirections; 
        private float speedBpmCalulation = 350f; // Multiplying this number with song's bps determines speed of enemies // How many pixels between skeleSpawn and attackRect.
        private int speed = 2; // Speed is divided by this number  
        private SongState currentSongState;
        public SongState CurrentSongState { get { return this.currentSongState; }
            set
            {
                if (currentSongState != value)
                {
                    currentSongState = value;
                    HandleStateChange();
                }
            }
        }
        private bool hasPlayerCompletedSong = false;
        public bool HasPlayerCompletedSong { get { return this.hasPlayerCompletedSong; }    }

        private void HandleStateChange()
        {
            if (this.currentSongState == SongState.IsQueued)
            {
                songManager.QueueSong();
            }
            else if (this.currentSongState == SongState.IsPlaying)
            {
                isPlaying = true;
                songManager.ResumeSong();
            }
            else if (this.currentSongState == SongState.HasEnded)
            {
                isPlaying = false;
                hasPlayerCompletedSong = true;
                skeletons.Clear();
                currentTime = 0;
                this.currentSongState = SongState.HasNotBeenPlayed;
            }
        }

        public SkeletonManager(Game game, MonoGameSwordsPerson player) : base(game)
        {
            this.player = player;
            skeletons = new List<MonogameSkeleton>();
            songManager = new SongManager(game);
            game.Components.Add(songManager);
            this.currentSongState = SongState.HasNotBeenPlayed;
            songManager.OnSongEnd += SongManager_OnSongEnd;
        }

        private void SongManager_OnSongEnd()
        {
            this.CurrentSongState = SongState.HasEnded;        
        }

        public void AddSkeleton(string direction)
        {
            AddSkeletonToList(direction);
        }

        private void AddSkeletonToList(string direction)
        {
            MonogameSkeleton skeleton = new MonogameSkeleton(this.Game);
            skeleton.Initialize();
            skeleton.NewSpeed = (speedBpmCalulation * songManager.Bps) / speed;
            
            if (direction == "Top")
            {
                skeleton.CurrentState = SkeletonEnum.Up;
                skeleton.Direction = new Vector2(0, 1);
                skeleton.Location = new Vector2(GraphicsDevice.Viewport.Width / 2 - skeleton.spriteTexture.Width / 2, 0 - skeleton.spriteTexture.Height);
            }
            else if (direction == "Bottom")
            {
                skeleton.CurrentState = SkeletonEnum.Down;
                skeleton.Direction = new Vector2(0, -1);
                skeleton.Location = new Vector2(GraphicsDevice.Viewport.Width / 2 - skeleton.spriteTexture.Width / 2, GraphicsDevice.Viewport.Height + skeleton.spriteTexture.Height);
            }
            else if (direction == "Left")
            {
                skeleton.CurrentState = SkeletonEnum.Left;
                skeleton.Direction = new Vector2(1, 0);
                skeleton.Location = new Vector2(0 - skeleton.spriteTexture.Width, GraphicsDevice.Viewport.Height / 2 - skeleton.spriteTexture.Height / 2);
            }
            else if (direction == "Right")
            {
                skeleton.CurrentState = SkeletonEnum.Right;
                skeleton.Direction = new Vector2(-1, 0);
                skeleton.Location = new Vector2(GraphicsDevice.Viewport.Width + skeleton.spriteTexture.Width, GraphicsDevice.Viewport.Height / 2 - skeleton.spriteTexture.Height / 2);
            }
            skeleton.SetTranformAndRect();
            skeleton.Enabled = true;
            skeleton.Visible = true;
            skeletons.Add(skeleton);
        }

        public override void Initialize()
        {
            deadSkeletons = new List<MonogameSkeleton>();            
            GetNumberOfSkeleDirections();
            base.Initialize();
            StartSong();
        }

        private void GetNumberOfSkeleDirections()
        {
            numberOfSkeletonDirections = Enum.GetNames(typeof(SkeletonEnum)).Length;
        }

        private void StartSong()
        {
            songManager.Initialize();
        }

        protected override void LoadContent()
        {            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {            
            if (isPlaying)
            {
                SpawnAtBeat(gameTime);
                for (int i = 0; i < skeletons.Count; i++)
                {
                    skeletons[i].Update(gameTime);
                    if (PlayerAttack(skeletons[i]))
                    {
                        ScoreManager.AddKill();
                        skeletons[i].IsDead = true;
                        skeletons.Remove(skeletons[i]);
                    }
                    else if (SkeleAttack(skeletons[i]))
                    {
                        ScoreManager.AddDamage();
                        player.IsHurt = true;
                        skeletons.Remove(skeletons[i]);
                    }
                }
            }            
            base.Update(gameTime);
        }

        private void SpawnAtBeat(GameTime gameTime)
        {
            float lastupdatetime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentTime += lastupdatetime;
            if ((currentTime) >= (60000 / songManager.Bpm))
            {
                //float newnum = (currenttime) - (60000 / songmanager.bpm);
                //float newnum = songmanager.bpm - songmanager.bpm / (currenttime / 1000); // if this doesn't work then
                //newnum = songmanager.bpm - songmanager.bpm / (currenttime / 1000)
                SpawnRandomSkeleton();
                float newnum = (currentTime - (60000 / songManager.Bpm));
                currentTime = newnum;
            }
        }

        private void SpawnRandomSkeleton()
        {
            Random r = new Random();
            int randomSkele = r.Next(0, numberOfSkeletonDirections);
            switch (randomSkele)
            {
                case 0:
                    AddSkeleton("Left");
                    break;
                case 1:
                    AddSkeleton("Top");
                    break;
                case 2:
                    AddSkeleton("Right");
                    break;
                case 3:
                    AddSkeleton("Bottom");
                    break;
                default:                    
                    break;
            }
        }

        private bool PlayerAttack(MonogameSkeleton skele)
        {
            foreach (AttackRectangle attackRect in player.attackRects)
            {
                if (skele.Intersects(attackRect))
                {
                    if (skele.CurrentState.ToString() == player.AttackDir.ToString())
                    {
                        return true;
                    }
                }
            }            
            return false;
        }

        private bool SkeleAttack(MonogameSkeleton skele)
        {
            if (skele.Intersects(player))
            {
                if (skele.PerPixelCollision(player))
                {                    
                    return true;
                }
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (MonogameSkeleton skele in skeletons)
            {
                skele.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        private void StartSpawning()
        {
            isPlaying = true;
        }
    }
}
