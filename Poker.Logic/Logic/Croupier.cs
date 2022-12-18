using Poker.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
{
    public class Croupier
    {
        private List<Card> _deck;

        public Croupier(DeckProvider deckProvider, DeckShuffler deckShuffler)
        {
            var initialDeck = deckProvider.GetInitialDeck();
            _deck =  deckShuffler.ShuffleDeck(initialDeck).ToList();
        }

        public void PreFlop(Table table)
        {
            var players = table.Players;
            foreach (var player in players)
                player.Cards[0] = GetNextCardFromDeck();
            foreach (var player in players)
                player.Cards[1] = GetNextCardFromDeck();
        }

        public void Flop(Table table)
        {
            table.Cards[0] = GetNextCardFromDeck();
            table.Cards[1] = GetNextCardFromDeck();
            table.Cards[2] = GetNextCardFromDeck();
        }

        public void Turn(Table table)
        {
            table.Cards[3] = GetNextCardFromDeck();
        }

        public void River(Table table)
        {
            table.Cards[4] = GetNextCardFromDeck();
        }

        private Card GetNextCardFromDeck()
        {
            var card = _deck.First();
            _deck.Remove(card);
            return card;
        }
    }
}
