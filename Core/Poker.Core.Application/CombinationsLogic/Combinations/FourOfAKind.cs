using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{

    class FourOfAKind : Combination
    {
        public FourOfAKind(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override CombinationDTO GetCombination()
        {
            var fourOfAKind = GetFourOfAKind();
            var highestCard = _cards.Where(x => !fourOfAKind.Contains(x))
                .OrderByDescending(x => x.Value)
                .First();

            var result = fourOfAKind.ToList();
            result.Add(highestCard);
            return new CombinationDTO(ECombination.FourOfAKind, result);
        }

        public override bool ThereIsCombination()
        {
            return GetFourOfAKind() != null;

        }

        private IEnumerable<Card> GetFourOfAKind()
        {
            return _cards.GroupBy(x => x.Value)
                .Where(x => x.Count() == 4)
                .SingleOrDefault();
        }
    }
}
