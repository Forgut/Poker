using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Interfaces;

namespace Poker.Core.Application.Betting.BetOrder
{
    class PlayerInfo
    {
        public PlayerInfo(IMoneyHolder player)
        {
            Player = player;
        }

        public IMoneyHolder Player { get; }
        public bool HasFolded { get; private set; }
        public void Fold() => HasFolded = true;
        public bool HasFinished { get; set; } = false;
    }
}
