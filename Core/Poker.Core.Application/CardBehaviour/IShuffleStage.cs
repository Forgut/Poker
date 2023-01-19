using Poker.Core.Application.CardBehaviour.Shuffling;

namespace Poker.Core.Application.CardBehaviour
{
    public interface IShuffleStage
    {
        Deck Shuffled(IShuffleRule shuffleRule);
        Deck NotShuffled();
    }
}
