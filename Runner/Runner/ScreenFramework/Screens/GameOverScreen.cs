using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.ScreenFramework.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Runner.ScreenFramework.Screens
{
    class GameOverScreen : GameScreen
    {
        private const string GameOverText = "- GAME OVER -";
        private const string InstructionText = "Press Spacebar or Left Click to go back to main menu.";
        int Score { get; set; }

        public GameOverScreen(int score)
        {
            Score = score;
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Space) || input.IsNewLeftClick())
            {
                ScreenManager.AddScreen(new TitleScreen());
                ScreenManager.RemoveScreen(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            // draw title
            spriteBatch.DrawString(font, GameOverText, new Vector2(graphics.Viewport.Width / 2, 100), Color.White, 0, font.MeasureString(GameOverText) / 2, 4f, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, "Score: " + Score, new Vector2(graphics.Viewport.Width / 2, 200), Color.White, 0, font.MeasureString("Score: " + Score) / 2, 4f, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, InstructionText, new Vector2(graphics.Viewport.Width / 2, 300), Color.White, 0, font.MeasureString(InstructionText) / 2, 4f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
