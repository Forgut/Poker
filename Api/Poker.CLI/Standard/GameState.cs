using Poker.Core.Application;
using System.Collections.Generic;
using System.Linq;
using System;
using Poker.CLI.Common;
using Poker.Core.Application.GameBehaviour;

namespace Poker.CLI.Standard
{
    class GameState : IGameState
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
                case EAction.FillPlayerHand:
                    FillPlayerHand();
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
                "f" or "fill" => EAction.FillPlayerHand,
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
                    DrawPlayerCards();
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
                    ShowCards();
                    break;
                case EGameState.ShowCards:
                    EndRound();
                    break;
                case EGameState.End:
                    _game.ResetRound();
                    break;
            }
        }

        private void DrawPlayerCards()
        {
            Console.WriteLine("Pre flop. Type in cards separated with ;");
            var cards = Console.ReadLine();
            try
            {
                _game.InsertTargetPlayerCards(cards);
            }
            catch
            {
                Console.WriteLine("Error occured");
            }
        }

        private void Flop()
        {
            Console.WriteLine("Flop round. Type in cards separated with ;");
            var cards = Console.ReadLine();
            try
            {
                _game.Flop(cards);
            }
            catch
            {
                Console.WriteLine("Error occured");
            }
        }

        private void Turn()
        {
            Console.WriteLine("Turn round. Type in card.");
            var card = Console.ReadLine();
            try
            {
                _game.Turn(card);
            }
            catch
            {
                Console.WriteLine("Error occured");
            }
        }

        private void River()
        {
            Console.WriteLine("River round. Type in card.");
            var card = Console.ReadLine();
            try
            {
                _game.River(card);

            }
            catch
            {
                Console.WriteLine("Error occured");
            }
        }

        private void EndRound()
        {
            Console.WriteLine("Game has ended. Type in other player cards to establish a winner");
            _game.EndRound();
            Console.WriteLine(_game.GetWinnerAsString());
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
            Console.WriteLine(_game.GetWinProbabilitiesAsString());
        }

        private void PrintTableState()
        {
            Console.WriteLine(_game.GameStateAsString());
        }

        private void ShowCards()
        {
            Console.WriteLine("Fill other players cards in order to calculate winner.\n" +
                "Players with no cards will not be included in calculations");
            _game.ShowCards();
        }

        private void FillPlayerHand()
        {
            Console.WriteLine("Type player name");
            var playerName = Console.ReadLine();
            Console.WriteLine("Type in player cards separated with ;");
            var cards = Console.ReadLine();
            try
            {
                _game.FillPlayersCards(playerName, cards);
            }
            catch
            {
                Console.WriteLine("Error occured");
            }
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
                "c clear                Clears console\n" +
                "f fill                 Sets selected player hand\n");
        }
    }
}
