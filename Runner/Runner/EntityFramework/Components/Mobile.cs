using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Microsoft.Xna.Framework;

namespace Runner.EntityFramework.Components
{
    class Mobile : Component
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int SpriteHeight { get; set; }
        public int SpriteWidth { get; set; }
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    SpriteWidth,
                    SpriteHeight);
            }
        }


        public Mobile(int sprite_h, int sprite_w, Vector2 position, Vector2 velocity)
        {
            this.Name = "Mobile";
            this.SpriteHeight = sprite_h;
            this.SpriteWidth = sprite_w;
            this.Position = position;
            this.Velocity = velocity;
        }

        public void Tick()
        {
            this.Position += this.Velocity;
        }
    }
}
