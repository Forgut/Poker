﻿using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour.WinCalculation;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core.Application.GameBehaviour
{
    public class GameSimulation
    {
        private readonly Table _table;
        private readonly Croupier _croupier;
        private readonly WinChanceEstimator _winChanceEstimator;
        private readonly PlayersInfo _playersInfo;
        private readonly WinDecision _winDecision;
        private readonly IEventPublisher _eventPublisher;

        public EGameState GameState { get; private set; }

        public GameSimulation(Croupier croupier,
                    CombinationComparer combinationComparer,
                    WinChanceEstimator winChanceEstimator,
                    Table table,
                    Players players,
                    IEventPublisher eventPublisher)
        {
            _croupier = croupier;
            _winChanceEstimator = winChanceEstimator;
            _table = table;
            _playersInfo = new PlayersInfo(players);
            _winDecision = new WinDecision(table, players, combinationComparer);
            _eventPublisher = eventPublisher;
        }

        public bool SetTargetPlayer(string playerName)
        {
            return _playersInfo.SetTargetPlayer(playerName);
        }

        public void ResetRound()
        {
            _table.ClearCards();
            foreach (var player in _playersInfo.Players)
                player.ClearCards();
            GameState = EGameState.New;
        }

        public void DrawPlayerCards()
        {
            _croupier.PreFlop(_playersInfo.Players);
            GameState = EGameState.PreFlop;
        }

        public void Flop()
        {
            _croupier.Flop(_table);
            GameState = EGameState.Flop;
        }

        public void Turn()
        {
            _croupier.Turn(_table);
            GameState = EGameState.Turn;
        }

        public void River()
        {
            _croupier.River(_table);
            GameState = EGameState.River;
        }

        public void PrintGameState()
        {
            Console.WriteLine();
            var sb = new StringBuilder();
            sb.AppendLine($"Current stage: {GameState}");
            sb.Append("Table: ");
            foreach (var card in _table.Cards)
            {
                if (card == null)
                    sb.Append("X ");
                else
                    sb.Append($"{card} ");
            }
            sb.AppendLine();

            sb.AppendLine("Players:");
            foreach (var player in _playersInfo.Players)
                sb.AppendLine($"{player.Name}: {player.Cards[0]} {player.Cards[1]}");

            Console.WriteLine(sb.ToString());
        }

        public void PrintWinProbabilities()
        {
            switch (GameState)
            {
                case EGameState.New:
                    break;
                case EGameState.PreFlop:
                    break;
                case EGameState.Flop:
                    PrintFlop();
                    break;
                case EGameState.Turn:
                    PrintTurn();
                    break;
                case EGameState.River:
                case EGameState.End:
                    PrintRiver();
                    break;
            }

            void PrintFlop()
            {
                if (_playersInfo.TargetPlayer == null)
                {
                    Console.WriteLine("Target player not specified");
                    return;
                }

                var flopProb = _winChanceEstimator
                        .ProbableCombinationsForPlayer2Missing(_table, _playersInfo.TargetPlayer);

                var flopEnemyProb = _winChanceEstimator
                    .ProbableCombinationsForEnemy2Missing(_table, _playersInfo.TargetPlayer);

                Console.WriteLine($"Flop probability for {_playersInfo.TargetPlayer.Name}");
                Console.WriteLine(flopProb);
                Console.WriteLine();

                Console.WriteLine($"Flop probability for {_playersInfo.TargetPlayer.Name} enemies");
                Console.WriteLine(flopEnemyProb);
                Console.WriteLine();
            }

            void PrintTurn()
            {
                if (_playersInfo.TargetPlayer == null)
                {
                    Console.WriteLine("Target player not specified");
                    return;
                }

                var turnProb = _winChanceEstimator
                .ProbableCombinationsForPlayer1Missing(_table, _playersInfo.TargetPlayer);

                var turnEnemyProb = _winChanceEstimator
                    .ProbableCombinationsForEnemy1Missing(_table, _playersInfo.TargetPlayer);

                Console.WriteLine($"Turn probability for {_playersInfo.TargetPlayer.Name}");
                Console.WriteLine(turnProb);
                Console.WriteLine();

                Console.WriteLine($"Turn probability for {_playersInfo.TargetPlayer.Name} enemies");
                Console.WriteLine(turnEnemyProb);
                Console.WriteLine();
            }

            void PrintRiver()
            {
                var playersCombinations =
                _playersInfo.Players.Select(x => new
                {
                    Player = x,
                    Combination = new CombinationFinder(x, _table).GetBestCombination()
                });

                foreach (var playerCombination in playersCombinations)
                    Console.WriteLine($"{playerCombination.Player.Name}: {playerCombination.Combination}");
            }
        }

        public void PrintWinner()
        {
            Console.WriteLine("Winners:");
            foreach (var winner in _winDecision.Winners)
                Console.Write($"{winner.Name};");
            Console.WriteLine();
        }

        public void EndRound()
        {
            var winners = _winDecision.Winners.ToList();

            PublishEvent(winners);
            GameState = EGameState.End;

            void PublishEvent(IEnumerable<Winner> winners)
            {
                var eventParameters = winners
                    .Select(x => new WinnerInfo(x.Name, x.Combination));
                var @event = new RoundEndedEvent(eventParameters);
                _eventPublisher.RoundEnded(@event);
            }
        }
    }
}
