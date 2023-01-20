using System;
using System.Collections.Generic;
using System.Text;

namespace Poker.Core.Domain.Exceptions
{
    public class WrongCardValueException : Exception
    {
        public WrongCardValueException(string input)
            : base($"Provided value ({input}) cannot be used to create card")
        {

        }
    }
}
