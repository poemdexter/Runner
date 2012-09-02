using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Runner.ScreenFramework.Framework;
using Runner.EntityFramework.Components;

namespace Runner.ScreenFramework.Screens
{
    class PlayGameScreen : GameScreen
    {
        Texture2D playerSprite, arrowSprite, batSprite;

        List<Arrow> arrowList;

        Bat bat;
        int score = 0;

        private GraphicsDevice Graphics;
        private SpriteBatch Batch;
        private SpriteFont Font;

        public PlayGameScreen()
        {
            arrowList = new List<Arrow>();
            bat = new Bat();
        }

        public override void LoadContent()
        {
            playerSprite = ScreenManager.Game.Content.Load<Texture2D>("player/char_bandit");
            arrowSprite = ScreenManager.Game.Content.Load<Texture2D>("entities/arrow");
            batSprite = ScreenManager.Game.Content.Load<Texture2D>("mobs/bat");
            Graphics = ScreenManager.GraphicsDevice;
            Batch = ScreenManager.SpriteBatch;
            Font = ScreenManager.Font;
        }

        public override void Update(GameTime gameTime)
        {
            if (arrowList.Count > 0)
            {
                foreach (Arrow arrow in arrowList)
                {
                    if (((Mobile)arrow.GetComponent("Mobile")).Position.X > GameUtil.windowWidth)
                        arrow.IsAlive = false;
                    else
                        ((Mobile)arrow.GetComponent("Mobile")).Tick();
                }
                CleanArrowList();
            }

            if (!bat.IsAlive)
            {
                bat = new Bat();
            }
            else
            {
                ((Mobile)bat.GetComponent("Mobile")).Tick();
                CheckCollisions();
            }
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Space) || input.IsNewLeftClick())
            {
                // fire ze arrow!
                arrowList.Add(new Arrow(input.GetMousePosition()));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Graphics.Clear(Color.CornflowerBlue);

            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            Batch.DrawString(Font, "Runner Prototype " + GameUtil.VERSION, new Vector2(20, 20), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

            Batch.Draw(playerSprite, new Vector2(GameUtil.playerX, GameUtil.playerY), playerSprite.Bounds, Color.White, 0f, Vector2.Zero, GameUtil.spriteScale, SpriteEffects.None, 0);

            if (arrowList.Count > 0)
            {
                foreach (Arrow arrow in arrowList)
                {
                    Batch.Draw(GameUtil.spriteDictionary[((Mobile)arrow.GetComponent("Mobile")).SpriteName], ((Mobile)arrow.GetComponent("Mobile")).Position, GameUtil.spriteDictionary[((Mobile)arrow.GetComponent("Mobile")).SpriteName].Bounds, Color.White, 0f, Vector2.Zero, GameUtil.spriteScale, SpriteEffects.None, 0);
                }
            }

            if (bat.IsAlive)
            {
                Batch.Draw(GameUtil.spriteDictionary[((Mobile)bat.GetComponent("Mobile")).SpriteName], ((Mobile)bat.GetComponent("Mobile")).Position, GameUtil.spriteDictionary[((Mobile)bat.GetComponent("Mobile")).SpriteName].Bounds, Color.White, 0f, Vector2.Zero, GameUtil.spriteScale, SpriteEffects.None, 0);
            }

            Batch.DrawString(Font, "BAT SCORE: " + score, new Vector2(20, 600), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

            Batch.End();

            base.Draw(gameTime);
        }

        private void CheckCollisions()
        {
            if (arrowList.Count > 0)
            {
                foreach (Arrow arrow in arrowList)
                {
                    if (((Mobile)arrow.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)bat.GetComponent("Mobile")).BoundingBox))
                    {
                        arrow.IsAlive = false;
                        bat.IsAlive = false;
                        score++;
                        break;
                    }
                }
            }
        }

        private void CleanArrowList()
        {
            for (int i = arrowList.Count - 1; i >= 0; --i)
            {
                if (!arrowList[i].IsAlive)
                    arrowList.RemoveAt(i);
            }
        }
    }
}
