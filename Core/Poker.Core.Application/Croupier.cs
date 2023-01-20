using Poker.Core.Application.CardBehaviour;
using Poker.Core.Application.CardBehaviour.Shuffling;
using Poker.Core.Domain.Entity;
using System;

namespace Poker.Core.Application
{
    public class Croupier
    {
        private Deck _deck;
        private readonly Random _random;

        public Croupier(Random random)
        {
            _random = random;
        }

        public void PreFlop(Players players)
        {
            ResetDeck();
            foreach (var player in players)
                player.SetFirstCard(GetNextCardFromDeck());
            foreach (var player in players)
                player.SetSecondCard(GetNextCardFromDeck());
        }

        public void Flop(Table table)
        {
            table.SetFirstCard(GetNextCardFromDeck());
            table.SetSecondCard(GetNextCardFromDeck());
            table.SetThirdCard(GetNextCardFromDeck());
        }

        public void Turn(Table table)
        {
            table.SetFourthCard(GetNextCardFromDeck());
        }

        public void River(Table table)
        {
            table.SetFifthCard(GetNextCardFromDeck());
        }

        private Card GetNextCardFromDeck()
        {
            return _deck.DrawCard();
        }

        private void ResetDeck()
        {
            _deck = Deck.NewDeck.Shuffled(new ShuffleRule(_random));
        }
    }
}
