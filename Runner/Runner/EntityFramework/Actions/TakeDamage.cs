using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Runner.EntityFramework.Args;

namespace Runner.EntityFramework.Actions
{
    class TakeDamage : EntityAction
    {
        public TakeDamage()
        {
            this.Name = "TakeDamage";
        }

        public override void Do(ActionArgs args)
        {
            Hitpoints hitpoints = this.Entity.GetComponent("Hitpoints") as Hitpoints;
            if (hitpoints != null)
            {
                hitpoints.HP -= ((SingleIntArgs)args).Amount;
                if (hitpoints.HP <= 0)
                {
                    this.Entity.IsAlive = false;
                }
            }
        }
    }
}
