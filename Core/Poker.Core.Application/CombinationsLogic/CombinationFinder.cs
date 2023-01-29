using Poker.Core.Application.CombinationsLogic.Combinations;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Exceptions;
using Poker.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic
{
    public interface ICombinationFinder
    {
        CombinationDTO GetBestCombination(ICardsHolder player, ITable table);
        CombinationDTO GetBestCombination(IEnumerable<Card> cards);
    }

    public class CombinationFinder : ICombinationFinder
    {
        public CombinationDTO GetBestCombination(IEnumerable<Card> cards)
        {
            var combinations = new List<Combination>()
            {
                new RoyalFlush(cards),
                new StraightFlush(cards),
                new FourOfAKind(cards),
                new FullHouse(cards),
                new Flush(cards),
                new Straight(cards),
                new ThreeOfAKind(cards),
                new TwoPair(cards),
                new OnePair(cards),
                new HighCard(cards),
            };

            foreach (var combination in combinations)
                if (combination.ThereIsCombination())
                    return combination.GetCombination();

            throw new CombinationNotFoundException();
        }

        public CombinationDTO GetBestCombination(ICardsHolder player, ITable table)
        {
            var cards = player.Cards.ToList();
            cards.AddRange(table.Cards);
            if (cards.Any(x => x == null))
                throw new ArgumentNullException();
            return GetBestCombination(cards!);
        }
    }
}
