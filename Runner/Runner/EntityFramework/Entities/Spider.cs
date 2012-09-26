﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Runner.EntityFramework.Actions;
using Runner.EntityFramework.Actions.MobAI;

namespace Runner.EntityFramework.Entities
{
    class Spider : Entity
    {
        public Spider()
        {
            int s_height = GameUtil.spriteDictionary["spider"].Height;
            int s_width = GameUtil.spriteDictionary["spider"].Width / GameUtil.spider_frames;

            this.AddComponent(new Jumping(s_height, s_width,
                                        new Vector2(GameUtil.windowWidth, GameUtil.groundY - s_height),
                                        new Vector2(-1, 0) * GameUtil.spiderSpeed));
            this.AddComponent(new Drawable("spider", s_height, s_width, 0, 
                                            GameUtil.spider_frames, true, SpriteEffects.None));
            this.AddComponent(new Hitpoints(GameUtil.spiderHP));
            this.AddAction(new TakeDamage());
            IsAlive = true;
        }
    }
}
