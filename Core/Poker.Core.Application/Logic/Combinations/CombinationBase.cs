using Poker.Core.Application.Entity;
using Poker.Core.Domain.Entity;
using System.Collections.Generic;

namespace Poker.Core.Application.Logic.Combinations
{
    abstract class CombinationBase
    {
        protected IEnumerable<Card> _cards;

        protected CombinationBase(IEnumerable<Card> cards)
        {
            _cards = cards;
        }

        public abstract bool ThereIsCombination();
        public abstract CombinationDTO GetCombination();
    }
}
