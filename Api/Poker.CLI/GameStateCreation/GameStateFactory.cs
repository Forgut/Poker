using Poker.CLI.Common;
using Poker.CLI.Simulation;
using Poker.CLI.Standard;
using Poker.Core.Application.CardBehaviour.Shuffling;
using Poker.Core.Application.CardBehaviour;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application;
using System.Collections.Generic;
using System;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Extensions;
using System.Linq;
using Poker.Core.Application.Events;
using Poker.Infrastructure.Services.Events;
using Poker.Core.Application.GameBehaviour;

namespace Poker.CLI.GameStateCreation
{
    class GameStateFactory : ISetSimulationStage,
        ISetCroupierStage,
        ISetTableStage,
        ISetWinEstimatorStage,
        ISetCombinationComparerStage,
        ISetPlayersStage,
        ISetEventPublisherStage,
        IBuildGameStateStage
    {
        private Table _table;
        private Croupier _croupier;
        private WinEstimator _winEstimator;
        private CombinationComparer _combinationComparer;
        private Players _players;
        private IEventPublisher _eventPublisher;
        private bool _isSimulation;


        public IGameState Build()
        {
            if (_isSimulation)
                return new GameSimulationState(BuildGameSimulation());
            return new GameState(BuildGame());

            GameSimulation BuildGameSimulation()
                => new GameSimulation(_croupier, _combinationComparer, _winEstimator, _table, _players, _eventPublisher);

            Game BuildGame()
                => new Game(_combinationComparer, _winEstimator, _table, _players, _eventPublisher);
        }

        private GameStateFactory()
        {

        }

        public static ISetSimulationStage Factory
            => new GameStateFactory();

        public ISetCroupierStage GetGameSimulation()
        {
            _isSimulation = true;
            return this;
        }

        public ISetTableStage GetStandardGame()
        {
            _isSimulation = false;
            return this;
        }

        public ISetTableStage WithDefaultCroupier()
        {
            _croupier = new Croupier(Deck.NewDeck.Shuffled(new ShuffleRule(new Random())));
            return this;
        }

        public ISetPlayersStage WithDefaultCombinationComparer()
        {
            _combinationComparer = new CombinationComparer();
            return this;
        }

        public ISetWinEstimatorStage WithDefaultTable()
        {
            _table = new Table();
            return this;
        }

        public ISetCombinationComparerStage WithDefaultWinEstimator()
        {
            _winEstimator = new WinEstimator();
            return this;
        }

        public ISetEventPublisherStage WithPlayers(IEnumerable<Player> players)
        {
            _players = players.ToPlayers();
            return this;
        }
        public ISetEventPublisherStage WithRandomPlayers(int numberOfplayers = 4)
        {
            _players = new Players();
            foreach (var i in Enumerable.Range(0, numberOfplayers))
                _players.Add(new Player($"Player {i + 1}"));
            return this;
        }

        public IBuildGameStateStage WithNoEventPublisher()
        {
            _eventPublisher = new EmptyEventPublisher();
            return this;
        }

        public IBuildGameStateStage WithEventPublisher(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            return this;
        }
    }
}
