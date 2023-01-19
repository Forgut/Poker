using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Logic.Exceptions
{
    internal class CombinationNotFoundException : Exception
    {
        public CombinationNotFoundException() :
            base("None combination was found - this shouldn't occure")
        {

        }
    }
}
