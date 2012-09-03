using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;
using Runner.EntityFramework.Components;
using Microsoft.Xna.Framework;
using Runner.EntityFramework.Args;

namespace Runner.EntityFramework.Actions
{
    class NextFrameOfAnimation : EntityAction
    {
        public NextFrameOfAnimation()
        {
            this.Name = "NextFrameOfAnimation";
        }

        public override void Do(ActionArgs args)
        {
            Drawable drawable = this.Entity.GetComponent("Drawable") as Drawable;
            if (drawable != null)
            {
                drawable.ElapsedTimeCounter += ((AnimationTimeArgs)args).Delta;
                if (drawable.ElapsedTimeCounter > GameUtil.playerAnimationLength)
                {
                    if (drawable.Looping)
                    {
                        drawable.CurrentFrame++;
                        // if we have 2 frames, they count as 0 and 1.  2 is out of bounds.
                        if (drawable.CurrentFrame == drawable.FrameCount) { drawable.CurrentFrame = 0; }
                    }
                    else
                    {
                        // make sure we haven't hit the end of drawable
                        if (drawable.CurrentFrame + 1 != drawable.FrameCount) { drawable.CurrentFrame++; }
                    }
                    drawable.SourceRect = new Rectangle(drawable.CurrentFrame * drawable.FrameHeight, 0, drawable.FrameHeight, drawable.FrameHeight);
                    drawable.ElapsedTimeCounter = 0;
                }
            }
        }
    }
}
