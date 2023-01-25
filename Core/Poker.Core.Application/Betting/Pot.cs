using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.Betting
{
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

        public void Reset()
        {
            _transactions.Clear();
        }
    }
}
