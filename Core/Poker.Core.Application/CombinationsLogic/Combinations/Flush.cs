using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{
    class Flush : Combination
    {
        public Flush(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override CombinationDTO GetCombination()
        {
            return new CombinationDTO(ECombination.Flush, GetFlush());
        }

        public override bool ThereIsCombination()
        {
            return GetFlush() != null;
        }

        private IEnumerable<Card>? GetFlush()
        {
            var oneColorCards = _cards
                .GroupBy(x => x.Color)
                .Where(x => x.Count() >= 5)
                .FirstOrDefault();

            if (oneColorCards == null)
                return null;

            return oneColorCards.OrderByDescending(x => x.Value)
                .Take(5);
        }
    }
}
