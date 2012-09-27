using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Entities;
using Runner.EntityFramework.Components;
using Runner.EntityFramework.Actions.MobAI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Runner.ScreenFramework.Framework;
using Runner.EntityFramework.Args;
using Runner.ScreenFramework.Screens;

namespace Runner.Managers
{
    class LevelManager
    {
        public List<Entity> MobList;
        List<Arrow> arrowList;
        public Player player {get; set;}
        Bat bat;
        Spider spider;
        Cultist cultist;

        public int Score { get; set; }

        public LevelManager()
        {
            arrowList = new List<Arrow>();
            player = new Player();
            MobList = new List<Entity>();
            bat = new Bat();
            spider = new Spider();
            cultist = new Cultist();
            Score = 0;
        }

        public void Update()
        {
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
                Score++;
            }
            else
            {
                ((Mobile)bat.GetComponent("Mobile")).Tick();
                if (((Mobile)bat.GetComponent("Mobile")).Position.X < -100) { bat.IsAlive = false; }
            }

            // same for spider
            if (!spider.IsAlive)
            {
                spider = new Spider();
                Score++;
            }
            else
            {
                ((Jumping)spider.GetComponent("Jumping")).Tick();
                if (((Jumping)spider.GetComponent("Jumping")).Position.X < -100) { spider.IsAlive = false; }
            }

            // same for cultist
            if (!cultist.IsAlive)
            {
                cultist = new Cultist();
                Score++;
            }
            else
            {
                ((Mobile)cultist.GetComponent("Mobile")).Tick();
                if (((Mobile)cultist.GetComponent("Mobile")).Position.X < -100) { cultist.IsAlive = false; }
            }

            if (player.Jumping || player.ForceDown)
            {
                ((Mobile)player.GetComponent("Mobile")).Tick();
            }
        }

        public void Draw(SpriteBatch Batch)
        {
            // draw player
            Drawable playerDrawable = (Drawable)player.GetComponent("Drawable");
            Batch.Draw(GameUtil.spriteDictionary[playerDrawable.SpriteName], ((Mobile)player.GetComponent("Mobile")).Position, playerDrawable.SourceRect, Color.White, playerDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);

            // draw arrows
            if (arrowList.Count > 0)
            {
                //Texture2D debugTexture = new Texture2D(Graphics, 1, 1);
                //debugTexture.SetData(new Color[] { Color.Brown });

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
                Batch.Draw(GameUtil.spriteDictionary[spiderDrawable.SpriteName], ((Jumping)spider.GetComponent("Jumping")).Position, spiderDrawable.SourceRect, Color.White, spiderDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            if (cultist.IsAlive)
            {
                Drawable cultistDrawable = (Drawable)cultist.GetComponent("Drawable");
                Batch.Draw(GameUtil.spriteDictionary[cultistDrawable.SpriteName], ((Mobile)cultist.GetComponent("Mobile")).Position, cultistDrawable.SourceRect, Color.White, cultistDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

        public void AddArrow(Vector2 mousePosition)
        {
            arrowList.Add(new Arrow(mousePosition, ((Mobile)player.GetComponent("Mobile")).Position));
        }

        public void CheckCollisions(ScreenManager ScreenManager, GameScreen playScreen)
        {
            if (arrowList.Count > 0)
            {
                foreach (Arrow arrow in arrowList)
                {
                    // arrow on bat collision
                    if (bat.IsAlive && ((Mobile)arrow.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)bat.GetComponent("Mobile")).BoundingBox))
                    {
                        arrow.IsAlive = false;
                        bat.DoAction("TakeDamage", new SingleIntArgs(GameUtil.arrowDmg));
                        continue;
                    }

                    // arrow on spider collision
                    if (spider.IsAlive && ((Mobile)arrow.GetComponent("Mobile")).BoundingBox.Intersects(((Jumping)spider.GetComponent("Jumping")).BoundingBox))
                    {
                        arrow.IsAlive = false;
                        spider.DoAction("TakeDamage", new SingleIntArgs(GameUtil.arrowDmg));
                        continue;
                    }

                    // arrow on cultist collision
                    if (cultist.IsAlive && ((Mobile)arrow.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)cultist.GetComponent("Mobile")).BoundingBox))
                    {
                        arrow.IsAlive = false;
                        cultist.DoAction("TakeDamage", new SingleIntArgs(GameUtil.arrowDmg));
                        continue;
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
                        ScreenManager.AddScreen(new GameOverScreen(Score));
                        ScreenManager.RemoveScreen(playScreen);  // back to title screen
                    }
                }
            }

            // spider on player collision
            if (spider.IsAlive)
            {
                if (((Jumping)spider.GetComponent("Jumping")).BoundingBox.Intersects(((Mobile)player.GetComponent("Mobile")).BoundingBox))
                {
                    spider.IsAlive = false;
                    player.DoAction("TakeDamage", new SingleIntArgs(GameUtil.spiderDmg));
                    if (!player.IsAlive)
                    {
                        ScreenManager.AddScreen(new GameOverScreen(Score));
                        ScreenManager.RemoveScreen(playScreen);  // back to title screen
                    }
                }
            }

            // cultist on player collision
            if (cultist.IsAlive)
            {
                if (((Mobile)cultist.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)player.GetComponent("Mobile")).BoundingBox))
                {
                    cultist.IsAlive = false;
                    player.DoAction("TakeDamage", new SingleIntArgs(GameUtil.cultistDmg));
                    if (!player.IsAlive)
                    {
                        ScreenManager.AddScreen(new GameOverScreen(Score));
                        ScreenManager.RemoveScreen(playScreen);  // back to title screen
                    }
                }
            }
        }

        public void CheckMobAI()
        {
            bat.DoAction("BatAttack");
        }

        public void AnimateObjects(GameTime time)
        {
            int elapsedTime = (int)time.ElapsedGameTime.TotalMilliseconds;
            player.DoAction("NextFrameOfAnimation", new AnimationTimeArgs(elapsedTime));
            cultist.DoAction("NextFrameOfAnimation", new AnimationTimeArgs(elapsedTime));
        }

        private void CleanArrowList()
        {
            for (int i = arrowList.Count - 1; i >= 0; --i)
            {
                if (!arrowList[i].IsAlive)
                    arrowList.RemoveAt(i);
            }
        }

        public void HandlePlayerJump(InputState input)
        {
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
        }
    }
}
