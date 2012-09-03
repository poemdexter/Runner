using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Runner
{
    public static class GameUtil
    {
        public const String VERSION = "0.0.2";

        public static int windowHeight = 720;
        public static int windowWidth = 1280;
        public static float fontScale = 2f;

        public static int playerX = 100;
        public static int playerY = 500;

        public static int arrowSpeed = 15;
        public static int arrowDelay = 300;
        public static int batSpeed = 6;

        public static Random random = new Random();
        public static IDictionary<String, Texture2D> spriteDictionary = new Dictionary<String, Texture2D>();

        // Animation Frames for each Image
        public static int bat_frames = 1;
        public static int arrow_frames = 1;
        public static int player_frames = 2;

        // Animation timing
        public static int playerAnimationLength = 200;

        public static void loadSprites(ContentManager Content)
        {
            spriteDictionary.Add("bat", Content.Load<Texture2D>("mobs/bat"));
            spriteDictionary.Add("arrow", Content.Load<Texture2D>("entities/arrow"));
            spriteDictionary.Add("ground", Content.Load<Texture2D>("environment/ground"));
            spriteDictionary.Add("background", Content.Load<Texture2D>("environment/background"));
            spriteDictionary.Add("player", Content.Load<Texture2D>("player/hunter_ani"));
        }
    }
}
