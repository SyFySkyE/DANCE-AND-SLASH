using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectRythym
{
    public interface IInputHandler
    {
        bool WasKeyPressed(Keys keys);

        KeyboardHandler KeyboardState { get; }
    }

    public partial class InputHandler : GameComponent, IInputHandler
    {
        private KeyboardHandler keyboard;
        private bool allowsExiting;

        public InputHandler(Game game, bool allowsExiting) : base(game)
        {
            this.allowsExiting = allowsExiting;
            game.Services.AddService(typeof(IInputHandler), this);

            keyboard = new KeyboardHandler();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update();
            base.Update(gameTime);
        }

        public bool WasKeyPressed(Keys keys)
        {
            return keyboard.WasKeyPressed(keys);
        }

        public KeyboardHandler KeyboardState
        {
            get { return keyboard; }
        }
    }

    public class KeyboardHandler
    {
        private KeyboardState prevKeyboardState;
        private KeyboardState keyboardState;

        public KeyboardHandler()
        {
            prevKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool IsHoldingKey(Keys key)
        {
            return keyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyDown(key);
        }

        public bool WasKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key);
        }

        public bool HasReleasedKey(Keys key)
        {
            return keyboardState.IsKeyUp(key) && prevKeyboardState.IsKeyDown(key);
        }

        public void Update()
        {
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public bool WasAnyKeyPressed()
        {
            Keys[] keysPressed = keyboardState.GetPressedKeys();
            if (keysPressed.Length > 0)
            {
                foreach (Keys key in keysPressed)
                {
                    if (prevKeyboardState.IsKeyUp(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
