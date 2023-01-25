using Poker.Core.Domain.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Poker.Core.Application.Betting
{
    public class BetOverseer //todo name
    {
        private readonly PlayersRotation _playersRotation;
        private readonly Pot _pot;

        public BetOverseer(Players players)
        {
            _playersRotation = new PlayersRotation(players);
            _pot = new Pot();
        }

        public string GetCurrentlyBettingPlayer()
        {
            return _playersRotation.CurrentPlayer.Name;
        }

        public string GetSmallBlindPlayer()
        {
            return _playersRotation.SmallBlindPlayer.Name;
        }

        public string GetBigBlindPlayer()
        {
            return _playersRotation.BigBlindPlayer.Name;
        }

        public int GetAmountToCheck()
        {
            return _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
        }

        public int GetTotalAmountOnPot()
        {
            return _pot.GetTotalAmount();
        }

        private void ResetForNextBetRound()
        {
            _playersRotation.ResetPlayersTurns();
            _playersRotation.MoveToSmallBlind();
        }

        public void ResetForNewGame()
        {
            DidExecuteBigBlindAndSmallBlind = false;
            _playersRotation.ResetPlayersTurns();
            _pot.Reset();
        }

        public bool DidExecuteBigBlindAndSmallBlind { get; private set; }
        public void ExecuteBigAndSmallBlind(int bigBlindValue, int smallBlindValue)
        {
            _playersRotation.BigBlindPlayer.TakeMoney(bigBlindValue);
            _pot.AddToPot(_playersRotation.BigBlindPlayer.Name, bigBlindValue);

            _playersRotation.SmallBlindPlayer.TakeMoney(smallBlindValue);
            _pot.AddToPot(_playersRotation.SmallBlindPlayer.Name, smallBlindValue);

            _playersRotation.MoveToPlayerAfterBigBlind();
            DidExecuteBigBlindAndSmallBlind = true;
        }

        public IEnumerable<Player> GetNotFoldedPlayers()
        {
            return _playersRotation.GetNotFoldedPlayers();
        }

        public (bool BetSucceeded, bool IsBettingOver) ExecuteForCurrentPlayer(string input)
        {
            var (decision, amount) = DecisionParser.Parse(input);

            if (decision == Decision.Unkown)
                return (false, false);
            if (amount <= 0)
                return (false, false);

            bool betSucceeded;
            switch (decision)
            {
                case Decision.Check:
                    betSucceeded = Check();
                    break;
                case Decision.Raise:
                    betSucceeded = Raise(amount!.Value);
                    break;
                case Decision.Fold:
                    betSucceeded = true;
                    Fold();
                    break;
                default:
                    betSucceeded = false;
                    break;
            }

            if (!betSucceeded)
                return (false, false);

            _playersRotation.MoveToNextNotFoldedPlayer();

            var isBettingOver = _playersRotation.IsBettingOver();
            if (isBettingOver)
                ResetForNextBetRound();

            return (betSucceeded, isBettingOver);
        }

        private bool Check()
        {
            var amount = _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
            if (_playersRotation.CurrentPlayer.Money < amount)
                return false;

            _playersRotation.CurrentPlayer.TakeMoney(amount);
            _pot.AddToPot(_playersRotation.CurrentPlayer.Name, amount);
            _playersRotation.MarkCurrentPlayerAsFinished();
            return true;
        }

        private bool Raise(int amount)
        {
            var amountToCheck = _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
            var totalAmount = amountToCheck + amount;
            if (_playersRotation.CurrentPlayer.Money < totalAmount)
                return false;

            _playersRotation.CurrentPlayer.TakeMoney(totalAmount);
            _pot.AddToPot(_playersRotation.CurrentPlayer.Name, totalAmount);
            _playersRotation.MarkNotFoldedPlayersAsNotFinished();
            _playersRotation.MarkCurrentPlayerAsFinished();
            return true;
        }

        private void Fold()
        {
            _playersRotation.MarkCurrentPlayerAsFolded();
        }

        public void MoveBlinds()
        {
            _playersRotation.MoveBlinds();
        }
    }
}
