using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ProjectRythym
{
    class GameManager : DrawableSprite // TODO GameManager does NOT need to be a drawable sprite, but drawable gamecomponent breaks! Write a new Drawable Game Component that can draw but doesn't need a Texture
    {
        private SpriteFont font;
        private Texture2D baseTexture; // TODO This is messy. GameManager does not need to be a drawable sprite

        private MonoGameSwordsPerson player;
        private SkeletonManager skeleManager;
        private PlayerController input;
        private ScoreManager score;
        private GroundManager ground;

        private string instruction1 = "Press W or UP to Load Song";
        private string instruction2 = "Press S or DOWN to Play Song";
        private string winText = "You Win! Load another song to keep Dancing and Slashing!";
        private string loseText = "You Lose! You need to kill at least 70% of the skeletons to pass!";

        private Vector2 instructionLoc;
        private Vector2 resultsLoc;

        public GameManager(Game game, SpriteBatch sb) : base(game)
        {            
            ground = new GroundManager(game);
            this.Game.Components.Add(ground);

            player = new MonoGameSwordsPerson(game);
            this.Game.Components.Add(player);

            skeleManager = new SkeletonManager(game, player);
            this.Game.Components.Add(skeleManager);

            input = new PlayerController(game);
            this.Game.Components.Add(input);

            score = new ScoreManager(game);
            this.Game.Components.Add(score);
        }

        public override void Initialize()
        {
            instructionLoc = new Vector2((GraphicsDevice.Viewport.Width / 2) - 100, 600); // TODO Find a better way to get XPos
            resultsLoc = new Vector2((GraphicsDevice.Viewport.Width / 2) - 100, GraphicsDevice.Viewport.Height / 2);
            this.SpriteTexture = Game.Content.Load<Texture2D>("SpriteMarker");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = this.Game.Content.Load<SpriteFont>("SpriteFont1");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime) // TODO Clean this up!!
        {
            spriteBatch.Begin();
            if (skeleManager.CurrentSongState == SongState.HasNotBeenPlayed)
            {
                spriteBatch.DrawString(font, instruction1, instructionLoc, Color.Black);
                if (input.HandleKeyboard() == "Top")
                {
                    skeleManager.CurrentSongState = SongState.IsQueued;                    
                }
                if (skeleManager.HasPlayerCompletedSong)
                {
                    if (ScoreManager.DidPlayerPass())
                    {
                        spriteBatch.DrawString(font, winText, resultsLoc, Color.Black);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, loseText, resultsLoc, Color.Black);
                    }
                }
            }
            else if (skeleManager.CurrentSongState == SongState.IsQueued)
            {
                spriteBatch.DrawString(font, instruction2, instructionLoc, Color.Black);
                if (input.HandleKeyboard() == "Bottom")
                {
                    ScoreManager.ResetStats();
                    skeleManager.CurrentSongState = SongState.IsPlaying;
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
