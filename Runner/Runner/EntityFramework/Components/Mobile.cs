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
        public String SpriteName { get; set; }
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    GameUtil.spriteDictionary[SpriteName].Width,
                    GameUtil.spriteDictionary[SpriteName].Height);
            }
        }


        public Mobile(String spriteName, Vector2 position, Vector2 velocity)
        {
            this.Name = "Mobile";
            this.SpriteName = spriteName;
            this.Position = position;
            this.Velocity = velocity;
        }

        public void Tick()
        {
            this.Position += this.Velocity;
        }
    }
}
