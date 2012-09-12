using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Runner.ScreenFramework.Framework
{
    public class InputState
    {
        KeyboardState CurrentKeyboardState, LastKeyboardState;
        MouseState CurrentMouseState, LastMouseState;

        private int InputElapsedTime;

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
            CurrentMouseState = new MouseState();
            LastMouseState = new MouseState();
            InputElapsedTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            LastKeyboardState = CurrentKeyboardState;
            LastMouseState = CurrentMouseState;
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            InputElapsedTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool IsNewKeyPress(Keys key, int inputDelay)
        {
            if (InputElapsedTime <= 0)
            {
                if (CurrentKeyboardState.IsKeyDown(key))
                {
                    InputElapsedTime = inputDelay;
                    return true;
                }
            }
            return false;
        }

        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key)) ? true : false;
        }

        public bool IsCurrentKeyPress(Keys key)
        {
            return LastKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsNewLeftClick(int inputDelay)
        {
            if (InputElapsedTime <= 0)
            {
                if (CurrentMouseState.LeftButton == ButtonState.Pressed)
                {
                    InputElapsedTime = inputDelay;
                    return true;
                }
            }
            return false;
        }

        public bool IsNewLeftClick()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed) ? true : false;
        }

        public Vector2 GetMousePosition()
        {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }
    }
}
