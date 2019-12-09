using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectRythym
{
    class AttackRectangle : DrawableSprite
    {
        private Texture2D attackRectTexture;

        public AttackRectangle(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {            
            base.LoadContent();
            attackRectTexture = this.Game.Content.Load<Texture2D>("attackZoneRect");
            this.spriteTexture = attackRectTexture;
        }
    }
}
