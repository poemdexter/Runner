using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Runner.ScreenFramework.Framework;
using Runner.EntityFramework.Components;
using Runner.EntityFramework.Entities;
using Runner.EntityFramework.Args;

namespace Runner.ScreenFramework.Screens
{
    class PlayGameScreen : GameScreen
    {
        List<Arrow> arrowList;

        Bat bat;
        Player player;

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
            player = new Player();
        }

        public override void LoadContent()
        {
            Graphics = ScreenManager.GraphicsDevice;
            Batch = ScreenManager.SpriteBatch;
            Font = ScreenManager.Font;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            // move the arrows
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

            // make a bat if dead else move bat
            if (!bat.IsAlive)
            {
                bat = new Bat();
            }
            else
            {
                ((Mobile)bat.GetComponent("Mobile")).Tick();
                if (((Mobile)bat.GetComponent("Mobile")).Position.X < -100) { bat.IsAlive = false; }
            }

            AnimateObjects(gameTime);

            CheckCollisions();
        }

        private void AnimateObjects(GameTime time)
        {
            int elapsedTime = (int)time.ElapsedGameTime.TotalMilliseconds;
            player.DoAction("NextFrameOfAnimation", new AnimationTimeArgs(elapsedTime));
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
            if (cam1_position.X <= -40)
                cam1_position = Vector2.Zero;
            if (cam2_position.X <= -200)
                cam2_position = Vector2.Zero;

            Graphics.Clear(Color.CornflowerBlue);

            // draw ground
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(cam1_position * 4f, 0)));
            Batch.Draw(GameUtil.spriteDictionary["ground"], Vector2.Zero, GameUtil.spriteDictionary["ground"].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            Batch.End();

            // draw background
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateTranslation(new Vector3(cam2_position * .8f, 0)));
            Batch.Draw(GameUtil.spriteDictionary["background"], Vector2.Zero, GameUtil.spriteDictionary["background"].Bounds, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            Batch.End();


            // draw everything else
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            Batch.DrawString(Font, "Runner Prototype " + GameUtil.VERSION, new Vector2(20, 20), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

            // draw player
            Drawable playerDrawable = (Drawable)player.GetComponent("Drawable");
            Batch.Draw(GameUtil.spriteDictionary[playerDrawable.SpriteName], new Vector2(GameUtil.playerX, GameUtil.playerY), playerDrawable.SourceRect, Color.White, playerDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);

            if (arrowList.Count > 0)
            {
                foreach (Arrow arrow in arrowList)
                {
                    Drawable arrowDrawable = (Drawable)arrow.GetComponent("Drawable");
                    Batch.Draw(GameUtil.spriteDictionary[arrowDrawable.SpriteName], ((Mobile)arrow.GetComponent("Mobile")).Position, arrowDrawable.SourceRect, Color.White, arrowDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
            }

            if (bat.IsAlive)
            {
                Drawable batDrawable = (Drawable)bat.GetComponent("Drawable");
                Batch.Draw(GameUtil.spriteDictionary[batDrawable.SpriteName], ((Mobile)bat.GetComponent("Mobile")).Position, batDrawable.SourceRect, Color.White, batDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            Batch.DrawString(Font, "BAT SCORE: " + score, new Vector2(20, 40), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

            Batch.End();

            base.Draw(gameTime);
        }

        private void CheckCollisions()
        {
            if (arrowList.Count > 0 && bat.IsAlive)
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
