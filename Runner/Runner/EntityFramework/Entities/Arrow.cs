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
        public Vector2 Origin { get; set; }

        public Arrow(Vector2 target, Vector2 playerPosition)
        {

            int s_height = GameUtil.spriteDictionary["arrow"].Height;
            int s_width = GameUtil.spriteDictionary["arrow"].Width / GameUtil.arrow_frames;
            IsAlive = true;
            this.AddComponent(new Mobile(s_height, s_width,
                                        CalculatePosition(playerPosition),
                                        CalculateVelocity(target) * GameUtil.arrowSpeed));

            this.AddComponent(new Drawable("arrow", s_height, s_width, CalculateRotation(target), 1, true, SpriteEffects.None));
            this.Origin = CalculateOrigin();
        }

        private Vector2 CalculatePosition(Vector2 playerPosition)
        {
            Rectangle playerSprite = GameUtil.spriteDictionary["player"].Bounds;

            return new Vector2(playerPosition.X + (playerSprite.Width / GameUtil.player_frames),
                               playerPosition.Y + playerSprite.Center.Y - ((GameUtil.spriteDictionary["arrow"].Width / GameUtil.arrow_frames) / 2));
        }

        private Vector2 CalculateVelocity(Vector2 target)
        {
            float rise = target.Y - GameUtil.playerY;
            float run = target.X - GameUtil.playerX;
            Vector2 slope = new Vector2(run, rise);
            return Vector2.Normalize(slope);
        }

        private float CalculateRotation(Vector2 target)
        {
            double coef = ((target.X - GameUtil.playerX) > 0) ? 0 : (Math.PI);
            float gradient = (target.Y - GameUtil.playerY) / (target.X - GameUtil.playerX);
            return (float)(coef + Math.Atan(gradient));
        }

        private Vector2 CalculateOrigin()
        {
            Rectangle arrowSprite = GameUtil.spriteDictionary["arrow"].Bounds;
            return new Vector2(arrowSprite.Width / 2, arrowSprite.Height / 2);
        }
    }
}
