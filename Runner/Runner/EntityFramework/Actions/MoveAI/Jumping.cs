using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Components;

namespace Runner.EntityFramework.Actions.MoveAI
{
    class Jumping : EntityAction
    {
        bool Jump { get; set; }
        bool Down { get; set; }

        public Jumping()
        {
            this.Name = "Jumping";
        }

        public override void Do()
        {
            Mobile mobile = this.Entity.GetComponent("Mobile") as Mobile;

            // if not forcing down nor jumping, lets jump
            if (!Jump && !Down)
            {
                Jump = true;
                mobile.Velocity = new Vector2(mobile.Velocity.X, GameUtil.JumpPower);
            }

            // we're still going up!
            if (Jump && !Down)
            {
                // hit max height?
                if (mobile.Position.Y <= GameUtil.playerY - GameUtil.maxJumpHeight)
                {
                    Down = true;
                    mobile.Velocity += new Vector2(0, GameUtil.JumpFriction);
                }
            }

            // we need to stop falling when we hit ground.
            if (Down)
            {
                if (mobile.Velocity.Y < GameUtil.FallPower)
                {
                    mobile.Velocity += new Vector2(0, GameUtil.JumpFriction);
                }

                if (mobile.Position.Y >= GameUtil.playerY)
                {
                    Vector2 standPosition = mobile.Position;
                    standPosition.Y = GameUtil.playerY;
                    mobile.Position = standPosition;
                    Down = false;
                    Jump = false;
                }
            }

            mobile.Position += mobile.Velocity;
        }
    }
}
