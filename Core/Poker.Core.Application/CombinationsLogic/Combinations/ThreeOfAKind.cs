using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{
    class ThreeOfAKind : Combination
    {
        public ThreeOfAKind(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override CombinationDTO GetCombination()
        {
            var threeOfAKind = GetThreeOfAKind();
            var highestCards = _cards.Where(x => !threeOfAKind.Contains(x))
                .OrderByDescending(x => x.Value)
                .Take(2);

            var result = threeOfAKind.ToList();
            result.AddRange(highestCards);
            return new CombinationDTO(ECombination.ThreeOfAKind, result);
        }

        public override bool ThereIsCombination()
        {
            return GetThreeOfAKind() != null;
        }

        private IEnumerable<Card> GetThreeOfAKind()
        {
            return _cards.GroupBy(x => x.Value)
                .Where(x => x.Count() == 3)
                .OrderByDescending(x => x.Key)
                .FirstOrDefault();
        }
    }
}
