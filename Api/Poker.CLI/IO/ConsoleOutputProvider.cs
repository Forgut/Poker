using System;

namespace Poker.CLI.IO
{
    class ConsoleOutputProvider : IOutputProvider
    {
        public void Write(string str)
        {
            Console.Write(str);
        }

        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
    }
}
