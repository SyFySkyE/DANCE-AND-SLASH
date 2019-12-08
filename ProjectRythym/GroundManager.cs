using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ProjectRythym
{
    class GroundManager : DrawableSprite 
    {
        private Texture2D backdrop;

        public GroundManager(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            backdrop = Game.Content.Load<Texture2D>("dnsback");
            this.SpriteTexture = backdrop;
            base.LoadContent();            
        }
    }
}
