using System;

namespace Poker.CLI.IO
{
    class ConsoleInputProvider : IInputProivder
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
