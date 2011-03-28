using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux
{
    public static class IntGen
    {
        static uint i = 0;

        public static uint GetNewGUID()
        {
            i++;
            return i;
        }
    }
}
