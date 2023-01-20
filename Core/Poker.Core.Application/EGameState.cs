﻿namespace Poker.Core.Application
{
    public enum EGameState
    {
        New = 0,
        PreFlop = 1,
        Flop = 2,
        Turn = 3,
        River = 4,
        ShowCards = 5,
        End = 6,
    }
}