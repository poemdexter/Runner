using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Entities;
using Runner.EntityFramework.Components;
using Runner.EntityFramework.Actions.AttackAI;
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

        public Player player { get; set; }
        public int Score { get; set; }

        int updateTime = 0;

<<<<<<< Updated upstream
=======
        public bool MultiShot { get; set; }
        public int WeaponDelay { get; set; }

>>>>>>> Stashed changes
        public LevelManager()
        {
            arrowList = new List<Arrow>();
            player = new Player();
            MobList = new List<Entity>();
            Score = 0;
<<<<<<< Updated upstream
=======
            MultiShot = false;
            WeaponDelay = GameUtil.arrowDelay;
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }

        public void Update(GameTime gameTime)
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
                        arrow.DoAction("Move");
                }
                CleanArrowList();
            }

            // controls how often to spawn enemies
            updateTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (updateTime >= GameUtil.level_1_spawnTime)
            {
                // spawn mob
                int spawnChance = GameUtil.random.Next(1, 101);
                if (spawnChance < 40)
                {
                    MobList.Add(new Bat());
                }
                else if (spawnChance < 80)
                {
                    MobList.Add(new Spider());
                }
                else
                {
                    MobList.Add(new Cultist());
                }


                updateTime = 0;
            }

            if (MobList.Count > 0)
            {
                foreach (Entity mob in MobList)
                {
                    mob.DoAction("Move", new SingleIntArgs((int)gameTime.ElapsedGameTime.TotalMilliseconds));
                    if (((Mobile)mob.GetComponent("Mobile")).Position.X < -100) { mob.IsAlive = false; }
                }
                CleanMobList();
            }

            if (player.Jumping || player.ForceDown)
            {
                player.DoAction("Move");
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
                foreach (Arrow arrow in arrowList)
                {
                    Drawable arrowDrawable = (Drawable)arrow.GetComponent("Drawable");
                    Batch.Draw(GameUtil.spriteDictionary[arrowDrawable.SpriteName], ((Mobile)arrow.GetComponent("Mobile")).Position, arrowDrawable.SourceRect, Color.White, arrowDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
            }

            if (MobList.Count > 0)
            {
                foreach (Entity mob in MobList)
                {
                    Drawable mobDrawable = (Drawable)mob.GetComponent("Drawable");
                    Batch.Draw(GameUtil.spriteDictionary[mobDrawable.SpriteName], ((Mobile)mob.GetComponent("Mobile")).Position, mobDrawable.SourceRect, Color.White, mobDrawable.Rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
            }
        }

        public void Fire(Vector2 mousePosition)
        {
            // TODO Handle weapon modifications here

<<<<<<< Updated upstream
            // arrow position
            Vector2 arrowPosition = WeaponManager.CalculatePosition(((Mobile)player.GetComponent("Mobile")).Position, GameUtil.spriteDictionary["arrow"].Height);
            // arrow velocity
            Vector2 arrowVelocity = WeaponManager.CalculateVelocity(mousePosition, arrowPosition);

            // normal single, one shot
            arrowList.Add(new Arrow(arrowPosition, arrowVelocity));
=======
            // fires 3 shots with spread dependent on distance
            if (MultiShot)
            {
                List<Arrow> multishotList = WeaponManager.MultiShot(arrowSpawnPosition, arrowVelocity);
                foreach (Arrow a in multishotList) { arrowList.Add(a); }
            }
            // normal single, one shot
            else
            {
                arrowList.Add(new Arrow(arrowSpawnPosition, Vector2.Normalize(arrowVelocity)));
            }
>>>>>>> Stashed changes
        }

        public void CheckCollisions(ScreenManager ScreenManager, GameScreen playScreen)
        {
            // arrow on mob collision
            if (arrowList.Count > 0 && MobList.Count > 0)
            {
                foreach (Arrow arrow in arrowList)
                {
                    foreach (Entity mob in MobList)
                    {
                        if (arrow.IsAlive && mob.IsAlive && ((Mobile)arrow.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)mob.GetComponent("Mobile")).BoundingBox))
                        {
                            arrow.IsAlive = false;
                            mob.DoAction("TakeDamage", new SingleIntArgs(GameUtil.arrowDmg));
                            if (!mob.IsAlive)
                                Score++;
                            continue;
                        }
                    }
                }
                CleanArrowList();
                CleanMobList();
            }

            // mob on player collision
            if (MobList.Count > 0)
            {
                foreach (Entity mob in MobList)
                {
                    if (mob.IsAlive)
                    {
                        if (((Mobile)mob.GetComponent("Mobile")).BoundingBox.Intersects(((Mobile)player.GetComponent("Mobile")).BoundingBox))
                        {
                            mob.IsAlive = false;
                            player.DoAction("TakeDamage", new SingleIntArgs(((Mobile)mob.GetComponent("Mobile")).CollisionDamage));
                            if (!player.IsAlive)
                            {
                                ScreenManager.AddScreen(new GameOverScreen(Score));
                                ScreenManager.RemoveScreen(playScreen);  // back to title screen
                            }
                        }
                    }
                }
                CleanMobList();
            }
        }

        public void CheckMobAI()
        {
            if (MobList.Count > 0)
            {
                foreach (Entity mob in MobList)
                {
                    if (mob.IsAlive)
                    {
                        mob.DoAction("MobAttackAI");
                    }
                }
            }
        }

        public void AnimateObjects(GameTime time)
        {
            int elapsedTime = (int)time.ElapsedGameTime.TotalMilliseconds;
            player.DoAction("NextFrameOfAnimation", new AnimationTimeArgs(elapsedTime));
            if (MobList.Count > 0)
            {
                foreach (Entity mob in MobList)
                {
                    if (mob.IsAlive)
                    {
                        mob.DoAction("NextFrameOfAnimation", new AnimationTimeArgs(elapsedTime));
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

        private void CleanMobList()
        {
            for (int i = MobList.Count - 1; i >= 0; --i)
            {
                if (!MobList[i].IsAlive)
                    MobList.RemoveAt(i);
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
