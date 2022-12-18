using Poker.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
{

    class TwoPair : CombinationBase
    {
        public TwoPair(IEnumerable<Card> cards)
            : base(cards)
        {
        }


        private IEnumerable<Card> GetTwoPairs()
        {
            return _cards.GroupBy(x => x.Value)
                .Where(x => x.Count() >= 2)
                .OrderByDescending(x => x.Key)
                .Take(2)
                .SelectMany(card => card)
                .OrderByDescending(x => x.Value);
        }

        public override bool ThereIsCombination()
        {
            return GetTwoPairs().Count() == 4;
        }

        public override CombinationDTO GetCombination()
        {
            var twoPairs = GetTwoPairs();
            var highestCardValue = _cards.Where(x => !twoPairs.Contains(x))
                .OrderByDescending(x => x.Value)
                .Take(1);

            var result = twoPairs.ToList();
            result.AddRange(highestCardValue);
            return new CombinationDTO(ECombination.TwoPair, result);
        }
    }
}
