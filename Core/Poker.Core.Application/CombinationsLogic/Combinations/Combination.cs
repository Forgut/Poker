using Poker.Core.Domain.Entity;
using System.Collections.Generic;

namespace Poker.Core.Application.CombinationsLogic.Combinations
{
    abstract class Combination
    {
        protected IEnumerable<Card> _cards;

        protected Combination(IEnumerable<Card> cards)
        {
            _cards = cards;
        }

        public abstract bool ThereIsCombination();
        public abstract CombinationDTO GetCombination();
    }
}
