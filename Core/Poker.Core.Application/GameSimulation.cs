﻿using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core.Application
{
    public class GameSimulation
    {
        private readonly Table _table;
        private readonly Croupier _croupier;
        private readonly CombinationComparer _combinationComparer;
        private readonly WinEstimator _winEstimator;
        private readonly Players _players;

        private Player? _targetPlayer;

        public List<Player> Winners { get; private set; }
        public EGameState GameState { get; private set; }

        public GameSimulation(Croupier croupier,
                    CombinationComparer combinationComparer,
                    WinEstimator winEstimator,
                    Table table,
                    Players players)
        {
            _croupier = croupier;
            _combinationComparer = combinationComparer;
            _winEstimator = winEstimator;
            _table = table;
            Winners = new List<Player>();
            _players = players;
        }

        public bool SetTargetPlayer(string playerName)
        {
            var player = _players.FirstOrDefault(x => x.Name == playerName);
            if (player != null)
            {
                _targetPlayer = player;
                return true;
            }

            _targetPlayer = _players.First();
            return false;
        }

        public void ResetRound()
        {
            _table.ClearCards();
            foreach (var player in _players)
                player.ClearCards();
            Winners.Clear();
            GameState = EGameState.New;
        }

        public void DrawPlayerCards()
        {
            _croupier.PreFlop(_players);
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
            foreach (var player in _players)
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
                    PrintRiver();
                    break;
                case EGameState.End:
                    break;
            }

            void PrintFlop()
            {
                if (_targetPlayer == null)
                {
                    Console.WriteLine("Target player not specified");
                    return;
                }

                var flopProb = _winEstimator
                        .ProbableCombinationsForPlayer2Missing(_table, _targetPlayer);

                var flopEnemyProb = _winEstimator
                    .ProbableCombinationsForEnemy2Missing(_table, _targetPlayer);

                Console.WriteLine($"Flop probability for {_targetPlayer.Name}");
                Console.WriteLine(flopProb);
                Console.WriteLine();

                Console.WriteLine($"Flop probability for {_targetPlayer.Name} enemies");
                Console.WriteLine(flopEnemyProb);
                Console.WriteLine();
            }

            void PrintTurn()
            {
                if (_targetPlayer == null)
                {
                    Console.WriteLine("Target player not specified");
                    return;
                }

                var turnProb = _winEstimator
                .ProbableCombinationsForPlayer1Missing(_table, _targetPlayer);

                var turnEnemyProb = _winEstimator
                    .ProbableCombinationsForEnemy1Missing(_table, _targetPlayer);

                Console.WriteLine($"Turn probability for {_targetPlayer.Name}");
                Console.WriteLine(turnProb);
                Console.WriteLine();

                Console.WriteLine($"Turn probability for {_targetPlayer.Name} enemies");
                Console.WriteLine(turnEnemyProb);
                Console.WriteLine();
            }

            void PrintRiver()
            {
                var playersCombinations =
                _players.Select(x => new
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
            foreach (var winner in Winners)
                Console.Write($"{winner.Name};");
            Console.WriteLine();
        }

        public void EndRound()
        {
            var playersCombinations =
                _players.Select(x => new
                {
                    Player = x,
                    Combination = new CombinationFinder(x, _table).GetBestCombination()
                });

            var winners = _combinationComparer
               .GetBestCombinations(playersCombinations.Select(x => x.Combination));

            Winners = playersCombinations.Where(x => winners.Contains(x.Combination))
                .Select(x => x.Player)
                .ToList();
        }
    }
}