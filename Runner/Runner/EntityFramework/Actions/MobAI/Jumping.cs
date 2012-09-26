using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework;

namespace Runner.EntityFramework.Actions.MobAI
{
    class Jumping : Mobile
    {
        bool Jump { get; set; }
        bool Down { get; set; }
        Vector2 OrigVelocity { get; set; }

        public Jumping(int sprite_h, int sprite_w, Vector2 position, Vector2 velocity)
        {
            this.Name = "Jumping";
            this.SpriteHeight = sprite_h;
            this.SpriteWidth = sprite_w;
            this.Position = position;
            this.Velocity = velocity;
            OrigVelocity = velocity;
        }

        public new void Tick()
        {
            // if not forcing down nor jumping, lets jump
            if (!Jump && !Down)
            {
                Jump = true;
                this.Velocity = new Vector2(0, GameUtil.JumpPower);
            }

            // we're still going up!
            if (Jump && !Down)
            {
                // hit max height?
                if (this.Position.Y <= GameUtil.playerY - GameUtil.maxJumpHeight)
                {
                    Down = true;
                    this.Velocity += new Vector2(0, GameUtil.JumpFriction);
                }
            }

            // we need to stop falling when we hit ground.
            if (Down)
            {
                if (this.Velocity.Y < GameUtil.FallPower)
                {
                    this.Velocity += new Vector2(0, GameUtil.JumpFriction);
                }

                if (this.Position.Y >= GameUtil.playerY)
                {
                    Vector2 standPosition = this.Position;
                    standPosition.Y = GameUtil.playerY;
                    this.Position = standPosition;
                    Down = false;
                    Jump = false;
                }
            }

            this.Position += this.Velocity + OrigVelocity;
        }
    }
}
