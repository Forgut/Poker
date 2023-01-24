﻿using Poker.CLI.Common;
using Poker.CLI.Input;
using Poker.Core.Application;
using Poker.Core.Application.GameBehaviour;
using System;
using System.Linq;

namespace Poker.CLI.Standard
{
    class GameState : IGameState
    {
        private readonly StandardGame _game;
        private readonly IInputProivder _inputProivder;

        public bool ShouldEndGame { get; private set; }

        public GameState(StandardGame game, IInputProivder inputProivder)
        {
            _game = game;
            _inputProivder = inputProivder;
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
                    SetupTargetPlayer();
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
                case EGameState.PreFlop:
                    DrawPlayerCards();
                    break;
                case EGameState.Flop:
                    Flop();
                    break;
                case EGameState.Turn:
                    Turn();
                    break;
                case EGameState.River:
                    River();
                    break;
                case EGameState.PreFlopBet:
                case EGameState.FlopBet:
                case EGameState.TurnBet:
                case EGameState.RiverBet:
                    Bet();
                    break;
                case EGameState.ShowCards:
                    ShowCards();
                    break;
                case EGameState.End:
                    EndRound();
                    _game.ResetRound();
                    break;
            }
        }

        private void DrawPlayerCards()
        {
            Console.WriteLine("Pre flop. Type in cards separated with ;");
            var cards = _inputProivder.ReadLine();
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
            var cards = _inputProivder.ReadLine();
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
            var card = _inputProivder.ReadLine();
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
            var card = _inputProivder.ReadLine();
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
            Console.WriteLine(_game.GetWinnersAsString());
            Console.WriteLine("Reset game to go again");
        }

        private void SetupTargetPlayer()
        {
            Console.WriteLine("Type in new target player name");
            var playerName = _inputProivder.ReadLine();
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

        private void Bet()
        {
            Console.WriteLine(_game.GetCurrentBetInfo());
            var decision = _inputProivder.ReadLine();
            _game.Bet(decision);
        }

        private void ShowCards()
        {
            Console.WriteLine("Fill other players cards in order to calculate winner.\n" +
                "Players with no cards will not be included in calculations");
            _game.SetGameStateAsEnd();
        }

        private void FillPlayerHand()
        {
            Console.WriteLine("Type player name");
            var playerName = _inputProivder.ReadLine();
            Console.WriteLine("Type in player cards separated with ;");
            var cards = _inputProivder.ReadLine();
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
