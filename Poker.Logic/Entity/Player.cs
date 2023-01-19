using Poker.Logic.Entity;
using Poker.Logic.Exceptions;
using System;
using System.Collections.ObjectModel;

namespace Poker.Entity
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
            _cards = new Card[2];
        }
        public string Name { get; }

        private readonly Card[] _cards;
        public ReadOnlyCollection<Card> Cards => Array.AsReadOnly(_cards);
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
            Array.Clear(_cards);
        }
    }
}
