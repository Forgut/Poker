using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{
    class OnePair : Combination
    {
        public OnePair(IEnumerable<Card> cards)
            : base(cards)
        {

        }

        public override bool ThereIsCombination()
        {
            return GetPair() != null;
        }

        public override CombinationDTO GetCombination()
        {
            var pair = GetPair();
            var highestValueCards = _cards.Where(x => !pair.Contains(x))
                .OrderByDescending(x => x.Value)
                .Take(3);

            var result = pair.ToList();
            result.AddRange(highestValueCards);

            return new CombinationDTO(ECombination.OnePair, result);
        }

        private IEnumerable<Card> GetPair()
        {
            return _cards.GroupBy(x => x.Value).FirstOrDefault(x => x.Count() == 2);
        }
    }
}
