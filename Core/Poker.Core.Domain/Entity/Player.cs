using Poker.Core.Common;
using Poker.Core.Domain.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Core.Domain.Entity
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
            _cards = new Card[2];
        }
        public string Name { get; }

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
    }
}
