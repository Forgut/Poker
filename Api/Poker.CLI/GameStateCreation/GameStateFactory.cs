﻿using Poker.CLI.Common;
using Poker.CLI.IO;
using Poker.CLI.Simulation;
using Poker.CLI.Standard;
using Poker.Core.Application;
using Poker.Core.Application.Betting;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Extensions;
using Poker.Infrastructure.Services.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Poker.CLI.GameStateCreation
{
    class GameStateFactory : ISetSimulationStage,
        ISetCroupierStage,
        ISetTableStage,
        ISetWinEstimatorStage,
        ISetCombinationComparerStage,
        ISetPlayersStage,
        ISetEventPublisherStage,
        ICombinationFinderStage,
        IBuildGameStateStage
    {
        private ITable _table;
        private ICroupier _croupier;
        private IWinChanceEstimator _winChanceEstimator;
        private ICombinationComparer _combinationComparer;
        private ICombinationFinder _combinationFinder;
        private Players _players;
        private IEventPublisher _eventPublisher;
        private bool _isSimulation;


        public IGameState Build()
        {
            if (_isSimulation)
                return new GameSimulationState(BuildGameSimulation());
            return new GameState(BuildGame(), new ConsoleInputProvider(), new ConsoleOutputProvider());

            SimulationGame BuildGameSimulation()
                => new SimulationGame(_croupier, _combinationComparer, _winChanceEstimator, _table, _players, _eventPublisher, _combinationFinder);

            StandardGame BuildGame()
                => new StandardGame(_combinationComparer, _winChanceEstimator, _table, _players, _eventPublisher, new BetOverseer(_players), _combinationFinder);
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
            _croupier = new Croupier(new Random());
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
            _winChanceEstimator = new WinChanceEstimator(_combinationFinder);
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
                _players.Add(new Player($"Player {i + 1}", 100));
            return this;
        }

        public ICombinationFinderStage WithNoEventPublisher()
        {
            _eventPublisher = new EmptyEventPublisher();
            return this;
        }

        public ICombinationFinderStage WithEventPublisher(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            return this;
        }

        public IBuildGameStateStage WithDefaultCombinationFinder()
        {
            _combinationFinder = new CombinationFinder();
            return this;
        }
    }
}
