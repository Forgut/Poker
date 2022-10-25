using Poker.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
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

        private IEnumerable<Card> GetStraight()
        {
            var ordered = _cards.OrderByDescending(x => x.Value);

            if (StraightCount(ordered.Take(5)))
                return ordered.Take(5);
            if (StraightCount(ordered.Skip(1).Take(5)))
                return ordered.Skip(1).Take(5);
            if (StraightCount(ordered.Skip(2).Take(5)))
                return ordered.Skip(2).Take(5);
            return null;
        }

        private bool StraightCount(IEnumerable<Card> cards)
        {
            return !cards
                .Where((i, index) => index > 0 && cards.ElementAt(index - 1).Value != i.Value + 1)
                .Any();
        }
    }
}
