using Poker.Entity;
using Poker.Logic;
using Poker.Logic.Estimator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    class Program
    {
        static void Main(string[] args)
        {
            var winners = new List<string>();
            //for (int i = 0; i < 10000; i++)
            //    winners.AddRange(ProcessGame(i));
            ProcessGame(2);
            foreach (var player in winners.GroupBy(x => x))
                Console.WriteLine($"{player.Key}: {player.Count()}");
        }

        static IEnumerable<string> ProcessGame(int seed)
        {
            var players = new List<Player>()
            {
                new Player("Fregi", 100),
                new Player("Szymeg", 100),
                new Player("Avamys", 100),
                new Player("Mietas", 100),
                new Player("JFK", 100),
                //new Player("Nataszka", 100),
            };

            var deckProvider = new DeckProvider();
            var deck = deckProvider.GetInitialDeck();
            var shuffledDeck = new DeckShuffler(new Random(seed)).ShuffleDeck(deck);
            var table = new Table(players);
            var croupier = new Croupier(shuffledDeck);
            var winEstimator = new WinEstimator(deckProvider);

            var targetPlayer = players[0];

            croupier.PreFlop(table);
            croupier.Flop(table);

            Console.WriteLine(table.GetTableState());

            var flopProb = winEstimator
                .ProbableCombinationsForPlayer2Missing(table, targetPlayer);

            var flopEnemyProb = winEstimator
                .ProbableCombinationsForEnemy2Missing(table, targetPlayer);

            Console.WriteLine($"Flop probability for {targetPlayer.Name}");
            Console.WriteLine(flopProb);
            Console.WriteLine();

            Console.WriteLine($"Flop probability for {targetPlayer.Name} enemies");
            Console.WriteLine(flopEnemyProb);
            Console.WriteLine();

            croupier.Turn(table);

            Console.WriteLine(table.GetTableState());

            var turnProb = winEstimator
                .ProbableCombinationsForPlayer1Missing(table, targetPlayer);

            var turnEnemyProb = winEstimator
                .ProbableCombinationsForEnemy1Missing(table, targetPlayer);

            Console.WriteLine($"Turn probability for {targetPlayer.Name}");
            Console.WriteLine(turnProb);
            Console.WriteLine();

            Console.WriteLine($"Turn probability for {targetPlayer.Name} enemies");
            Console.WriteLine(turnEnemyProb);
            Console.WriteLine();

            croupier.River(table);
            Console.WriteLine(table.GetTableState());

            var playersCombinations =
                players.Select(x => new
                {
                    Player = x,
                    Combination = new CombinationFinder(x, table).GetBestCombination()
                });

            foreach (var playerCombination in playersCombinations)
                Console.WriteLine($"{playerCombination.Player.Name}: {playerCombination.Combination}");

            var winners = new CombinationComparer()
                .GetBestCombinations(playersCombinations.Select(x => x.Combination));

            Console.WriteLine("Winners:");
            foreach (var winner in playersCombinations.Where(x => winners.Contains(x.Combination)))
                Console.Write($"{winner.Player.Name};");
            Console.WriteLine();

            Console.WriteLine("=======================");

            return playersCombinations.Where(x => winners.Contains(x.Combination)).Select(x => x.Player.Name);
        }
    }
}
