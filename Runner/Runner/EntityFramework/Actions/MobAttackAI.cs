using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;

namespace Runner.EntityFramework.Actions
{
    class MobAttackAI : EntityAction
    {
        String AIName { get; set; }
        public MobAttackAI(String attackAIName)
        {
            this.Name = "MobAttackAI";
            this.AIName = attackAIName;
        }

        public override void Do()
        {
            if (AIName != null && AIName != "")
            {
                this.Entity.DoAction(AIName);
            }
        }
    }
}
