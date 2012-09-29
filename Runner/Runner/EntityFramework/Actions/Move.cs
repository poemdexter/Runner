using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;

namespace Runner.EntityFramework.Actions
{
    class Move : EntityAction
    {
        String AIName { get; set; }
        public Move(String moveAIName)
        {
            this.Name = "Move";
            this.AIName = moveAIName;
        }

        public override void Do()
        {
            if (AIName != null && AIName != "" && AIName != "none")
            {
                this.Entity.DoAction(AIName);
            }
            else
            {
                // move at mob specific speed
                Mobile mobile = this.Entity.GetComponent("Mobile") as Mobile;
                mobile.Position += mobile.Velocity;
            }
        }

        public override void Do(ActionArgs args)
        {
            if (AIName != null && AIName != "" && AIName != "none")
            {
                this.Entity.DoAction(AIName, args);
            }
            else
            {
                // move at mob specific speed
                Mobile mobile = this.Entity.GetComponent("Mobile") as Mobile;
                mobile.Position += mobile.Velocity;
            }
        }
    }
}
