using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{
    class FullHouse : Combination
    {
        public FullHouse(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override CombinationDTO GetCombination()
        {
            return new CombinationDTO(ECombination.FullHouse, GetFullHouse());
        }

        public override bool ThereIsCombination()
        {
            return GetFullHouse() != null;
        }

        private IEnumerable<Card>? GetFullHouse()
        {
            var pair = _cards.GroupBy(x => x.Value)
                .FirstOrDefault(x => x.Count() == 2);

            if (pair == null)
                return null;

            var threeOfAKind = _cards
                .Where(x => !pair.Contains(x))
                .GroupBy(x => x.Value)
                .FirstOrDefault(x => x.Count() == 3);

            if (threeOfAKind == null)
                return null;

            var result = pair.ToList();
            result.AddRange(threeOfAKind);
            return result;
        }
    }
}
