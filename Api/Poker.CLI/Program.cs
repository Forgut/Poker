using Poker.CLI.Common;
using Poker.CLI.GameStateCreation;
using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isSimulation = false;
            if (args.Length > 0)
                bool.TryParse(args[0], out isSimulation);

            if (isSimulation)
                Console.WriteLine("Running in simulation mode");
            else
                Console.WriteLine("Running in standard mode");

            IGameState gameState;
            if (isSimulation)
            {
                gameState = GameStateFactory.Factory
                    .GetGameSimulation()
                    .WithDefaultCroupier()
                    .WithDefaultTable()
                    .WithDefaultWinEstimator()
                    .WithDefaultCombinationComparer()
                    //.WithPlayers(GetPlayers())
                    .WithRandomPlayers()
                    .WithNoEventPublisher()
                    .Build();
            }
            else
            {
                gameState = GameStateFactory.Factory
                    .GetStandardGame()
                    .WithDefaultTable()
                    .WithDefaultWinEstimator()
                    .WithDefaultCombinationComparer()
                    .WithPlayers(GetPlayers())
                    .WithNoEventPublisher()
                    .Build();
            }


            gameState.ExecuteAction("help");
            while (!gameState.ShouldEndGame)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                gameState.ExecuteAction(input);
            }
        }

        static IEnumerable<Player> GetPlayers()
        {
            return GetPlayerNames()
                .Select(x => new Player(x));

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
