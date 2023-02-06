using Poker.Core.Common;
using Poker.Core.Domain.Entity.Snapshot;
using Poker.Core.Domain.Exceptions;
using Poker.Core.Domain.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

        private Player()
        {

        }

        public string Name { get; private set; }
        public int Money { get; private set; }

        private Card?[] _cards;
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

        public PlayerSnapshot ToSnapshot()
        {
            return new PlayerSnapshot()
            {
                Name = this.Name,
                Money = this.Money,
                Cards = this.Cards.Select(x => x?.ToSnapshot()),
            };
        }

        public static Player EmptyPlayer =>
            new Player();

        public Player FromSnapshot(PlayerSnapshot snapshot)
        {
            this.Money = snapshot.Money;
            this.Name = snapshot.Name;
            this._cards = snapshot.Cards.Select(x => x.HasValue ? Card.EmptyCard.FromSnapshot(x.Value) : null).ToArray();
            return this;
        }
    }
}
