using Poker.Core.Application.CombinationsLogic.Combinations;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic
{

    public class CombinationFinder
    {
        private List<Combination> _combinations;
        public CombinationFinder(Player player, Table table)
        {
            var cards = player.Cards.ToList();
            cards.AddRange(table.Cards);
            InitCombinations(cards);
        }

        public CombinationFinder(IEnumerable<Card> cards)
        {
            InitCombinations(cards);
        }

        public CombinationDTO GetBestCombination()
        {
            foreach (var combination in _combinations)
            {
                if (combination.ThereIsCombination())
                    return combination.GetCombination();
            }

            throw new CombinationNotFoundException();
        }

        private void InitCombinations(IEnumerable<Card> cards)
        {
            _combinations = new List<Combination>()
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
        }
    }
}
