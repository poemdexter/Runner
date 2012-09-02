using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;

namespace Runner
{
    public class Bat : Entity
    {
        public Bat()
        {
            this.AddComponent(new Mobile("bat", new Vector2(GameUtil.windowWidth, GameUtil.random.Next(100, 400)), new Vector2(-1, 0) * GameUtil.batSpeed));
            IsAlive = true;
        }
    }
}