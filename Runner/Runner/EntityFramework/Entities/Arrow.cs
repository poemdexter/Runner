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
    public class Arrow : Entity
    {
        public Arrow(Vector2 target)
        {
            int s_height = GameUtil.spriteDictionary["arrow"].Height;
            int s_width = GameUtil.spriteDictionary["arrow"].Width / GameUtil.arrow_frames;
            IsAlive = true;
            this.AddComponent(new Mobile(s_height, s_width, 
                                        new Vector2(GameUtil.playerX, GameUtil.playerY), 
                                        CalculateVelocity(target) * GameUtil.arrowSpeed));

            this.AddComponent(new Drawable("arrow", s_height, s_width, 2, true, SpriteEffects.None));
        }

        private Vector2 CalculateVelocity(Vector2 target)
        {
            float rise = target.Y - GameUtil.playerY;
            float run = target.X - GameUtil.playerX;
            Vector2 slope = new Vector2(run, rise);
            return Vector2.Normalize(slope);
        }
    }
}
