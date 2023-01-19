using Poker.Core.Application.Entity;
using Poker.Core.Domain.Entity;

namespace Poker.Core.Application.Logic
{
    public class Croupier
    {
        private readonly Deck _deck;

        public Croupier(Deck deck)
        {
            _deck = deck;
        }

        public void PreFlop(Players players)
        {
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
