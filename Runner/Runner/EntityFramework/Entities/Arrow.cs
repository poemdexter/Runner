using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework.Graphics;
using Runner.EntityFramework.Actions;

namespace Runner.EntityFramework.Entities
{
    public class Arrow : Entity
    {
        public Arrow(Vector2 position, Vector2 velocity)
        {

            int s_height = GameUtil.spriteDictionary["arrow"].Height;
            int s_width = GameUtil.spriteDictionary["arrow"].Width / GameUtil.arrow_frames;
            IsAlive = true;
            this.AddComponent(new Mobile(s_height, s_width, position, velocity * GameUtil.arrowSpeed, GameUtil.arrowDmg));
            this.AddAction(new Move("none"));
            this.AddComponent(new Drawable("arrow", s_height, s_width, 0, 1, true, SpriteEffects.None));
        }
    }
}
