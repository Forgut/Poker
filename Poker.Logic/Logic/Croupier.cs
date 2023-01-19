using Poker.Entity;
using Poker.Logic.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
{
    public class Croupier
    {
        private readonly Deck _deck;

        public Croupier(Deck deck)
        {
            _deck = deck;
        }

        public void PreFlop(Table table)
        {
            var players = table.Players;
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
    }
}
