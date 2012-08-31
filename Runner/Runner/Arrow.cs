using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Runner
{
   public class Arrow
   {
      public Vector2 Position { get; set; }
      public bool IsDead { get; set; }
      public Vector2 Target { get; set; }
      public Vector2 Velocity { get; set; }
      public Rectangle Bounds
      {
         get
         {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)GameUtil.spriteScale * 7, (int)GameUtil.spriteScale * 3);
         }
      }

      public Arrow(Vector2 target)
      {
         Position = new Vector2(GameUtil.playerX, GameUtil.playerY);
         Target = target;
         IsDead = false;
         CalculateVelocity();
      }

      public void Tick()
      {
         Position += Velocity * GameUtil.arrowSpeed;
      }

      private void CalculateVelocity()
      {
         float rise = Target.Y - GameUtil.playerY;
         float run = Target.X - GameUtil.playerX;
         Vector2 slope = new Vector2(run, rise);
         Velocity = Vector2.Normalize(slope);
      }
   }
}
