using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Runner
{
   public class Arrow
   {
      public float X { get; set; }
      public float Y { get; set; }
      public bool IsDead { get; set; }

      public Arrow(float x, float y)
      {
         this.X = x;
         this.Y = y;
         IsDead = false;
      }

      public void Tick()
      {
         this.X += 5;
      }
   }
}
