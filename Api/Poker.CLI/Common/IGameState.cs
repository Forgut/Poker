namespace Poker.CLI.Common
{
    interface IGameState
    {
        bool ShouldEndGame { get; }
        void ExecuteAction(string action);
    }
}
