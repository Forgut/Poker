using Poker.Core.Application.CardBehaviour;
using Poker.Core.Application.CardBehaviour.Shuffling;
using Poker.Core.Domain.Entity;
using System;

namespace Poker.Core.Application
{
    public interface ICroupier
    {
        void Flop(ITable table);
        void PreFlop(Players players);
        void River(ITable table);
        void Turn(ITable table);
    }

    public class Croupier : ICroupier
    {
        private Deck? _deck;
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

        public void Flop(ITable table)
        {
            table.SetFirstCard(GetNextCardFromDeck());
            table.SetSecondCard(GetNextCardFromDeck());
            table.SetThirdCard(GetNextCardFromDeck());
        }

        public void Turn(ITable table)
        {
            table.SetFourthCard(GetNextCardFromDeck());
        }

        public void River(ITable table)
        {
            table.SetFifthCard(GetNextCardFromDeck());
        }

        private Card GetNextCardFromDeck()
        {
            return _deck!.DrawCard();
        }

        private void ResetDeck()
        {
            _deck = Deck.NewDeck.Shuffled(new ShuffleRule(_random));
        }
    }
}
