using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{
    class HighCard : Combination
    {
        public HighCard(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override CombinationDTO GetCombination()
        {
            var highestValue = _cards.OrderByDescending(x => x.Value)
                .Take(5);
            return new CombinationDTO(ECombination.HighCard, highestValue);
        }

        public override bool ThereIsCombination()
        {
            return true;
        }
    }
}
