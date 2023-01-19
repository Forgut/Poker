using System;

namespace Poker.Core.Domain.Exceptions
{
    public class CombinationNotFoundException : Exception
    {
        public CombinationNotFoundException() :
            base("None combination was found - this shouldn't occure")
        {

        }
    }
}
