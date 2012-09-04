using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Runner.EntityFramework.Components
{
    class Drawable : Component
    {
        public String SpriteName { get; set; }
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public int FrameHeight { get; set; }
        public int FrameWidth { get; set; }
        public Rectangle SourceRect { get; set; }
        public bool Looping { get; set; }
        public SpriteEffects Effects { get; set; }
        public bool Animated { get; set; }
        public int ElapsedTimeCounter { get; set; }
        public float Rotation { get; set; }

        public Drawable(String spriteName, int frameHeight, int frameWidth, float rotation, int frameCount, bool looping, SpriteEffects effects)
        {
            this.Name = "Drawable";
            this.Animated = (frameCount > 1) ? true : false;
            this.SpriteName = spriteName;
            this.FrameCount = frameCount;
            this.Effects = effects;
            this.Looping = looping;
            this.CurrentFrame = 0;
            this.FrameHeight = frameHeight;
            this.FrameWidth = frameWidth;
            this.Rotation = rotation;
            this.SourceRect = new Rectangle(this.CurrentFrame * this.FrameHeight, 0, this.FrameWidth, this.FrameHeight);
            this.ElapsedTimeCounter = 0;
        }
    }
}
