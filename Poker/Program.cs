using Poker.Entity;
using Poker.Logic;
using Poker.Logic.Entity;
using Poker.Logic.Estimator;
using Poker.Logic.Logic;
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
            var players = new Players()
            {
                new Player("Fregi"),
                new Player("Szymeg"),
                new Player("Avamys"),
                new Player("Mietas"),
                new Player("JFK"),
                //new Player("Nataszka", 100),
            };


            var table = new Table();
            var deck = Deck.NewDeck
                .Shuffled(new ShuffleRule(new Random(1)));
            var croupier = new Croupier(deck);
            var winEstimator = new WinEstimator();
            var combinationComparer = new CombinationComparer();

            var game = new Game(croupier, combinationComparer, winEstimator, table, players);

            game.SetTargetPlayer(players[0].Name);

            game.DrawPlayerCards();
            game.Flop();

            game.PrintGameState();
            game.PrintWinProbabilities();

            game.Turn();

            game.PrintGameState();
            game.PrintWinProbabilities();

            game.River();
            game.PrintGameState();
            game.PrintWinProbabilities();

            game.EndRound();
            game.PrintWinner();

            return game.Winners.Select(x => x.Name);
        }
    }
}
