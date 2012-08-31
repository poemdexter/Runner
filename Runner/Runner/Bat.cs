using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Runner
{
   public class Bat
   {
      public Vector2 Position { get; set; }
      public bool IsDead { get; set; }
      public Vector2 Velocity { get; set; }
      public Rectangle Bounds 
      { 
         get 
         {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)GameUtil.spriteScale * 8, (int)GameUtil.spriteScale * 8);
         } 
      }

      public Bat()
      {
         Position = new Vector2(GameUtil.windowWidth, GameUtil.random.Next(100,400));
         IsDead = false;
      }

      public void Tick()
      {
         Position += new Vector2(-1,0) * GameUtil.batSpeed;
      }
   }
}
