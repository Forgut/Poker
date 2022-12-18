using Poker.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
{

    class HighCard : CombinationBase
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
    class OnePair : CombinationBase
    {
        public OnePair(IEnumerable<Card> cards)
            : base(cards)
        {

        }

        public override bool ThereIsCombination()
        {
            return GetPair() != null;
        }

        public override CombinationDTO GetCombination()
        {
            var pair = GetPair();
            var highestValueCards = _cards.Where(x => !pair.Contains(x))
                .OrderByDescending(x => x.Value)
                .Take(3);

            var result = pair.ToList();
            result.AddRange(highestValueCards);

            return new CombinationDTO(ECombination.OnePair, result);
        }

        private IEnumerable<Card> GetPair()
        {
            return _cards.GroupBy(x => x.Value).FirstOrDefault(x => x.Count() == 2);
        }
    }
}
