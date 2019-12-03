using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectRythym
{
    class ScoreManager : DrawableGameComponent
    {
        SpriteFont font;
        private static int numberOfKills;
        private static int numberOfDamageTaken;
        public static int Score;
        public static string SongName;
        public static string SongLength;

        private SpriteBatch sb;
        private Vector2 numOfKillsLoc;
        private Vector2 numOfDamageTakenLoc;
        private Vector2 scoreLoc;
        private Vector2 songNameLoc;
        private Vector2 songLengthLoc;

        public ScoreManager(Game game) : base(game)
        {
            SetUpGame();
        }

        private static void SetUpGame()
        {
            Score = 0;
            numberOfKills = 0;
            numberOfDamageTaken = 0;
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("SpriteFont1");
            numOfKillsLoc = Vector2.Zero;
            numOfDamageTakenLoc = new Vector2(0, 20);
            scoreLoc = new Vector2(0, 40);
            songNameLoc = new Vector2(0, 60);
            songLengthLoc = new Vector2(0, 80);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.DrawString(font, "Kills: " + numberOfKills.ToString(), numOfKillsLoc, Color.White);
            sb.DrawString(font, "Damage Taken: " + numberOfDamageTaken.ToString(), numOfDamageTakenLoc, Color.White);
            sb.DrawString(font, "Score: " + Score, scoreLoc, Color.White);
            sb.DrawString(font, "Song: " + SongName, songNameLoc, Color.White);
            sb.DrawString(font, SongLength, songLengthLoc, Color.White);
            sb.End();
            base.Draw(gameTime);
        }

        public static void AddKill()
        {
            numberOfKills++;
            Score++;
        }

        public static void AddDamage()
        {
            numberOfDamageTaken++;
            Score--;
        }
    }
}
