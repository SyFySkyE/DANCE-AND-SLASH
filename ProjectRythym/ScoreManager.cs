using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectRythym
{
    class ScoreManager : DrawableGameComponent // TODO I get odd feelings when I look at this class. Should be redone probably.
    {
        SpriteFont font;
        private static int numberOfKills;
        private static int numberOfDamageTaken;
        public static int Score;
        public static string SongName;
        public static string SongLength;
        private static int totalNumberOfNotes = 0;
        private static float winThreshold = 0.70f; // Multiplies total # of notes with this. If # of kills exceeds this, player wins.

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
            sb.DrawString(font, "Kills: " + numberOfKills.ToString(), numOfKillsLoc, Color.Black);
            sb.DrawString(font, "Damage Taken: " + numberOfDamageTaken.ToString(), numOfDamageTakenLoc, Color.Black);
            sb.DrawString(font, "Score: " + Score, scoreLoc, Color.Black);
            sb.DrawString(font, "Song: " + SongName, songNameLoc, Color.Black);
            sb.DrawString(font, SongLength, songLengthLoc, Color.Black);
            sb.End();
            base.Draw(gameTime);
        }

        public static void AddKill()
        {
            numberOfKills++;
            Score++;
            totalNumberOfNotes++;
        }

        public static void AddDamage()
        {
            numberOfDamageTaken++;
            Score--;
            totalNumberOfNotes++;
        }

        public static bool DidPlayerPass()
        {
            if (totalNumberOfNotes * winThreshold >= numberOfKills)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ResetStats()
        {
            totalNumberOfNotes = 0;
            numberOfKills = 0;
            numberOfDamageTaken = 0;
        }
    }
}
