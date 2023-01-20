using Poker.CLI.Common;
using Poker.Core.Application.Events;
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
        ISetEventPublisherStage WithPlayers(IEnumerable<Player> players);
        ISetEventPublisherStage WithRandomPlayers(int numberOfplayers = 4);
    }

    interface ISetEventPublisherStage
    {
        IBuildGameStateStage WithNoEventPublisher();
        IBuildGameStateStage WithEventPublisher(IEventPublisher eventPublisher);
    }

    interface IBuildGameStateStage
    {
        IGameState Build();
    }
}
