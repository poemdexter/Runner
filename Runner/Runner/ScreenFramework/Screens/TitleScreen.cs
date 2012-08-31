using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Runner.ScreenFramework.Framework;

namespace Runner.ScreenFramework.Screens
{
    class TitleScreen : GameScreen
    {
        private const string titleText = "Runner Prototype";

        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;

        public TitleScreen()
        {
            menuEntries.Add(new MenuEntry("Start"));
            menuEntries.Add(new MenuEntry("Exit"));
            menuEntries[0].Active = true;
        }

        // handle key presses
        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Up))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }
            if (input.IsNewKeyPress(Keys.Down))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }
            if (input.IsNewKeyPress(Keys.Enter))
            {
                switch (selectedEntry)
                {
                    case (int)MainMenuEntry.Start:
                        ScreenManager.AddScreen(new PlayGameScreen());
                        break;
                    case (int)MainMenuEntry.Exit:
                        ScreenManager.Game.Exit();
                        break;
                }
            }
        }

        // update menu entry to signal which is selected
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                menuEntries[i].Active = (i == selectedEntry) ? true : false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            // draw title
            spriteBatch.DrawString(font, titleText, new Vector2(graphics.Viewport.Width / 2, 100), Color.White, 0, font.MeasureString(titleText) / 2, 4f, SpriteEffects.None, 0);

            // draw options
            int x = 0;
            foreach (MenuEntry entry in menuEntries)
            {
                spriteBatch.DrawString(font, entry.Text, new Vector2(50, graphics.Viewport.Height - 140 + x), entry.getColor(), 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                x += 30;
            }

            spriteBatch.End();
        }
    }
}
