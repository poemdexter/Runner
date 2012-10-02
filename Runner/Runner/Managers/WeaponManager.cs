using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Runner.Managers
{
    public static class WeaponManager
    {
        public static Vector2 CalculatePosition(Vector2 playerPosition, int spriteheight)
        {
            Rectangle playerSprite = GameUtil.spriteDictionary["player"].Bounds;
            return new Vector2(playerPosition.X + (playerSprite.Width / GameUtil.player_frames),
                               playerPosition.Y + playerSprite.Center.Y - (spriteheight / 2));
        }

        public static Vector2 CalculateVelocity(Vector2 mousePosition, Vector2 spawnPoint)
        {
            float rise = mousePosition.Y - spawnPoint.Y;
            float run = mousePosition.X - spawnPoint.X;
            Vector2 slope = new Vector2(run, rise);
            return Vector2.Normalize(slope);
        }
    }
}
