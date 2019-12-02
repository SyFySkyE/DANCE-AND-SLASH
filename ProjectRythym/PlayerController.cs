using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectRythym
{
    class PlayerController : GameComponent
    {
        InputHandler input;

        public PlayerController(Game game) : base(game)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            if (input == null)
            {
                input = new InputHandler(game, true);
                game.Components.Add(input);
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboard();
            base.Update(gameTime);
        }

        public string HandleKeyboard()
        {
            if (input.KeyboardState.WasKeyPressed(Keys.A) || input.KeyboardState.WasKeyPressed(Keys.Left))
            {
                return "Left";
            }
            else if (input.KeyboardState.WasKeyPressed(Keys.D) || input.KeyboardState.WasKeyPressed(Keys.Right))
            {
                return "Right";
            }
            else if (input.KeyboardState.WasKeyPressed(Keys.W) || input.KeyboardState.WasKeyPressed(Keys.Up))
            {
                return "Top";
            }
            else if (input.KeyboardState.WasKeyPressed(Keys.S) || input.KeyboardState.WasKeyPressed(Keys.Down))
            {
                return "Bottom";
            }
            else if (input.KeyboardState.WasKeyPressed(Keys.I))
            {
                return "start";
            }
            else if (input.KeyboardState.WasKeyPressed(Keys.R))
            {
                return "resume";
            }
            else
            {
                return "No Input";
            }
        }
    }
}
