using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectRythym
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        MonoGameSwordsPerson player;
        SkeletonManager skeleManager;
        private SongManager songManager;
        private PlayerController input;
        private ScoreManager score;
        private GroundManager ground;

        private string instruction;
        private string instruction1 = "Press I to load song, then R to play";        

        private Vector2 instructionLoc;
        private double framesPerSecond = 60;

        public Game1()
        {
            ground = new GroundManager(this);
            this.Components.Add(ground);

            player = new MonoGameSwordsPerson(this);
            this.Components.Add(player);

            songManager = new SongManager(this);
            this.Components.Add(songManager);

            skeleManager = new SkeletonManager(this, player);
            this.Components.Add(skeleManager);                       

            input = new PlayerController(this);
            this.Components.Add(input);

            score = new ScoreManager(this);
            this.Components.Add(score);

            IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / framesPerSecond); 

            this.Window.Title = TargetElapsedTime.ToString();

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 800;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            instructionLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, 600);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = this.Content.Load<SpriteFont>("SpriteFont1");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!songManager.IsPlaying)
            {
                instruction = instruction1;
            }
            else
            {
                instruction = "";
            }
            
            if (input.HandleKeyboard() == "start") 
            {                
                songManager.QueueSong();
            }
            else if (input.HandleKeyboard() == "resume")
            {
                skeleManager.StartSpawning();
                songManager.ResumeSong();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, instruction, instructionLoc, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
