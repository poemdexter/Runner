using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;

namespace Runner.EntityFramework.Components
{
    class Hitpoints : Component
    {
        public int HP { get; set; }

        public Hitpoints(int hp)
        {
            this.Name = "Hitpoints";
            this.HP = hp;
        }
    }
}
