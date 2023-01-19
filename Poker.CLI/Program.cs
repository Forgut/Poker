using Poker.Entity;
using Poker.Logic;
using Poker.Logic.Entity;
using Poker.Logic.Estimator;
using Poker.Logic.Logic;
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
                .Shuffled(new DeckShuffler(new Random(1)));
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

    class GameState
    {
        private readonly Game _game;

        public bool ShouldEndGame { get; private set; }

        public GameState(Game game)
        {
            _game = game;
        }

        public void ExecuteAction(string action)
        {
            switch (ParseAction(action))
            {
                case EAction.Quit:
                    ShouldEndGame = true;
                    break;
                case EAction.Reset:
                    ResetRound();
                    break;
                case EAction.Next:
                    NextRound();
                    break;
                case EAction.PrintWinProbabilities:
                    PrintWinProbabilities();
                    break;
                case EAction.SetupPlayer:
                    SetupTargetPlayer(ParseParameters(action).FirstOrDefault());
                    break;
                case EAction.PrintTable:
                    PrintTableState();
                    break;
                case EAction.AvamysShouts:
                    AvamysShouts();
                    break;
                case EAction.ClearConsole:
                    ClearConsole();
                    break;
                case EAction.Help:
                case EAction.Unkown:
                    PrintHelp();
                    break;
            }
        }

        private IEnumerable<string> ParseParameters(string action)
        {
            var parameters = action.Split(":")
                .Skip(1)
                .Select(x => x.Trim());
            return parameters;
        }

        private EAction ParseAction(string action)
        {
            var actionPrefix = action.Split(":")
                .First()
                .Trim();
            return actionPrefix switch
            {
                "h" or "help" => EAction.Help,
                "q" or "quit" => EAction.Quit,
                "r" or "reset" => EAction.Reset,
                "n" or "next" => EAction.Next,
                "p" or "print" => EAction.PrintWinProbabilities,
                "s" or "set" => EAction.SetupPlayer,
                "t" or "table" => EAction.PrintTable,
                "c" or "clear" => EAction.ClearConsole,
                "xd" => EAction.AvamysShouts,
                _ => EAction.Unkown,
            };
        }

        private void ClearConsole()
        {
            Console.Clear();
        }

        private void AvamysShouts()
        {
            Console.WriteLine("Avamys: \"Jestem WJEBANY! (big blind x2)\"");
        }

        private void ResetRound()
        {
            Console.WriteLine("Clearing the table");
            _game.ResetRound();
        }

        private void NextRound()
        {
            switch (_game.GameState)
            {
                case EGameState.New:
                    DrawCards();
                    break;
                case EGameState.PreFlop:
                    Flop();
                    break;
                case EGameState.Flop:
                    Turn();
                    break;
                case EGameState.Turn:
                    River();
                    break;
                case EGameState.River:
                    EndRound();
                    break;
                case EGameState.End:
                    _game.ResetRound();
                    break;
            }
        }

        private void DrawCards()
        {
            Console.WriteLine("Drawing cards");
            _game.DrawPlayerCards();
        }

        private void Flop()
        {
            Console.WriteLine("Flop round");
            _game.Flop();
        }

        private void Turn()
        {
            Console.WriteLine("Turn round");
            _game.Turn();
        }

        private void River()
        {
            Console.WriteLine("River round");
            _game.River();
        }

        private void EndRound()
        {
            Console.WriteLine("Game has ended");
            _game.EndRound();
            _game.PrintWinner();
            Console.WriteLine("Reset game to go again");
        }

        private void SetupTargetPlayer(string playerName)
        {
            Console.Write($"Setting up as target {playerName} ");
            if (_game.SetTargetPlayer(playerName))
                Console.WriteLine("succeded");
            else
                Console.WriteLine(" failed. Player with specified name not found");
        }

        private void PrintWinProbabilities()
        {
            _game.PrintWinProbabilities();
        }

        private void PrintTableState()
        {
            _game.PrintGameState();
        }

        private void PrintHelp()
        {
            Console.WriteLine("List of commands:\n" +
                "h help                 Displays list of commands\n" +
                "r reset                Resets the round\n" +
                "q quit                 Quits the game\n" +
                "n next                 Executes next round\n" +
                "p print                Prints win probabilties\n" +
                "s set                  Sets player as target player\n" +
                "t table                Prints table state\n" +
                "c clear                Clears console\n");
        }
    }

    enum EAction
    {
        Unkown = 0,
        Help = 1,
        Reset = 2,
        Next = 3,
        PrintWinProbabilities = 4,
        SetupPlayer = 5,
        PrintTable = 6,
        ClearConsole = 7,
        AvamysShouts = 998,
        Quit = 999,
    }
}
