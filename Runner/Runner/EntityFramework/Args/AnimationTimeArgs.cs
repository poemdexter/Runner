using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;

namespace Runner.EntityFramework.Args
{
    class AnimationTimeArgs : ActionArgs
    {
        public int Delta { get; set; }

        public AnimationTimeArgs(int delta)
        {
            this.Delta = delta;
        }
    }
}
