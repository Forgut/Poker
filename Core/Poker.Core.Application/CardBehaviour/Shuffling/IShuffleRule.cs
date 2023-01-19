using System.Collections.Generic;
using Poker.Core.Domain.Entity;

namespace Poker.Core.Application.CardBehaviour.Shuffling
{
    public interface IShuffleRule
    {
        void ShuffleDeck(List<Card> cards);
    }
}
