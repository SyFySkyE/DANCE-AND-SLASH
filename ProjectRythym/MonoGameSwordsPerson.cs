using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectRythym
{
    public enum PlayerAttackDir { Left, Right, Up, Down, Idle }

    class MonoGameSwordsPerson : DrawableSprite
    {
        public PlayerController controller;
        private Swordsperson player;
        private Texture2D idleTexture;
        private Texture2D attackLeftTexture;
        private Texture2D attackRightTexture;
        private Texture2D attackUpTexture;
        private Texture2D attackDownTexture;
        private Texture2D hurtTexture;
        private bool isHurt = false;
        public bool IsHurt { get { return this.isHurt; }
            set
            {
                this.isHurt = value;
            }
        }
        private float currentTime = 0f;
        private float timeBeforeIdleReset = 1f;
        public AttackRectangle[] attackRects = new AttackRectangle[4];
        private PlayerAttackDir attackDir;
        public PlayerAttackDir AttackDir { get { return this.attackDir; } }

        public MonoGameSwordsPerson(Game game) : base(game)
        {            
            this.controller = new PlayerController(game);
            player = new Swordsperson();
            AddDrawingRects();
        }

        private void AddDrawingRects()
        {
            for (int i = 0; i < attackRects.Length; i++)
            {
                attackRects[i] = new AttackRectangle(this.Game);
                this.Game.Components.Add(attackRects[i]);
            }
        }

        public override void Initialize()
        {
            this.attackDir = PlayerAttackDir.Idle;
            base.Initialize();
        }

        private void DrawAttackRects()
        {
            attackRects[0].Location = new Vector2(this.Location.X - this.spriteTexture.Width * 1.5f, this.Location.Y - spriteTexture.Height / 2);
            attackRects[1].Location = new Vector2(this.Location.X + this.spriteTexture.Width * 0.5f, this.Location.Y - spriteTexture.Height / 2);
            attackRects[2].Location = new Vector2(this.Location.X - this.spriteTexture.Width / 2, this.Location.Y - this.spriteTexture.Height * 1.5f);
            attackRects[3].Location = new Vector2(this.Location.X - this.spriteTexture.Width / 2, this.Location.Y + this.spriteTexture.Height * 0.5f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            LoadTextures();
            this.spriteTexture = idleTexture;
            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
            this.Location = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);            
            DrawAttackRects();
        }

        private void LoadTextures()
        {
            idleTexture = this.Game.Content.Load<Texture2D>("playerIdle");
            attackLeftTexture = this.Game.Content.Load<Texture2D>("playerAttackLeft");
            attackRightTexture = this.Game.Content.Load<Texture2D>("playerAttackRight");
            attackUpTexture = this.Game.Content.Load<Texture2D>("playerAttackUp");
            attackDownTexture = this.Game.Content.Load<Texture2D>("playerAttackDown");
            hurtTexture = this.Game.Content.Load<Texture2D>("playerHurt");
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            this.controller.Update(gameTime);
            if (isHurt)
            {
                this.SpriteTexture = hurtTexture;
            }
            HandleControllerInput(time);
            base.Update(gameTime);
        }

        private void HandleControllerInput(float timeInMs)
        {
            switch (this.controller.HandleKeyboard())
            {
                case "Left":
                    AttackLeft();
                    break;
                case "Right":
                    AttackRight();
                    break;
                case "Top":
                    AttackTop();
                    break;
                case "Bottom":
                    AttackDown();
                    break;
            }
            ResetIdleSprite(timeInMs);
        }

        private void AttackLeft()
        {
            this.spriteTexture = attackLeftTexture;
            Point topLeftPoint = new Point(this.locationRect.Left - this.spriteTexture.Width, this.locationRect.Top);
            Point bottomRightPoint = new Point(this.locationRect.Left, this.locationRect.Bottom);
            Rectangle attackZoneLeft = new Rectangle(topLeftPoint, bottomRightPoint);
            this.attackDir = PlayerAttackDir.Left;
        }

        private void AttackRight()
        {
            this.spriteTexture = attackRightTexture;
            Point topLeftPoint = new Point(this.locationRect.Right, this.locationRect.Top);
            Point bottomRightPoint = new Point(this.locationRect.Right + this.spriteTexture.Width, this.locationRect.Bottom);
            Rectangle attackZoneRight = new Rectangle(topLeftPoint, bottomRightPoint);
            this.attackDir = PlayerAttackDir.Right;
        }

        private void AttackTop()
        {
            this.spriteTexture = attackUpTexture;
            Point topLeftPoint = new Point(this.locationRect.Left, this.locationRect.Top - spriteTexture.Height);
            Point bottomRightPoint = new Point(this.locationRect.Right, this.locationRect.Top);
            Rectangle attackZoneTop = new Rectangle(topLeftPoint, bottomRightPoint);
            this.attackDir = PlayerAttackDir.Up;    
        }

        private void AttackDown()
        {
            this.spriteTexture = attackDownTexture;
            Point topLeftPoint = new Point(this.locationRect.Left, this.locationRect.Bottom);
            Point bottomRightPoint = new Point(this.locationRect.Right, this.locationRect.Bottom + this.spriteTexture.Height);
            Rectangle attackZoneBottom = new Rectangle(topLeftPoint, bottomRightPoint);
            this.attackDir = PlayerAttackDir.Down;
        }
        
        private void ResetIdleSprite(float timeInMs)
        {            
            if (this.spriteTexture != idleTexture && controller.HandleKeyboard() == "No Input")
            {
                this.attackDir = PlayerAttackDir.Idle;
                currentTime += timeInMs / 1000;
                if (currentTime >= timeBeforeIdleReset)
                {
                    this.spriteTexture = idleTexture;
                    currentTime = 0f;
                }
            }
            else if (controller.HandleKeyboard() != "No Input")
            {
                currentTime = 0f;
                IsHurt = false;
            }
        }
    }
}
