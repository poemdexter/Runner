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
        public Arrow(Vector2 target, Vector2 playerPosition)
        {

            int s_height = GameUtil.spriteDictionary["arrow"].Height;
            int s_width = GameUtil.spriteDictionary["arrow"].Width / GameUtil.arrow_frames;
            IsAlive = true;
            this.AddComponent(new Mobile(s_height, s_width,
                                        CalculatePosition(playerPosition, s_height),
                                        CalculateVelocity(target) * GameUtil.arrowSpeed));

            this.AddComponent(new Drawable("arrow", s_height, s_width, 0, 1, true, SpriteEffects.None));
        }

        private Vector2 CalculatePosition(Vector2 playerPosition, int spriteheight)
        {
            Rectangle playerSprite = GameUtil.spriteDictionary["player"].Bounds;
            return new Vector2(playerPosition.X + (playerSprite.Width / GameUtil.player_frames), 
                               playerPosition.Y + playerSprite.Center.Y - (spriteheight /2));
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
