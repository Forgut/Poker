using Poker.Core.Application.CardBehaviour.Shuffling;
using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Core.Application.CardBehaviour
{
    public class Deck : IShuffleStage
    {
        private readonly List<Card> _cards;
        public ReadOnlyCollection<Card> Cards => _cards.AsReadOnly();

        private Deck()
        {
            _cards = GetFullDeck()
                .ToList();
        }

        public Card DrawCard()
        {
            var card = _cards.First();
            _cards.Remove(card);
            return card;
        }

        #region Fluent api

        public static IShuffleStage NewDeck
           => new Deck();

        public Deck Shuffled(IShuffleRule shuffleRule)
        {
            shuffleRule.ShuffleDeck(_cards);
            return this;
        }

        public Deck NotShuffled()
        {
            return this;
        }

        private IEnumerable<Card> GetFullDeck()
        {
            foreach (EColor color in Enum.GetValues(typeof(EColor)))
                foreach (EValue value in Enum.GetValues(typeof(EValue)))
                    yield return new Card(value, color);
        }

        #endregion
    }
}
