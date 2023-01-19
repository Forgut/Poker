using Poker.Entity;
using Poker.Logic.Entity;
using Poker.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
{

    public class CombinationFinder
    {
        private List<CombinationBase> _combinations;
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
            _combinations = new List<CombinationBase>()
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
