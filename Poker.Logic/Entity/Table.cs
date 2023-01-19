using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Poker.Logic.Entity;
using Poker.Logic.Exceptions;

namespace Poker.Entity
{
    public class Table
    {
        public Table()
        {
            _cards = new Card[5];
        }
        private Card[] _cards { get; }
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

        public void SetThirdCard(Card card)
        {
            if (_cards[2] != null)
                throw new CardAlreadySetException();
            _cards[2] = card;
        }

        public void SetFourthCard(Card card)
        {
            if (_cards[3] != null)
                throw new CardAlreadySetException();
            _cards[3] = card;
        }

        public void SetFifthCard(Card card)
        {
            if (_cards[4] != null)
                throw new CardAlreadySetException();
            _cards[4] = card;
        }

        public void ClearCards()
        {
            Array.Clear(_cards);
        }
    }
}
