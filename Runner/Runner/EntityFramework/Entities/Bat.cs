using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework.Graphics;
using Runner.EntityFramework.Actions;
using Runner.EntityFramework.Actions.AttackAI;

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
                                        new Vector2(-1, 0) * GameUtil.batSpeed, GameUtil.batDmg));
            this.AddAction(new Move("none"));
            this.AddComponent(new Drawable("bat", s_height, s_width, 0, 
                                            GameUtil.bat_frames, true, SpriteEffects.None));
            this.AddComponent(new Hitpoints(GameUtil.batHP));
            this.AddAction(new TakeDamage());
            this.AddAction(new NextFrameOfAnimation());
            this.AddAction(new MobAttackAI("BatAttack"));
            this.AddAction(new BatAttack());
            IsAlive = true;
        }
    }
}