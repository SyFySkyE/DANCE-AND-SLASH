using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ProjectRythym
{
    class GameManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        private MonoGameSwordsPerson player;
        private SkeletonManager skeleManager;
        private PlayerController input;
        private ScoreManager score;
        private GroundManager ground;

        private string instruction1 = "Press W";
        private string instruction2 = "Press S";

        private Vector2 instructionLoc;

        public GameManager(Game game, SpriteBatch sb) : base(game)
        {
            spriteBatch = sb;
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
            instructionLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, 600);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = this.Game.Content.Load<SpriteFont>("SpriteFont1");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (skeleManager.CurrentSongState == SongState.HasNotBeenPlayed)
            {
                spriteBatch.DrawString(font, instruction1, instructionLoc, Color.Black);
                if (input.HandleKeyboard() == "Top")
                {
                    skeleManager.CurrentSongState = SongState.IsQueued;                    
                }
            }
            else if (skeleManager.CurrentSongState == SongState.IsQueued)
            {
                spriteBatch.DrawString(font, instruction2, instructionLoc, Color.Black);
                if (input.HandleKeyboard() == "Bottom")
                {
                    skeleManager.CurrentSongState = SongState.IsPlaying;
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
