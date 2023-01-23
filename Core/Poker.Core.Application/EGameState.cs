namespace Poker.Core.Application
{
    public enum EGameState
    {
        PreFlop = 0,
        PreFlopBet = 1,
        Flop = 2,
        FlopBet = 3,
        Turn = 4,
        TurnBet = 5,
        River = 6,
        RiverBet = 7,
        ShowCards = 8,
        End = 9,
    }
}
