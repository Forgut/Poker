using Poker.Core.Application;
using Poker.Core.Application.CardBehaviour;
using Poker.Core.Application.CardBehaviour.Shuffling;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameState = GetGameState();

            gameState.ExecuteAction("help");
            while (!gameState.ShouldEndGame)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                gameState.ExecuteAction(input);
            }
        }

        static GameState GetGameState()
        {
            var players = GetPlayerNames()
                .Select(x => new Player(x))
                .ToPlayers();

            var table = new Table();
            var deck = Deck.NewDeck
                .Shuffled(new ShuffleRule(new Random(1)));
            var croupier = new Croupier(deck);
            var winEstimator = new WinEstimator();
            var combinationComparer = new CombinationComparer();

            var game = new Game(croupier, combinationComparer, winEstimator, table, players);
            return new GameState(game);

            IEnumerable<string> GetPlayerNames()
            {
                Console.WriteLine("Insert players separated by ;");
                var names = Console.ReadLine();
                return names.Split(";")
                    .Select(x => x.Trim());
            }
        }
    }
}
