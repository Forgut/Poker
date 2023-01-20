using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{
    class StraightFlush : Combination
    {
        public StraightFlush(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override bool ThereIsCombination()
        {
            return GetStraightFlush() != null;
        }

        public override CombinationDTO GetCombination()
        {
            return new CombinationDTO(ECombination.StraightFlush, GetStraightFlush());
        }

        protected IEnumerable<Card>? GetStraightFlush()
        {
            var ordered = _cards
                .OrderBy(x => x.Color).ThenByDescending(x => x.Value);

            if (StraightFlushCount(ordered.Take(5)))
                return ordered.Take(5).OrderByDescending(x => x.Value);
            if (StraightFlushCount(ordered.Skip(1).Take(5)))
                return ordered.Skip(1).Take(5).OrderByDescending(x => x.Value);
            if (StraightFlushCount(ordered.Skip(2).Take(5)))
                return ordered.Skip(2).Take(5).OrderByDescending(x => x.Value);
            return null;
        }

        private bool StraightFlushCount(IEnumerable<Card> cards)
        {
            return !cards
                .Where((i, index) => index > 0
                    && (cards.ElementAt(index - 1).Value != i.Value + 1
                    || cards.ElementAt(index - 1).Color != i.Color))
                .Any();
        }

    }
}
