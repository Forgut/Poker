using System;

namespace Poker.CLI.Input
{
    class ConsoleInputProvider : IInputProivder
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
