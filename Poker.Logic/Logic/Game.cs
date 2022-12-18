using Poker.Entity;
using Poker.Logic.Estimator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic.Logic
{
    public class Game
    {
        private readonly Table _table;
        private readonly Croupier _croupier;
        private readonly CombinationComparer _combinationComparer;
        private readonly WinEstimator _winEstimator;

        private Player _targetPlayer;

        public List<Player> Winners { get; private set; }
        public EGameState GameState { get; private set; }

        public Game(Croupier croupier,
                    CombinationComparer combinationComparer,
                    WinEstimator winEstimator,
                    Table table)
        {
            _croupier = croupier;
            _combinationComparer = combinationComparer;
            _winEstimator = winEstimator;
            _table = table;
            Winners = new List<Player>();
        }

        public bool SetTargetPlayer(string playerName)
        {
            var player = _table.Players.FirstOrDefault(x => x.Name == playerName);
            if (player != null)
            {
                _targetPlayer = player;
                return true;
            }

            _targetPlayer = _table.Players.First();
            return false;
        }

        public void ResetRound()
        {
            Array.Clear(_table.Cards, 0, _table.Cards.Length);
            foreach (var player in _table.Players)
                Array.Clear(player.Cards, 0, player.Cards.Length);
            Winners.Clear();
            GameState = EGameState.New;
        }

        public void DrawPlayerCards()
        {
            _croupier.PreFlop(_table);
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
            Console.WriteLine($"Current stage: {GameState}");
            Console.WriteLine(_table.GetTableState());
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
                _table.Players.Select(x => new
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
                _table.Players.Select(x => new
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

    public enum EGameState
    {
        New = 0,
        PreFlop = 1,
        Flop = 2,
        Turn = 3,
        River = 4,
        End = 5,
    }
}
