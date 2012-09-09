using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runner.EntityFramework.Framework;

namespace Runner.EntityFramework.Args
{
    class SingleIntArgs : ActionArgs
    {
        public int Amount { get; set; }

        public SingleIntArgs(int amt)
        {
            this.Amount = amt;
        }
    }
}
