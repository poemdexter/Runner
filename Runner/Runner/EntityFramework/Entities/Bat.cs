using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework.Graphics;

namespace Runner.EntityFramework.Entities
{
    public class Bat : Entity
    {
        public Bat()
        {
            int s_height = GameUtil.spriteDictionary["bat"].Height;
            int s_width = GameUtil.spriteDictionary["bat"].Width / GameUtil.bat_frames;
            this.AddComponent(new Mobile(s_height, s_width, 
                                        new Vector2(GameUtil.windowWidth, 
                                        GameUtil.random.Next(100, 400)), 
                                        new Vector2(-1, 0) * GameUtil.batSpeed));

            this.AddComponent(new Drawable("bat", s_height, s_width, 2, true, SpriteEffects.None));
            IsAlive = true;
        }
    }
}