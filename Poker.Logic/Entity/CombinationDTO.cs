using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Entity
{
    public class CombinationDTO : IComparable
    {
        public CombinationDTO(ECombination combination, IEnumerable<Card> cards)
        {
            Combination = combination;
            Cards = cards;
        }

        public ECombination Combination { get; }
        public IEnumerable<Card> Cards { get; }

        public override bool Equals(object obj)
        {
            var combination = (CombinationDTO)obj;

            return combination.Cards.Select(x => x.Value).SequenceEqual(this.Cards.Select(x => x.Value))
                && combination.Combination == this.Combination;
        }

        public int CompareTo(object obj)
        {
            var combination = (CombinationDTO)obj;

            var incomingCards = combination.Cards
                .OrderByDescending(x => x.Value)
                .ToList();

            var thisCards = this.Cards
                .OrderByDescending(x => x.Value)
                .ToList();

            for (int i = 0; i < incomingCards.Count(); i++)
            {
                if (incomingCards[i].Value > thisCards[i].Value)
                    return -1;
                if (incomingCards[i].Value < thisCards[i].Value)
                    return 1;
            }
            return 0;
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Cards)} ({Combination})";
        }
    }
}
