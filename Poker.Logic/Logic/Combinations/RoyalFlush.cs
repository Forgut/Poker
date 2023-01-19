using Poker.Entity;
using Poker.Logic.Cards.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
{
    class RoyalFlush : StraightFlush
    {
        public RoyalFlush(IEnumerable<Card> cards) : base(cards)
        {
        }

        public override CombinationDTO GetCombination()
        {
            return new CombinationDTO(ECombination.RoyalFlush, GetRoyalFlush());
        }

        public override bool ThereIsCombination()
        {
            return GetRoyalFlush() != null;
        }

        private IEnumerable<Card> GetRoyalFlush()
        {
            var straightFlush = GetStraightFlush();
            if (straightFlush?.First().Value == EValue.Ace)
                return straightFlush;
            return null;
        }
    }
}
