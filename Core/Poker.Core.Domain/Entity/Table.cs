using Poker.Core.Common;
using Poker.Core.Domain.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Core.Domain.Entity
{
    public interface ITable
    {
        ReadOnlyCollection<Card> Cards { get; }
        bool IsFullySet { get; }

        void ClearCards();
        void SetFifthCard(Card card);
        void SetFirstCard(Card card);
        void SetFourthCard(Card card);
        void SetSecondCard(Card card);
        void SetThirdCard(Card card);
    }

    public class Table : ITable
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
            Array.Clear(_cards, 0, _cards.Length);
        }

        public bool IsFullySet
            => _cards.ExceptNull().Count() == 5;
    }
}
