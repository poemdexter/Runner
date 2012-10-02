using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Entities;

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
            return new Vector2(run, rise);
        }

        public static List<Arrow> MultiShot(Vector2 arrowSpawnPosition, Vector2 arrowVelocity)
        {
            List<Arrow> shotList = new List<Arrow>();
            // need this for spread
            float length = arrowVelocity.Length();

            // length can only be .1 - .5
            length = Math.Max(100, length);
            length = Math.Min(length, 500);
            length = length / 1000;
            Console.Write("length:{0}", length);

            // get perpendicular normals to velocity
            Vector2 arrowNormalVelocity = Vector2.Normalize(arrowVelocity);
            Vector2 norm1 = new Vector2(arrowNormalVelocity.Y, -arrowNormalVelocity.X);
            Vector2 norm2 = new Vector2(-arrowNormalVelocity.Y, arrowNormalVelocity.X);
            norm1 = norm1 * (float)((.6 - length) * GameUtil.multishot_spread);
            norm2 = norm2 * (float)((.6 - length) * GameUtil.multishot_spread);
            
            shotList.Add(new Arrow(arrowSpawnPosition, Vector2.Normalize(arrowVelocity) + norm1));
            shotList.Add(new Arrow(arrowSpawnPosition, Vector2.Normalize(arrowVelocity) + norm2));
            shotList.Add(new Arrow(arrowSpawnPosition, Vector2.Normalize(arrowVelocity)));

            return shotList;
        }
    }
}
