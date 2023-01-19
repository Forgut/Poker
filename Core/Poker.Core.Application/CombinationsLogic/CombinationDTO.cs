using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic
{
    public class CombinationDTO : IComparable
    {
        public CombinationDTO(ECombination combination, IEnumerable<Card>? cards)
        {
            Combination = combination;
            Cards = cards ?? throw new ArgumentNullException(nameof(cards));
        }

        public ECombination Combination { get; }
        public IEnumerable<Card> Cards { get; }

        public override bool Equals(object obj)
        {
            var combination = (CombinationDTO)obj;

            return combination.Cards.Select(x => x.Value).SequenceEqual(Cards.Select(x => x.Value))
                && combination.Combination == Combination;
        }

        public int CompareTo(object obj)
        {
            var combination = (CombinationDTO)obj;

            var incomingCards = combination.Cards
                .OrderByDescending(x => x.Value)
                .ToList();

            var thisCards = Cards
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
