using Poker.Core.Application.Entity;
using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.Logic.Combinations
{
    class Straight : CombinationBase
    {
        public Straight(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override CombinationDTO GetCombination()
        {
            return new CombinationDTO(ECombination.Straight, GetStraight());
        }

        public override bool ThereIsCombination()
        {
            return GetStraight() != null;
        }

        private bool StraightCount(IEnumerable<Card> cards)
        {
            return !cards
                .Where((i, index) => index > 0 && cards.ElementAt(index - 1).Value != i.Value + 1)
                .Any();
        }

        private IEnumerable<Card> GetStraight()
        {
            var orderedDistinct = _cards.GroupBy(x => x.Value)
                .Select(x => x.First())
                .OrderByDescending(x => x.Value);

            if (orderedDistinct.Count() >= 5 && StraightCount(orderedDistinct.Take(5)))
                return orderedDistinct.Take(5);
            if (orderedDistinct.Count() >= 6 && StraightCount(orderedDistinct.Skip(1).Take(5)))
                return orderedDistinct.Skip(1).Take(5);
            if (orderedDistinct.Count() >= 7 && StraightCount(orderedDistinct.Skip(2).Take(5)))
                return orderedDistinct.Skip(2).Take(5);
            return null;
        }
    }
}
