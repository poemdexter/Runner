﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Runner.EntityFramework.Actions;

namespace Runner.EntityFramework.Entities
{
    class Player : Entity
    {
        public bool Jumping { get; set; }
        public bool ForceDown { get; set; }

        public Player()
        {
            Jumping = false;
            ForceDown = false;
            int s_height = GameUtil.spriteDictionary["player"].Height;
            int s_width = GameUtil.spriteDictionary["player"].Width / GameUtil.player_frames;
            this.AddComponent(new Mobile(s_height, s_width,
                                        new Vector2(GameUtil.playerX, GameUtil.groundY - s_height),
                                        Vector2.Zero, 0));
            this.AddAction(new Move("none"));
            this.AddComponent(new Drawable("player", s_height, s_width, 0, 2, true, SpriteEffects.None));
            this.AddAction(new NextFrameOfAnimation());
            this.AddComponent(new Hitpoints(GameUtil.playerHP));
            this.AddAction(new TakeDamage());
            IsAlive = true;
        }
    }
}
