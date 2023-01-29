using Poker.Core.Common;
using Poker.Core.Domain.Exceptions;
using Poker.Core.Domain.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Core.Domain.Entity
{
    public class Player : IMoneyHolder, ICardsHolder
    {
        public Player(string name, int initialMoney)
        {
            Name = name;
            Money = initialMoney;
            _cards = new Card[2];
        }
        public string Name { get; }
        public int Money { get; private set; }

        private readonly Card?[] _cards;
        public ReadOnlyCollection<Card?> Cards => Array.AsReadOnly(_cards);
        public bool HasCards => _cards.ExceptNull().Count() == 2;

        public void SetFirstCard(Card card)
        {
            if (_cards[0] != null)
                throw new CardAlreadySetException();
            _cards[0] = card;
        }

        public void SetSecondCard(Card card)
        {
            if (_cards[1] != null)
                throw new CardAlreadySetException();
            _cards[1] = card;
        }

        public void ClearCards()
        {
            Array.Clear(_cards, 0, _cards.Length);
        }

        public int TakeMoney(int amount)
        {
            if (amount > Money)
                amount = Money;
            Money -= amount;
            return amount;
        }

        public void AddMoney(int amount)
        {
            Money += amount;
        }
    }
}
