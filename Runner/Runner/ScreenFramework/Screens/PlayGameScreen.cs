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
        Spider spider;
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
            spider = new Spider();
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
                    Vector2 arrowPosition = ((Mobile)arrow.GetComponent("Mobile")).Position;
                    if (arrowPosition.X > GameUtil.windowWidth
                        || arrowPosition.Y > GameUtil.windowHeight
                        || arrowPosition.X < 0 
                        || arrowPosition.Y < 0)
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

            if (!spider.IsAlive)
            {
                spider = new Spider();
            }
            else
            {
                ((Mobile)spider.GetComponent("Mobile")).Tick();
                if (((Mobile)spider.GetComponent("Mobile")).Position.X < -100) { spider.IsAlive = false; }
            }

            if (player.Jumping || player.ForceDown)
            {
                ((Mobile)player.GetComponent("Mobile")).Tick();
            }
            
            AnimateObjects(gameTime);

            CheckCollisions();
            CheckMobAI();
        }

        private void CheckMobAI()
        {
            bat.DoAction("BatAttack");
        }

        private void AnimateObjects(GameTime time)
        {
            int elapsedTime = (int)time.ElapsedGameTime.TotalMilliseconds;
            player.DoAction("NextFrameOfAnimation", new AnimationTimeArgs(elapsedTime));
        }

        public override void HandleInput(InputState input)
        {
            // firing arrows
            if (input.IsNewKeyPress(Keys.Z, GameUtil.arrowDelay) || input.IsNewLeftClick(GameUtil.arrowDelay)) // fire weapon
            {
                arrowList.Add(new Arrow(input.GetMousePosition(), ((Mobile)player.GetComponent("Mobile")).Position));
            }

            #region Jump Logic
            /* We put this check above new jump check because if new jump check
             * is first, then we'll register new jump, see it's not same as old
             * input state, and immediate stop jumping.  Doing it this way ensures
             * we get at least one input state with jump being true meaning we
             * can maintain the jump.
             */
            if (input.IsCurrentKeyPress(Keys.Space)) // player still jumping
            {
                if (player.Jumping && !player.ForceDown) // we're still going up!
                {
                    // hit max height?
                    if ((player.GetComponent("Mobile") as Mobile).Position.Y <= GameUtil.playerY - GameUtil.maxJumpHeight)
                    {
                        player.ForceDown = true;
                        (player.GetComponent("Mobile") as Mobile).Velocity += new Vector2(0, GameUtil.JumpFriction);
                    }
                }
            }
            else // we let go, so force going down
            {
                if (player.Jumping && !player.ForceDown)
                {
                    player.ForceDown = true;
                    (player.GetComponent("Mobile") as Mobile).Velocity += new Vector2(0, GameUtil.JumpFriction);
                }
            }

            if (input.IsNewKeyPress(Keys.Space)) // player new jump!
            {
                if (!player.Jumping && !player.ForceDown) // if not forcing down nor jumping, lets jump
                {
                    player.Jumping = true;
                    (player.GetComponent("Mobile") as Mobile).Velocity = new Vector2(0, GameUtil.JumpPower);
                }
            }

            // we need to stop falling when we hit ground.
            if (player.ForceDown)
            {
                if ((player.GetComponent("Mobile") as Mobile).Velocity.Y < GameUtil.FallPower)
                {
                    (player.GetComponent("Mobile") as Mobile).Velocity += new Vector2(0, GameUtil.JumpFriction);
                }

                if ((player.GetComponent("Mobile") as Mobile).Position.Y >= GameUtil.playerY)
                {
                    Vector2 standPosition = (player.GetComponent("Mobile") as Mobile).Position;
                    standPosition.Y = GameUtil.playerY;
                    (player.GetComponent("Mobile") as Mobile).Position = standPosition;
                    player.ForceDown = false;
                    player.Jumping = false;
                }
            }
            #endregion

            // uncomment below for awesome rope of bullets
            // arrowList.Add(new Arrow(input.GetMousePosition()));
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
            Batch.DrawString(Font, "MOBS KILLED: " + score, new Vector2(20, 40), Color.White, 0, Vector2.Zero, GameUtil.fontScale, SpriteEffects.None, 0);

            // draw player HP
            for (int i = 0; i < (player.GetComponent("Hitpoints") as Hitpoints).HP; i++)
            {
                Batch.Draw(GameUtil.spriteDictionary["heart"], new Vector2(20 + (35 * i),60), GameUtil.spriteDictionary["heart"].Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            // draw player
            Drawable playerDrawable = (Drawable)player.GetComponent("Drawable");
            Batch.Draw(GameUtil.spriteDictionary[playerDrawable.SpriteName], ((Mobile)player.GetComponent("Mobile")).Position, playerDrawable.SourceRect, Color.White, playerDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);

            // draw arrows
            if (arrowList.Count > 0)
            {
                Texture2D debugTexture = new Texture2D(Graphics, 1, 1);
                debugTexture.SetData(new Color[] { Color.Brown });
                foreach (Arrow arrow in arrowList)
                {
                    Drawable arrowDrawable = (Drawable)arrow.GetComponent("Drawable");
                    Batch.Draw(GameUtil.spriteDictionary[arrowDrawable.SpriteName], ((Mobile)arrow.GetComponent("Mobile")).Position, arrowDrawable.SourceRect, Color.White, arrowDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
                    //Batch.Draw(debugTexture, ((Mobile)arrow.GetComponent("Mobile")).BoundingBox, Color.BlanchedAlmond);
                }
            }

            if (bat.IsAlive)
            {
                Drawable batDrawable = (Drawable)bat.GetComponent("Drawable");
                Batch.Draw(GameUtil.spriteDictionary[batDrawable.SpriteName], ((Mobile)bat.GetComponent("Mobile")).Position, batDrawable.SourceRect, Color.White, batDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            if (spider.IsAlive)
            {
                Drawable spiderDrawable = (Drawable)spider.GetComponent("Drawable");
                Batch.Draw(GameUtil.spriteDictionary[spiderDrawable.SpriteName], ((Mobile)spider.GetComponent("Mobile")).Position, spiderDrawable.SourceRect, Color.White, spiderDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            Batch.End();

            base.Draw(gameTime);
        }

        private void CheckCollisions()
        {
            // arrow on bat collision
            if (arrowList.Count > 0 && bat.IsAlive)
            {
                foreach (Arrow arrow in arrowList)
                {
                    if (((Mobile)arrow.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)bat.GetComponent("Mobile")).BoundingBox))
                    {
                        arrow.IsAlive = false;
                        bat.DoAction("TakeDamage", new SingleIntArgs(GameUtil.arrowDmg));
                        score++;
                        break;
                    }
                }
            }

            // arrow on spider collision
            if (arrowList.Count > 0 && spider.IsAlive)
            {
                foreach (Arrow arrow in arrowList)
                {
                    if (((Mobile)arrow.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)spider.GetComponent("Mobile")).BoundingBox))
                    {
                        arrow.IsAlive = false;
                        spider.DoAction("TakeDamage", new SingleIntArgs(GameUtil.arrowDmg));
                        score++;
                        break;
                    }
                }
            }

            // bat on player collision
            if (bat.IsAlive)
            {
                if (((Mobile)bat.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)player.GetComponent("Mobile")).BoundingBox))
                {
                    bat.IsAlive = false;
                    player.DoAction("TakeDamage", new SingleIntArgs(GameUtil.batDmg));
                    if (!player.IsAlive)
                    {
                        ScreenManager.AddScreen(new GameOverScreen(score));
                        ScreenManager.RemoveScreen(this);  // back to title screen
                    }
                }
            }

            // spider on player collision
            if (spider.IsAlive)
            {
                if (((Mobile)spider.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)player.GetComponent("Mobile")).BoundingBox))
                {
                    spider.IsAlive = false;
                    player.DoAction("TakeDamage", new SingleIntArgs(GameUtil.spiderDmg));
                    if (!player.IsAlive)
                    {
                        ScreenManager.AddScreen(new GameOverScreen(score));
                        ScreenManager.RemoveScreen(this);  // back to title screen
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
