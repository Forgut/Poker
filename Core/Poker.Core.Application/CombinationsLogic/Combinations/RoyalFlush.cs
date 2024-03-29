﻿using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic.Combinations
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

        private List<Card>? GetRoyalFlush()
        {
            var straightFlush = GetStraightFlush();
            if (straightFlush?.First().Value == EValue.Ace)
                return straightFlush.ToList();
            return null;
        }
    }
}
