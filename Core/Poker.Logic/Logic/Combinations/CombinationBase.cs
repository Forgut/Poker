using Poker.Entity;
using Poker.Logic.Entity;
using System.Collections.Generic;

namespace Poker.Logic
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
