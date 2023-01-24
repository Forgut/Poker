namespace Poker.CLI.IO
{
    public interface IOutputProvider
    {
        void Write(string str);
        void WriteLine(string str);
    }
}
