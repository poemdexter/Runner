using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Components;
using Runner.EntityFramework.Args;

namespace Runner.EntityFramework.Actions.MoveAI
{
    class Jumping : EntityAction
    {
        bool Jump { get; set; }
        bool Down { get; set; }
        int ElapsedTime { get; set; }

        public Jumping()
        {
            this.Name = "Jumping";
            ElapsedTime = 0;
        }

        public override void Do(ActionArgs args)
        {
            // Do Jumping
            Mobile mobile = this.Entity.GetComponent("Mobile") as Mobile;
            ElapsedTime += ((SingleIntArgs)args).Amount;

            // if not forcing down nor jumping, lets jump
            if (!Jump && !Down && ElapsedTime >= (1 * 1000))
            {
                Jump = true;
                mobile.Velocity = new Vector2(mobile.Velocity.X, GameUtil.JumpPower);
                this.Entity.DoAction("ChangeFrameOfAnimation", new SingleIntArgs(1));
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
                    standPosition.Y = GameUtil.groundY - mobile.SpriteHeight;
                    mobile.Position = standPosition;
                    mobile.Velocity = new Vector2(mobile.Velocity.X, 0);
                    Down = false;
                    Jump = false;
                    ElapsedTime = 0;
                    this.Entity.DoAction("ChangeFrameOfAnimation", new SingleIntArgs(0));
                }
            }

            mobile.Position += mobile.Velocity;
        }
    }
}
