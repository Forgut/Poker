using Poker.Core.Domain.Entity;
using System.Collections.ObjectModel;

namespace Poker.Core.Domain.Interfaces
{
    public interface ICardsHolder : IPlayer
    {
        ReadOnlyCollection<Card?> Cards { get; }
        bool HasCards { get; }

        void ClearCards();
        void SetFirstCard(Card card);
        void SetSecondCard(Card card);
    }
}
