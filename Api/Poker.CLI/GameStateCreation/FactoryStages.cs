using Poker.CLI.Common;
using Poker.Core.Domain.Entity;
using System.Collections.Generic;

namespace Poker.CLI.GameStateCreation
{
    interface ISetSimulationStage
    {
        ISetCroupierStage GetGameSimulation();
        ISetTableStage GetStandardGame();
    }

    interface ISetCroupierStage
    {
        ISetTableStage WithDefaultCroupier();
    }

    interface ISetTableStage
    {
        ISetWinEstimatorStage WithDefaultTable();
    }

    interface ISetWinEstimatorStage
    {
        ISetCombinationComparerStage WithDefaultWinEstimator();
    }

    interface ISetCombinationComparerStage
    {
        ISetPlayersStage WithDefaultCombinationComparer();
    }

    interface ISetPlayersStage
    {
        IBuildGameStateStage WithPlayers(IEnumerable<Player> players);
    }

    interface IBuildGameStateStage
    {
        IGameState Build();
    }
}
