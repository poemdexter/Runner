﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework;

namespace Runner.EntityFramework.Actions.MobAI
{
    class BatAttack : MobAttacking
    {
        private bool Attacking = false;
        public BatAttack()
        {
            this.Name = "BatAttack";
        }

        public override void Do()
        {
            if (!Attacking)
            {
                Mobile mobile = this.Entity.GetComponent("Mobile") as Mobile;

                if (mobile.Position.X < 200)
                {
                    Attacking = true;
                    mobile.Velocity = CalculateVelocity(mobile.Position) * GameUtil.batSpeed;
                }
            }
        }

        private Vector2 CalculateVelocity(Vector2 batPos)
        {
            float rise = GameUtil.playerY - batPos.Y;
            float run = GameUtil.playerX - batPos.X;
            Vector2 slope = new Vector2(run, rise);
            return Vector2.Normalize(slope);
        }
    }
}
