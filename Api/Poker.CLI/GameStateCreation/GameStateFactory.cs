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

namespace Poker.CLI.GameStateCreation
{
    class GameStateFactory : ISetSimulationStage,
        ISetCroupierStage,
        ISetTableStage,
        ISetWinEstimatorStage,
        ISetCombinationComparerStage,
        ISetPlayersStage,
        IBuildGameStateStage
    {
        private Table _table;
        private Croupier _croupier;
        private WinEstimator _winEstimator;
        private CombinationComparer _combinationComparer;
        private Players _players;
        private bool _isSimulation;


        public IGameState Build()
        {
            if (_isSimulation)
                return new GameSimulationState(BuildGameSimulation());
            return new GameState(BuildGame());

            GameSimulation BuildGameSimulation()
                => new GameSimulation(_croupier, _combinationComparer, _winEstimator, _table, _players);

            Game BuildGame()
                => new Game(_combinationComparer, _winEstimator, _table, _players);
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

        public IBuildGameStateStage WithPlayers(IEnumerable<Player> players)
        {
            _players = players.ToPlayers();
            return this;
        }
    }
}
