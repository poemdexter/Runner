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
        Texture2D playerSprite;

        List<Arrow> arrowList;

        Bat bat;
        int score = 0;

        private GraphicsDevice Graphics;
        private SpriteBatch Batch;
        private SpriteFont Font;

        Vector2 cam1_position = Vector2.Zero, cam2_position = Vector2.Zero;
        Vector2 cam_velocity = new Vector2(-1, 0);

        public PlayGameScreen()
        {
            arrowList = new List<Arrow>();
            bat = new Bat();
        }

        public override void LoadContent()
        {
            playerSprite = ScreenManager.Game.Content.Load<Texture2D>("player/char_bandit");
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
            cam1_position += cam_velocity;
            cam2_position += cam_velocity;
            if (cam1_position.X <= -80)
                cam1_position = Vector2.Zero;
            if (cam2_position.X <= -200)
                cam2_position = Vector2.Zero;

            Graphics.Clear(Color.CornflowerBlue);

            // draw ground
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(cam1_position * 2f, 0)));
            Batch.Draw(GameUtil.spriteDictionary["ground"], Vector2.Zero, GameUtil.spriteDictionary["ground"].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            Batch.End();

            // draw background
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(cam2_position * .8f, 0)));
            Batch.Draw(GameUtil.spriteDictionary["background"], Vector2.Zero, GameUtil.spriteDictionary["background"].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            Batch.End();


            // draw everything else
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            Batch.DrawString(Font, "Runner Prototype " + GameUtil.VERSION, new Vector2(20, 20), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);         

            Batch.Draw(playerSprite, new Vector2(GameUtil.playerX, GameUtil.playerY), playerSprite.Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);

            if (arrowList.Count > 0)
            {
                foreach (Arrow arrow in arrowList)
                {
                    Batch.Draw(GameUtil.spriteDictionary[((Mobile)arrow.GetComponent("Mobile")).SpriteName], ((Mobile)arrow.GetComponent("Mobile")).Position, GameUtil.spriteDictionary[((Mobile)arrow.GetComponent("Mobile")).SpriteName].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
            }

            if (bat.IsAlive)
            {
                Batch.Draw(GameUtil.spriteDictionary[((Mobile)bat.GetComponent("Mobile")).SpriteName], ((Mobile)bat.GetComponent("Mobile")).Position, GameUtil.spriteDictionary[((Mobile)bat.GetComponent("Mobile")).SpriteName].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            Batch.DrawString(Font, "BAT SCORE: " + score, new Vector2(20, 40), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

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
