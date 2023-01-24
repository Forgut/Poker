﻿using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetAmountToCheck()
        {
            return _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
        }

        public int GetTotalAmountOnPot()
        {
            return _pot.GetTotalAmount();
        }

        public bool IsBettingOver()
        {
            return _playersRotation.IsBettingOver();
        }

        public void ResetForNextRound()
        {
            _playersRotation.ResetPlayersTurns();
        }

        public bool ExecuteForCurrentPlayer(string input) //todo name
        {
            var (decision, amount) = DecisionParser.Parse(input);

            if (decision == Decision.Unkown)
                return false;
            if (amount <= 0)
                return false;

            switch (decision)
            {
                case Decision.Check:
                    return Check();
                case Decision.Raise:
                    return Raise(amount!.Value);
                case Decision.Fold:
                    Fold();
                    return true;
            }
            return false;
        }

        private bool Check()
        {
            var amount = _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
            if (_playersRotation.CurrentPlayer.Money < amount)
                return false;

            _playersRotation.CurrentPlayer.TakeMoney(amount);
            _pot.AddToPot(_playersRotation.CurrentPlayer.Name, amount);
            _playersRotation.MarkCurrentPlayerAsFinished();
            _playersRotation.MoveToNextNotFoldedPlayer();
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
            _playersRotation.MoveToNextNotFoldedPlayer();
            return true;
        }

        private void Fold()
        {
            _playersRotation.MarkCurrentPlayerAsFolded();
            _playersRotation.MoveToNextNotFoldedPlayer();
        }

        public void MoveBlinds()
        {
            _playersRotation.MoveBlinds();
        }
    }

    public class Pot
    {
        private readonly List<(string PlayerName, int Amount)> _transactions;

        public Pot()
        {
            _transactions = new List<(string PlayerName, int Amount)>();
        }

        public int AmountToCheck(string playerName)
        {
            var sumOfPlayerBets = _transactions
                .GroupBy(x => x.PlayerName)
                .SingleOrDefault(x => x.Key == playerName)
                ?.Sum(x => x.Amount) ?? 0;

            var highestBet = _transactions
                .GroupBy(x => x.PlayerName)
                .Select(x => x.Sum(x => x.Amount))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            return highestBet - sumOfPlayerBets;
        }
        public void AddToPot(string playerName, int amount)
        {
            _transactions.Add((playerName, amount));
        }

        public int GetTotalAmount()
        {
            return _transactions.Sum(x => x.Amount);
        }
    }
}
