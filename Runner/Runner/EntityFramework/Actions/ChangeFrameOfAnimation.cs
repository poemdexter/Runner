using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Runner.EntityFramework.Args;
using Microsoft.Xna.Framework;

namespace Runner.EntityFramework.Actions
{
    class ChangeFrameOfAnimation : EntityAction
    {
        public ChangeFrameOfAnimation()
        {
            this.Name = "ChangeFrameOfAnimation";
        }

        public override void Do(ActionArgs args)
        {
            Drawable drawable = this.Entity.GetComponent("Drawable") as Drawable;
            if (drawable != null)
            {
                drawable.CurrentFrame = ((SingleIntArgs)args).Amount;
                drawable.SourceRect = new Rectangle(drawable.CurrentFrame * drawable.FrameWidth, 0, drawable.FrameWidth, drawable.FrameHeight);
            }
        }
    }
}
