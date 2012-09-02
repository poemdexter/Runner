using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;

namespace Runner
{
    public class Arrow : Entity
    {
        public Arrow(Vector2 target)
        {
            IsAlive = true;
            this.AddComponent(new Mobile("arrow", new Vector2(GameUtil.playerX, GameUtil.playerY), CalculateVelocity(target) * GameUtil.arrowSpeed));
        }

        private Vector2 CalculateVelocity(Vector2 target)
        {
            float rise = target.Y - GameUtil.playerY;
            float run = target.X - GameUtil.playerX;
            Vector2 slope = new Vector2(run, rise);
            return Vector2.Normalize(slope);
        }
    }
}
