using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Actions;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Runner.EntityFramework.Entities
{
    public class Cultist : Entity
    {
        public Cultist()
        {
            int s_height = GameUtil.spriteDictionary["cultist"].Height;
            int s_width = GameUtil.spriteDictionary["cultist"].Width / GameUtil.cultist_frames;

            this.AddComponent(new Mobile(s_height, s_width,
                                        new Vector2(GameUtil.windowWidth, GameUtil.groundY - s_height),
                                        new Vector2(-1, 0) * GameUtil.cultistSpeed));
            this.AddComponent(new Drawable("cultist", s_height, s_width, 0, 
                                            GameUtil.cultist_frames, true, SpriteEffects.None));
            this.AddComponent(new Hitpoints(GameUtil.cultistHP));
            this.AddAction(new NextFrameOfAnimation());
            this.AddAction(new TakeDamage());
            IsAlive = true;
        }
    }
}
